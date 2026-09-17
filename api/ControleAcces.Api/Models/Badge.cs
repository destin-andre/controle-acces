using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ControleAcces.Api.Models
{
    // Cette classe représente un badge dans le système.
    // Chaque propriété correspond à une colonne de la table "Badges" en base de données,
    // telle que définie dans le MCD (docs/bdd/).
    public class Badge
    {
        // [Key] indique explicitement à Entity Framework que cette propriété est la clé primaire.
        // C'est nécessaire ici car son nom (IdBadge) ne suit pas la convention automatique
        // d'Entity Framework, qui attend "Id" ou "BadgeId".
        [Key]
        public int IdBadge { get; set; }

        // Numéro unique du badge.
        public string UidBadge { get; set; } = string.Empty;

        // Date d'atribution du badge du badge.
        public DateTime DateAttribution { get; set; }

        // Indique si le badge est actif (true) ou désactivé (false).
        public bool Actif { get; set; } = true;

        // Clé étrangère vers l'utilisateur auquel le badge est attribué.
        public int IdUtilisateur { get; set; }

        // Navigation property vers l'utilisateur associé au badge.
        [ForeignKey(nameof(IdUtilisateur))]
        public Utilisateur? Utilisateur { get; set; }
    }
}