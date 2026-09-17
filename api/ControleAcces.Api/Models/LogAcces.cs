using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControleAcces.Api.Models
{
    // Cette classe représente un log d'accès dans le système.
    // Chaque propriété correspond à une colonne de la table "LogsAcces" en base de données,
    // telle que définie dans le MCD (docs/bdd/).
    public class LogAcces
    {
        // [Key] indique explicitement à Entity Framework que cette propriété est la clé primaire.
        [Key]
        public int IdLog { get; set; }

        // Date et heure de l'accès.
        public DateTime Horodatage { get; set; }

        public string Methode { get; set; } = string.Empty;
        public string Resultat { get; set; } = string.Empty;

        // Clé étrangère vers le badge utilisé pour l'accès.
        public int IdBadge { get; set; }

        // [ForeignKey] indique explicitement que IdBadge est la clé étrangère
        // correspondant à cette relation, car son nom ne suit pas la convention
        // automatique attendue par Entity Framework (BadgeId).
        [ForeignKey(nameof(IdBadge))]
        public Badge? Badge { get; set; }

        // Clé étrangère vers la zone concernée par l'accès.
        public int IdZone { get; set; }

        [ForeignKey(nameof(IdZone))]
        public Zone? Zone { get; set; }
    }
}