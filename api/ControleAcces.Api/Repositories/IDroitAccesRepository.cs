using ControleAcces.Api.Models;
namespace ControleAcces.Api.Repositories
{
    // Cette interface définit les opérations disponibles pour manipuler des droits d'accès,
    // sans préciser comment elles sont réalisées concrètement.
    // Cela permet, par exemple, de remplacer facilement PostgreSQL par une autre base plus tard,
    // sans avoir à modifier le code qui utilise cette interface (les Services).
    public interface IDroitAccesRepository
    {
        // Récupère la liste de tous les droits d'accès.
        Task<List<DroitAcces>> GetAllAsync();

        // Récupère un droit d'accès précis à partir de son identifiant.
        // Peut renvoyer null si aucun droit d'accès ne correspond.
        Task<DroitAcces?> GetByIdAsync(int id);

        // Ajoute un nouveau droit d'accès en base de données.
        Task<DroitAcces> CreateAsync(DroitAcces droitAcces);

        // Met à jour un droit d'accès existant.
        Task UpdateAsync(DroitAcces droitAcces);

        // Supprime un droit d'accès à partir de son identifiant.
        Task DeleteAsync(int id);
    }
}