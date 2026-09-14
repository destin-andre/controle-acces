using ControleAcces.Api.Models;

namespace ControleAcces.Api.Repositories
{
    // Cette interface définit les opérations disponibles pour manipuler des utilisateurs,
    // sans préciser comment elles sont réalisées concrètement.
    // Cela permet, par exemple, de remplacer facilement PostgreSQL par une autre base plus tard,
    // sans avoir à modifier le code qui utilise cette interface (les Services).
    public interface IUtilisateurRepository
    {
        // Récupère la liste de tous les utilisateurs.
        Task<List<Utilisateur>> GetAllAsync();

        // Récupère un utilisateur précis à partir de son identifiant.
        // Peut renvoyer null si aucun utilisateur ne correspond.
        Task<Utilisateur?> GetByIdAsync(int id);

        // Ajoute un nouvel utilisateur en base de données.
        Task<Utilisateur> CreateAsync(Utilisateur utilisateur);

        // Met à jour un utilisateur existant.
        Task UpdateAsync(Utilisateur utilisateur);

        // Supprime un utilisateur à partir de son identifiant.
        Task DeleteAsync(int id);
    }
}