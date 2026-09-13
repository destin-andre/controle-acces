# Architecture globale du système de contrôle d'accès

## Introduction

Ce document décrit l'architecture technique globale du système de contrôle d'accès physique développé dans le cadre de ce projet. Il explique comment les différents composants matériels et logiciels communiquent entre eux, pourquoi cette organisation a été choisie, et quelles implications elle a sur la sécurité et l'évolutivité du système. Il s'adresse à toute personne souhaitant comprendre le fonctionnement d'ensemble sans nécessairement entrer dans le détail de chaque brique technique, ce qui sera fait séparément dans la documentation propre à la base de données, à l'API et à l'infrastructure.

L'idée directrice de cette architecture est de séparer clairement les responsabilités entre trois grands ensembles fonctionnels, chacun isolé dans sa propre zone réseau. Cette séparation n'est pas qu'une commodité d'organisation du code ou du matériel : elle constitue un principe de sécurité à part entière, connu sous le nom de segmentation réseau, qui limite la surface d'attaque en cas de compromission d'un des composants.

## Vue d'ensemble des trois zones

Le système est découpé en trois zones distinctes, que l'on retrouvera plus tard concrètement sous forme de VLAN séparés dans l'infrastructure Proxmox. La première zone, dite zone embarquée, regroupe le matériel physique installé au niveau des portes ou points d'accès à sécuriser. La deuxième zone héberge l'API, qui constitue le cœur logique du système et le seul point de passage autorisé entre le matériel de terrain et les données sensibles. La troisième zone concentre les composants les plus critiques du point de vue de la sécurité, à savoir la base de données et l'annuaire LDAP, et bénéficie à ce titre des restrictions d'accès les plus strictes.

Cette organisation en trois zones répond à une logique simple mais essentielle. Le matériel embarqué, physiquement accessible à toute personne se trouvant près d'une porte, ne doit jamais avoir un accès direct aux données sensibles de l'entreprise. S'il était compromis ou trafiqué, un attaquant ne pourrait interagir qu'avec l'API, laquelle applique ses propres règles de validation et de journalisation avant de transmettre quoi que ce soit aux systèmes internes. La base de données et l'annuaire, à l'inverse, ne sont jamais exposés directement à l'extérieur du réseau interne, ce qui réduit considérablement les vecteurs d'attaque possibles.

## La zone embarquée

Cette zone correspond au matériel installé physiquement à chaque point d'accès contrôlé. Elle comprend un microcontrôleur, Arduino ou Raspberry Pi selon le choix retenu, connecté à un lecteur de badge RFID ainsi qu'à un capteur d'empreinte digitale. Le rôle de ce matériel se limite volontairement à des tâches simples : lire l'identifiant du badge présenté, capturer les données biométriques nécessaires, puis transmettre ces informations à l'API par le réseau.

Aucune logique de décision n'est effectuée à ce niveau. Le matériel embarqué ne sait pas si l'accès doit être autorisé ou refusé, il se contente de collecter l'information et de poser la question à l'API. Ce choix est délibéré et découle directement du principe de sécurité évoqué plus haut : en ne stockant aucune information sensible sur le droit d'accès directement sur le boîtier physique, on empêche qu'une personne malveillante puisse extraire cette logique en démontant ou en piratant le matériel installé sur une porte.

## La zone API

L'API, développée en C# selon une architecture en couches, constitue le point de passage obligatoire entre le monde physique et les données internes du système. Chaque tentative d'accès en provenance de la zone embarquée y transite avant qu'une décision ne soit rendue. C'est également l'API qui porte la responsabilité de la journalisation systématique de chaque tentative, qu'elle soit couronnée de succès ou non.

Le choix d'une architecture en couches, avec une séparation nette entre les contrôleurs qui reçoivent les requêtes, les services qui portent la logique métier, et les dépôts qui accèdent aux données, répond à un objectif de maintenabilité et de testabilité du code. Chaque couche peut être modifiée ou testée indépendamment des autres, ce qui facilite l'évolution du système au fil du temps sans risquer de casser des fonctionnalités existantes.

Sur le plan de la sécurité, l'API joue également un rôle de filtre. C'est elle qui valide la cohérence des données reçues avant de les transmettre plus loin, qui applique les règles métier telles que les horaires d'accès autorisés, et qui décide en dernier ressort si la porte doit s'ouvrir ou rester fermée. Aucun composant en amont ne peut contourner cette étape de validation.

## La zone base de données et annuaire

Cette dernière zone regroupe les deux composants qui portent l'information la plus sensible du système. La base de données relationnelle conserve l'ensemble des données structurelles telles que les utilisateurs, les badges, les droits d'accès par zone physique, et l'historique complet des tentatives d'accès. Elle est configurée en réplication primaire secondaire, ce qui garantit la disponibilité du service même en cas de défaillance d'un des deux nœuds hébergeant la base.

L'annuaire LDAP, quant à lui, centralise la gestion des identités et des groupes d'utilisateurs. Ce choix suit une pratique répandue en entreprise, où l'authentification et la gestion des comptes reposent sur un système d'annuaire unique plutôt que sur une base de données propre à chaque application. Cela facilite la gestion centralisée des comptes, notamment lors de l'arrivée ou du départ d'un collaborateur, puisqu'une seule modification dans l'annuaire se répercute sur l'ensemble des systèmes qui s'y réfèrent.

