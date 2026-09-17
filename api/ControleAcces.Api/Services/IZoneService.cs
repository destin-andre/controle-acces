using ControleAcces.Api.Models;

namespace ControleAcces.Api.Services
{
    // Cette interface définit les opérations métier disponibles pour les zones.
    // Contrairement au Repository, qui ne fait que lire/écrire en base sans réfléchir,
    // le Service peut appliquer des règles avant d'agir (validation, vérifications, etc.).
    public interface IZoneService
    {
        Task<List<Zone>> GetAllAsync();
        Task<Zone?> GetByIdAsync(int id);
        Task<Zone> CreateAsync(Zone zone);
        Task UpdateAsync(int id, Zone zone);
        Task DeleteAsync(int id);
    }
}
