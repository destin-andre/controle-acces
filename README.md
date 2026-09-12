# Système de contrôle d'accès physique sécurisé

## Description

Ce projet implémente un système de contrôle d'accès physique par badge RFID,
renforcé par une authentification biométrique simple. Il couvre l'ensemble
de la chaîne technique : de la lecture matérielle du badge jusqu'à
l'infrastructure serveur qui héberge et sécurise les données d'accès.

L'objectif est de proposer une solution complète et réaliste, comparable à
ce qu'on retrouverait en entreprise pour sécuriser l'accès à une salle
serveur, un bureau sensible ou une zone restreinte.

## Objectifs du projet

- Mettre en pratique une architecture complète : embarqué → API → base de
  données → annuaire → infrastructure
- Manipuler des outils d'infrastructure as code (Ansible, Terraform) sur un
  cluster de virtualisation réel (Proxmox)
- Appliquer les bonnes pratiques de conception logicielle (architecture en
  couches, principes POO) côté backend
- Sécuriser les flux réseau entre les différents composants (VPN,
  segmentation)

## Fonctionnement général

1. Un utilisateur badge à l'entrée d'une zone (lecteur RFID connecté à un
   Arduino ou Raspberry Pi)
2. Une vérification biométrique par empreinte digitale complète
   l'authentification
3. Le module embarqué transmet l'identifiant du badge à l'API via HTTP
4. L'API vérifie les droits d'accès de l'utilisateur pour cette zone
   (interrogation de l'annuaire LDAP/AD)
5. La décision (accès autorisé / refusé) est renvoyée au module embarqué et
   journalisée dans la base de données

## Architecture technique

**Partie embarquée**
- Arduino ou Raspberry Pi
- Lecteur RFID/badge + capteur d'empreinte digitale
- Communication HTTP vers l'API

**Backend / API**
- Développée en C#
- Architecture en couches : Controllers / Services / Repositories / Models
- Expose les droits d'accès et journalise les tentatives

**Base de données**
- SGBD relationnel (PostgreSQL ou MySQL)
- Modélisation : utilisateurs, badges, droits, zones, horodatage, logs
  d'accès
- Réplication primaire/secondaire pour la disponibilité

**Annuaire**
- LDAP ou Active Directory
- Gestion centralisée des utilisateurs et de leurs groupes de droits

**Infrastructure**
- Cluster Proxmox (2 nœuds physiques)
- VMs dédiées : annuaire (LDAP/AD), API, base de données
- Déploiement automatisé via Ansible (configuration) et Terraform
  (provisionnement)
- VPN et segmentation réseau (VLAN) entre les zones embarqué / API /
  BDD-LDAP