Ces deux composants ne sont jamais accessibles directement depuis l'extérieur du réseau interne. Seule l'API, positionnée dans sa propre zone, dispose des droits nécessaires pour les interroger. Cette restriction d'accès est renforcée par la segmentation réseau, qui empêche techniquement toute tentative de connexion directe en provenance d'une autre zone, y compris la zone embarquée.

## Le déroulement d'une tentative d'accès

Pour bien comprendre l'intérêt de cette architecture, il est utile de dérouler le parcours complet d'une tentative d'accès, depuis le moment où une personne présente son badge jusqu'à la décision finale d'ouvrir ou non la porte.

La séquence commence lorsque le lecteur RFID installé sur la porte détecte un badge à proximité et en lit l'identifiant unique. Le capteur biométrique associé capture simultanément l'empreinte digitale de la personne. Ces deux informations sont regroupées par le microcontrôleur, qui les transmet ensuite à l'API via une requête HTTP standard, à travers le réseau segmenté propre à la zone embarquée.

L'API reçoit cette requête et engage alors une série de vérifications. Elle interroge d'abord l'annuaire LDAP afin de confirmer que le badge correspond bien à un utilisateur actif et d'en récupérer les groupes d'appartenance. Elle consulte ensuite la base de données pour vérifier que ce badge dispose effectivement d'un droit d'accès valide pour la zone physique concernée, en tenant compte le cas échéant des dates de validité ou des plages horaires autorisées. Si l'ensemble de ces vérifications aboutit favorablement, l'API renvoie une réponse positive au matériel embarqué, qui peut alors déverrouiller la porte.

Indépendamment du résultat, qu'il soit positif ou négatif, l'API enregistre systématiquement la tentative dans la table des journaux d'accès, en conservant l'identifiant du badge utilisé, la zone concernée, l'horodatage précis, la méthode d'authentification employée et le résultat obtenu. Cette journalisation systématique constitue une exigence de traçabilité essentielle dans tout système de contrôle d'accès physique, qu'il s'agisse de répondre à un incident de sécurité ou simplement de produire des rapports d'utilisation.

## Pourquoi cette séparation en zones plutôt qu'une architecture plus simple

Il aurait été techniquement possible de concevoir un système plus simple, où le matériel embarqué communiquerait directement avec la base de données, sans passer par une API intermédiaire. Ce choix a été écarté pour plusieurs raisons qui méritent d'être explicitées, notamment parce qu'elles reflètent des préoccupations concrètes rencontrées dans les systèmes professionnels équivalents.

La première raison tient à la sécurité. Exposer directement une base de données à du matériel installé dans des lieux physiquement accessibles constituerait une faille majeure, puisque toute personne parvenant à extraire les identifiants de connexion du microcontrôleur obtiendrait un accès direct aux données sensibles de l'ensemble du système, et non plus seulement à une porte isolée. En centralisant l'accès aux données à travers une API dédiée, on réduit ce risque à sa plus simple expression : même en cas de compromission complète d'un boîtier physique, l'attaquant ne pourrait interagir qu'avec les fonctionnalités exposées par l'API, sans jamais atteindre directement la base de données ou l'annuaire.

La deuxième raison concerne la cohérence des règles métier. En centralisant la logique de décision dans l'API, on garantit que toutes les portes du système appliquent exactement les mêmes règles de validation, sans risque de divergence entre différentes implémentations embarquées sur différents boîtiers. Toute évolution des règles d'accès, par exemple l'ajout d'une nouvelle condition horaire, ne nécessite qu'une modification unique côté API, sans avoir à redéployer du code sur chaque microcontrôleur installé sur le terrain.

Enfin, cette architecture facilite également la montée en charge et l'évolution future du système. L'ajout d'une nouvelle porte se limite à l'installation d'un nouveau boîtier configuré pour dialoguer avec l'API existante, sans modification de l'infrastructure centrale. De la même manière, l'API pourrait à terme être dupliquée sur plusieurs instances pour répartir la charge, sans que cela n'affecte en rien le fonctionnement du matériel embarqué déjà déployé.

## Perspectives d'évolution

Cette architecture, telle que présentée ici, correspond à l'état actuel du projet à l'issue de sa phase de conception. Plusieurs évolutions sont d'ores et déjà prévues dans les phases suivantes du développement. La segmentation réseau actuellement décrite au niveau conceptuel sera concrètement mise en œuvre à travers la création de VLAN distincts sur l'infrastructure Proxmox, accompagnée de règles de pare-feu strictes n'autorisant que les flux explicitement nécessaires entre les zones. Un tunnel VPN viendra également sécuriser les communications entre les différents nœuds de l'infrastructure.

L'ensemble du déploiement de cette architecture, de la création des machines virtuelles jusqu'à la configuration des règles réseau, sera par ailleurs automatisé à travers des outils d'infrastructure as code, à savoir Ansible pour la configuration des services et Terraform pour le provisionnement des ressources. Cette automatisation garantira que l'architecture décrite dans ce document pourra être recréée de manière fiable et reproductible, un aspect particulièrement valorisé dans les environnements professionnels modernes.
