using ControleAcces.Api.Models;

namespace ControleAcces.Api.Services
{
    // Cette interface définit les opérations métier disponibles pour les utilisateurs.
    // Contrairement au Repository, qui ne fait que lire/écrire en base sans réfléchir,
    // le Service peut appliquer des règles avant d'agir (validation, vérifications, etc.).
    public interface IUtilisateurService
    {
        Task<List<Utilisateur>> GetAllAsync();
        Task<Utilisateur?> GetByIdAsync(int id);
        Task<Utilisateur> CreateAsync(Utilisateur utilisateur);
        Task UpdateAsync(int id, Utilisateur utilisateur);
        Task DeleteAsync(int id);
    }
}
