using ControleAcces.Api.Models;
namespace ControleAcces.Api.Repositories
{
    // Cette interface définit les opérations disponibles pour manipuler des logs d'accès,
    // sans préciser comment elles sont réalisées concrètement.
    // Cela permet, par exemple, de remplacer facilement PostgreSQL par une autre base plus tard,
    // sans avoir à modifier le code qui utilise cette interface (les Services).
    public interface ILogAccesRepository
    {
        // Récupère la liste de tous les logs d'accès.
        Task<List<LogAcces>> GetAllAsync();

        // Récupère un log d'accès précis à partir de son identifiant.
        // Peut renvoyer null si aucun log ne correspond.
        Task<LogAcces?> GetByIdAsync(int id);

        // Ajoute un nouveau log d'accès en base de données.
        Task<LogAcces> CreateAsync(LogAcces logAcces);

        // Met à jour un log d'accès existant.
        Task UpdateAsync(LogAcces logAcces);

        // Supprime un log d'accès à partir de son identifiant.
        Task DeleteAsync(int id);
    }
}