# Référence des commandes utilisées

Ce document regroupe toutes les commandes tapées dans le terminal depuis le début du projet, groupées par outil, avec une explication de ce que fait chacune. À consulter en cas de doute ou pour se rafraîchir la mémoire sans avoir à chercher dans l'historique des sessions.

## Git

| Commande | Ce qu'elle fait |
|---|---|
| `git --version` | Affiche la version de Git installée, permet de vérifier qu'il est bien installé. |
| `git config --global user.name "..."` | Définit le nom associé à tous tes futurs commits, sur toutes machines confondues. |
| `git config --global user.email "..."` | Définit l'email associé à tous tes futurs commits. |
| `git config --global --list` | Affiche la configuration Git actuelle (nom, email, etc.). |
| `git init` | Transforme le dossier courant en dépôt Git, crée le dossier caché `.git`. |
| `git status` | Affiche l'état actuel du dépôt : fichiers modifiés, non suivis, prêts à être commités. |
| `git add .` | Ajoute tous les fichiers modifiés/créés au prochain commit (équivalent du bouton "+" dans VS Code). |
| `git add <fichier>` | Ajoute un fichier précis au prochain commit. |
| `git commit -m "message"` | Crée un commit avec tous les fichiers ajoutés, accompagné du message donné. |
| `git branch -M main` | Renomme la branche courante en `main`. |
| `git remote add origin <url>` | Relie le dépôt local à un dépôt distant sur GitHub, nommé `origin`. |
| `git push -u origin main` | Envoie les commits vers GitHub pour la première fois, et mémorise ce lien pour les prochains push. |
| `git push` | Envoie les nouveaux commits vers GitHub (une fois le lien déjà établi par la commande précédente). |
| `git log` | Affiche l'historique des commits (auteur, date, message). |

## Réseau / diagnostic Windows

| Commande | Ce qu'elle fait |
|---|---|
| `ping <adresse>` | Teste si une adresse (site ou IP) répond, et mesure le temps de réponse. |
| `ipconfig /flushdns` | Vide le cache DNS de Windows, force une nouvelle résolution des noms de domaine. |
| `nslookup <domaine>` | Interroge le DNS pour voir à quelle adresse IP correspond un nom de domaine. |
| `netstat -ano \| findstr :<port>` | Liste les processus qui écoutent sur un port réseau précis, avec leur PID. |
| `tasklist /FI "PID eq <numéro>"` | Affiche le nom du programme correspondant à un PID (identifiant de processus). |

## .NET / dotnet

| Commande | Ce qu'elle fait |
|---|---|
| `dotnet --version` | Affiche la version du SDK .NET installée. |
| `dotnet new webapi -n <nom> --use-controllers` | Génère un nouveau projet Web API, dans sa variante avec contrôleurs classiques. |
| `dotnet run` | Compile et démarre l'application (l'API), affiche l'adresse d'écoute. |
| `dotnet add package <nom>` | Ajoute une dépendance externe (bibliothèque) au projet. |
| `dotnet tool install --global dotnet-ef` | Installe l'outil en ligne de commande de gestion des migrations Entity Framework. |
| `dotnet ef migrations add <NomMigration>` | Génère une nouvelle migration à partir des changements détectés dans les modèles. |
| `dotnet ef database update` | Applique les migrations en attente à la base de données réelle. |
| `dotnet ef migrations remove` | Annule la dernière migration générée (si elle n'a pas encore été appliquée). |

## Docker

| Commande | Ce qu'elle fait |
|---|---|
| `docker compose up -d` | Démarre les services décrits dans `docker-compose.yml`, en arrière-plan. |
| `docker compose down` | Arrête et supprime les conteneurs, sans toucher aux données stockées. |
| `docker compose down -v` | Arrête et supprime les conteneurs ET leurs volumes de données (repart de zéro). |
| `docker ps` | Liste les conteneurs actuellement en cours d'exécution. |
| `docker logs <nom-conteneur>` | Affiche les journaux (logs) d'un conteneur, utile en cas de comportement anormal. |
| `docker exec -it <nom-conteneur> <commande>` | Exécute une commande à l'intérieur d'un conteneur en cours d'exécution. |
| `docker exec <nom-conteneur> env` | Affiche les variables d'environnement définies dans un conteneur. |

## PostgreSQL (une fois entré dans le conteneur via docker exec)

| Commande | Ce qu'elle fait |
|---|---|
| `psql -U <utilisateur> -d <base>` | Ouvre l'invite de commande PostgreSQL, connecté à une base précise. |
| `\dt` | Liste les tables de la base actuellement connectée. |
| `\q` | Quitte l'invite de commande PostgreSQL. |
| `SELECT * FROM "NomTable";` | Affiche toutes les lignes d'une table (requête SQL standard). |

## Remarques

Le conteneur Docker de la base de données ne redémarre pas automatiquement après l'arrêt de Docker Desktop ou un redémarrage du PC. Le réflexe à avoir en début de chaque session de travail est de lancer `docker ps` pour vérifier que la base tourne, et `docker compose up -d` si ce n'est pas le cas.

Toute modification apportée à `Program.cs`, aux modèles, ou à tout autre fichier C# nécessite un redémarrage du serveur (`Ctrl+C` puis `dotnet run`) pour être prise en compte. .NET ne recharge pas le code à chaud pendant qu'il tourne avec `dotnet run` simple.
