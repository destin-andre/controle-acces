using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ControleAcces.Api.Models
{
    // Cette classe représente un droit d'accès dans le système.
    // Chaque propriété correspond à une colonne de la table "DroitsAcces" en base de données,
    // telle que définie dans le MCD (docs/bdd/).
    public class DroitAcces
    {
        [Key]
        public int IdDroit { get; set; }

        // Date à partir de laquelle ce droit d'accès devient valide.
        public DateTime DateDebut { get; set; }

        // Date à partir de laquelle ce droit d'accès n'est plus valide.
        public DateTime DateFin { get; set; }

        // Clé étrangère vers le badge concerné par ce droit.
        public int IdBadge { get; set; }

        // Propriété de navigation vers le badge associé.
        [ForeignKey(nameof(IdBadge))]
        public Badge? Badge { get; set; }

        // Clé étrangère vers la zone concernée par ce droit.
        public int IdZone { get; set; }

        // Propriété de navigation vers la zone associée.
        [ForeignKey(nameof(IdZone))]
        public Zone? Zone { get; set; }
    }
}