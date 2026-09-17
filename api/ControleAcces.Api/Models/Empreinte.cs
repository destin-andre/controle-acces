using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ControleAcces.Api.Models

{
    // Cette classe représente une empreinte dans le système.
    // Chaque propriété correspond à une colonne de la table "Empreintes" en base de données,
    // telle que définie dans le MCD (docs/bdd/).
    public class Empreinte
    {
        // [Key] indique explicitement à Entity Framework que cette propriété est la clé primaire.
        // C'est nécessaire ici car son nom (IdEmpreinte) ne suit pas la convention automatique
        // d'Entity Framework, qui attend "Id" ou "EmpreinteId".
        [Key]
        public int IdEmpreinte { get; set; }

        // Données binaires représentant l'empreinte digitale.
        public byte[] TemplatesBiometrique { get; set; } = Array.Empty<byte>();

        // Date d'enrolement du badge du badge.
        public DateTime DateEnrolement { get; set; }

        // Clé étrangère vers l'utilisateur auquel l'empreinte est associée.
        public int IdUtilisateur { get; set; }

        // Navigation property vers l'utilisateur associé à l'empreinte.
        [ForeignKey(nameof(IdUtilisateur))]
        public Utilisateur? Utilisateur { get; set; }
    }
}