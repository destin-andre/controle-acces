using ControleAcces.Api.Models;
namespace ControleAcces.Api.Repositories
{
    // Cette interface définit les opérations disponibles pour manipuler des zones,
    // sans préciser comment elles sont réalisées concrètement.
    // Cela permet, par exemple, de remplacer facilement PostgreSQL par une autre base plus tard,
    // sans avoir à modifier le code qui utilise cette interface (les Services).
    public interface IZoneRepository
    {
        // Récupère la liste de toutes les zones.
        Task<List<Zone>> GetAllAsync();

        // Récupère une zone précise à partir de son identifiant.
        // Peut renvoyer null si aucune zone ne correspond.
        Task<Zone?> GetByIdAsync(int id);

        // Ajoute une nouvelle zone en base de données.
        Task<Zone> CreateAsync(Zone zone);

        // Met à jour une zone existante.
        Task UpdateAsync(Zone zone);

        // Supprime une zone à partir de son identifiant.
        Task DeleteAsync(int id);
    }
}