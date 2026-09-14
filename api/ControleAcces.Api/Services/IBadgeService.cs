using ControleAcces.Api.Models;

namespace ControleAcces.Api.Services
{
    // Cette interface définit les opérations métier disponibles pour les badges.
    // Contrairement au Repository, qui ne fait que lire/écrire en base sans réfléchir,
    // le Service peut appliquer des règles avant d'agir (validation, vérifications, etc.).
    public interface IBadgeService
    {
        Task<List<Badge>> GetAllAsync();
        Task<Badge?> GetByIdAsync(int id);
        Task<Badge> CreateAsync(Badge badge);
        Task UpdateAsync(int id, Badge badge);
        Task DeleteAsync(int id);
    }
}