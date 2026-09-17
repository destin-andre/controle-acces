using ControleAcces.Api.Models;

namespace ControleAcces.Api.Services

{
    // Cette interface définit les opérations métier disponibles pour les empreintes.
    // Contrairement au Repository, qui ne fait que lire/écrire en base sans réfléchir,
    // le Service peut appliquer des règles avant d'agir (validation, vérifications, etc.).
    public interface IEmpreinteService
    {
        Task<List<Empreinte>> GetAllAsync();
        Task<Empreinte?> GetByIdAsync(int id);
        Task<Empreinte> CreateAsync(Empreinte empreinte);
        Task UpdateAsync(int id, Empreinte empreinte);
        Task DeleteAsync(int id);
    }
}