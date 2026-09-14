using ControleAcces.Api.Models;
namespace ControleAcces.Api.Repositories
{
    // Cette interface définit les opérations disponibles pour manipuler des badges,
    // sans préciser comment elles sont réalisées concrètement.
    // Cela permet, par exemple, de remplacer facilement PostgreSQL par une autre base plus tard,
    // sans avoir à modifier le code qui utilise cette interface (les Services).
    public interface IBadgeRepository
    {
        // Récupère la liste de tous les badges.
        Task<List<Badge>> GetAllAsync();

        // Récupère un badge précis à partir de son identifiant.
        // Peut renvoyer null si aucun badge ne correspond.
        Task<Badge?> GetByIdAsync(int id);

        // Ajoute un nouveau badge en base de données.
        Task<Badge> CreateAsync(Badge badge);

        // Met à jour un badge existant.
        Task UpdateAsync(Badge badge);

        // Supprime un badge à partir de son identifiant.
        Task DeleteAsync(int id);
    }
}