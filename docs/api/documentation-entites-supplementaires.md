# Ajout des entités Zone, DroitAcces et LogAcces

## Introduction

Ce document fait suite à la documentation initiale du squelette de l'API, qui couvrait la mise en place générale du projet ainsi que la première entité complète, Utilisateur, accompagnée de Badge. Il retrace la suite de ce travail, à savoir l'ajout des trois entités restantes du modèle conceptuel de données, Zone, DroitAcces et LogAcces, ainsi qu'une difficulté technique importante rencontrée en cours de route concernant la gestion des relations entre les tables, et sa résolution.

## Ajout de Zone

L'entité Zone a été la plus simple à mettre en place, puisqu'elle ne comporte aucune relation vers une autre entité, contrairement à Badge qui dépend d'Utilisateur. Les quatre couches habituelles ont été créées en suivant exactement le même schéma que pour les entités précédentes, à savoir un modèle dans le dossier Models, une interface et son implémentation dans Repositories, une interface et son implémentation dans Services, et un contrôleur exposant les cinq routes standard dans Controllers.

Une attention particulière a dû être portée au nom de la classe du contrôleur. En ASP.NET Core, le nom de la route d'un contrôleur est dérivé automatiquement du nom de sa classe, en retirant le mot Controller à la fin. Une classe nommée ZoneController, sans la lettre s à la fin de Zone, produit ainsi une route accessible à l'adresse /api/zone au singulier, et non /api/zones comme cela aurait pu être supposé par analogie avec les autres entités. Ce point a nécessité une clarification lors des tests, le premier essai ayant échoué avec une erreur 404 simplement parce que l'adresse testée dans Postman ne correspondait pas à la route réellement générée.

## Ajout de DroitAcces

L'entité DroitAcces introduit la première relation double du projet, puisqu'elle référence à la fois un badge et une zone, avec deux clés étrangères distinctes. Son modèle comporte ainsi deux propriétés simples, IdBadge et IdZone, chacune accompagnée d'une propriété de navigation vers l'entité correspondante. La règle métier appliquée dans le service vérifie que la date de fin d'un droit d'accès est bien postérieure à sa date de début, une vérification simple mais essentielle pour garantir la cohérence des données.

## Ajout de LogAcces

L'entité LogAcces suit la même logique que DroitAcces, avec également deux clés étrangères vers Badge et Zone, en plus de ses propres champs horodatage, méthode d'authentification utilisée et résultat de la tentative d'accès. Cette entité a également permis d'introduire une amélioration technique intéressante au niveau du repository, à travers l'utilisation de la méthode Include fournie par Entity Framework Core.

Par défaut, lorsqu'une entité est récupérée depuis la base de données, les objets liés par clé étrangère ne sont pas automatiquement chargés avec elle, seule la valeur brute de l'identifiant l'est. La méthode Include permet de demander explicitement à Entity Framework de charger également l'objet complet correspondant à une relation, ce qui a été fait pour les relations Badge et Zone dans les méthodes de lecture du repository de LogAcces. Cette amélioration permet à l'API de renvoyer directement les informations complètes du badge et de la zone concernés par un log, sans que le client de l'API ait besoin d'effectuer des requêtes supplémentaires pour les récupérer séparément.

## La difficulté rencontrée avec les clés étrangères

Une fois LogAcces mis en place et testé, un problème est apparu malgré l'utilisation de la méthode Include décrite ci-dessus : les objets badge et zone renvoyés dans la réponse restaient obstinément vides, alors même que les identifiants numériques correspondants étaient correctement enregistrés en base de données. Ce même problème, non identifié comme tel à l'époque, avait d'ailleurs déjà été observé plus tôt sur l'entité DroitAcces, sans qu'une explication complète n'ait été trouvée sur le moment.

L'origine du problème s'est révélée être une convention de nommage propre à Entity Framework Core. Ce dernier ne reconnaît automatiquement une propriété comme étant la clé étrangère d'une relation que si son nom suit un format précis, à savoir le nom de la classe cible suivi du mot Id, par exemple BadgeId. Or, l'ensemble du projet suit la convention inverse héritée du modèle conceptuel de données initial, à savoir IdBadge plutôt que BadgeId. Cette même inversion de convention avait déjà nécessité l'ajout de l'attribut Key sur chaque clé primaire, mais son impact sur les clés étrangères et les relations entre tables n'avait pas été anticipé.

En l'absence de reconnaissance automatique, Entity Framework Core avait silencieusement créé, en arrière-plan, une colonne technique supplémentaire pour gérer chaque relation, totalement déconnectée de la propriété IdBadge ou IdZone visible dans le code. La valeur de cette dernière était donc bien enregistrée en base de données de façon indépendante, mais la relation réelle, exploitée par la méthode Include, ne s'appuyait pas dessus, ce qui explique que les objets liés restaient vides malgré une valeur d'identifiant apparemment correcte.

La correction a consisté à ajouter l'attribut ForeignKey sur chaque propriété de navigation concernée, en lui indiquant explicitement quelle propriété simple sert de clé étrangère pour cette relation. Cette correction a dû être appliquée sur toutes les entités du projet comportant une relation vers une autre, à savoir Badge, DroitAcces et LogAcces. Dans la mesure où des colonnes techniques incorrectes avaient déjà été créées par les migrations précédentes, la décision a été prise de repartir d'une base de données entièrement vide plutôt que de tenter de corriger la structure existante, jugée plus risquée et plus longue à mettre en œuvre correctement. Le dossier de migrations a ainsi été supprimé, le conteneur Docker de la base de données recréé depuis zéro avec son volume de données, puis une nouvelle migration initiale a été générée et appliquée, cette fois avec les relations correctement définies dès le départ.

## Séparation des secrets de configuration

En parallèle de ce travail sur les entités, une amélioration a été apportée à la gestion du mot de passe de connexion à la base de données. Celui-ci se trouvait initialement directement dans le fichier appsettings.json, qui fait partie des fichiers suivis par Git et donc envoyés sur le dépôt distant. Cette pratique, bien que sans risque réel dans le cas présent puisqu'il s'agit d'un mot de passe de développement local protégeant une base de données jamais exposée à l'extérieur, ne correspond pas aux bonnes pratiques à adopter, notamment en vue d'un futur environnement de production où de véritables secrets seraient en jeu.

La chaîne de connexion a donc été déplacée vers un second fichier, appsettings.Development.json, chargé automatiquement par l'application uniquement lorsqu'elle s'exécute en environnement de développement, et volontairement exclu du suivi Git à travers le fichier .gitignore du projet. Le fichier appsettings.json d'origine ne contient désormais plus que la configuration générique et sans risque de l'application.

## État du projet à l'issue de cette phase

À ce stade, les cinq entités principales du modèle conceptuel de données disposent chacune d'un modèle, d'un repository, d'un service et d'un contrôleur complets, avec un jeu d'opérations de création, lecture, modification et suppression fonctionnel et testé à travers Postman. Les relations entre les tables sont correctement établies et exploitées. Il ne s'agit toutefois encore que d'un ensemble d'opérations élémentaires sur chaque table prise isolément. La logique métier propre au système de contrôle d'accès, à savoir la vérification effective des droits d'un badge sur une zone donnée avant d'autoriser ou de refuser un passage, reste entièrement à construire, et constitue la prochaine étape du développement.
