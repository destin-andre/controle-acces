using ControleAcces.Api.Models;

namespace ControleAcces.Api.Services
{
    // Cette interface définit les opérations métier disponibles pour les droits d'accès.
    // Contrairement au Repository, qui ne fait que lire/écrire en base sans réfléchir,
    // le Service peut appliquer des règles avant d'agir (validation, vérifications, etc.).
    public interface IDroitAccesService
    {
        Task<List<DroitAcces>> GetAllAsync();
        Task<DroitAcces?> GetByIdAsync(int id);
        Task<DroitAcces> CreateAsync(DroitAcces droitAcces);
        Task UpdateAsync(int id, DroitAcces droitAcces);
        Task DeleteAsync(int id);
    }
}