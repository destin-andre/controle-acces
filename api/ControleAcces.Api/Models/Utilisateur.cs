using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ControleAcces.Api.Models
{
    // Cette classe représente un utilisateur dans le système.
    // Chaque propriété correspond à une colonne de la table "Utilisateurs" en base de données,
    // telle que définie dans le MCD (docs/bdd/).
    public class Utilisateur
    {
        // [Key] indique explicitement à Entity Framework que cette propriété est la clé primaire.
        // C'est nécessaire ici car son nom (IdUtilisateur) ne suit pas la convention automatique
        // d'Entity Framework, qui attend "Id" ou "UtilisateurId".
        [Key]
        public int IdUtilisateur { get; set; }

        // Nom de famille de l'utilisateur.
        public string Nom { get; set; } = string.Empty;

        // Prénom de l'utilisateur.
        public string Prenom { get; set; } = string.Empty;

        // Adresse email professionnelle.
        public string Email { get; set; } = string.Empty;

        // Poste occupé par l'utilisateur dans l'entreprise.
        public string Poste { get; set; } = string.Empty;

        // Indique si l'utilisateur est encore actif (true) ou a quitté l'entreprise (false).
        // Par défaut, un nouvel utilisateur est considéré comme actif.
        public bool EnActivite { get; set; } = true;

        // Un utilisateur peut avoir au maximum une empreinte.
        [JsonIgnore]
        public Empreinte? Empreinte { get; set; }
    }
}