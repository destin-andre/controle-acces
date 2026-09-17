using ControleAcces.Api.Models;
namespace ControleAcces.Api.Repositories

{
    // Cette interface définit les opérations disponibles pour manipuler des empreintes,
    // sans préciser comment elles sont réalisées concrètement.
    // Cela permet, par exemple, de remplacer facilement PostgreSQL par une autre base plus tard,
    // sans avoir à modifier le code qui utilise cette interface (les Services).
    public interface IEmpreinteRepository
    {
        // Récupère la liste de toutes les empreintes.
        Task<List<Empreinte>> GetAllAsync();

        // Récupère une empreinte précise à partir de son identifiant.
        // Peut renvoyer null si aucune empreinte ne correspond.
        Task<Empreinte?> GetByIdAsync(int id);

        // Ajoute une nouvelle empreinte en base de données.
        Task<Empreinte> CreateAsync(Empreinte empreinte);

        // Met à jour une empreinte existante.
        Task UpdateAsync(Empreinte empreinte);

        // Supprime une empreinte à partir de son identifiant.
        Task DeleteAsync(int id);
    }
}