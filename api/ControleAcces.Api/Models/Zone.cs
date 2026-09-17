using System.ComponentModel.DataAnnotations;
namespace ControleAcces.Api.Models

{
    // Cette classe représente une zone dans le système.
    // Chaque propriété correspond à une colonne de la table "Zones" en base de données,
    // telle que définie dans le MCD (docs/bdd/).
    public class Zone
    {
        // [Key] indique explicitement à Entity Framework que cette propriété est la clé primaire.
        // C'est nécessaire ici car son nom (IdZone) ne suit pas la convention automatique
        // d'Entity Framework, qui attend "Id" ou "ZoneId".
        [Key]
        public int IdZone { get; set; }

        // Nom de la zone.
        public string NomZone { get; set; } = string.Empty;

        // Niveau de sécurité de la zone.
        public string NiveauSecurite { get; set; } = string.Empty;
    }
}