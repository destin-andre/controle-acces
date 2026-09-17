using ControleAcces.Api.Models;

namespace ControleAcces.Api.Services
{
    public interface ILogAccesService
    {
        Task<List<LogAcces>> GetAllAsync();
        Task<LogAcces?> GetByIdAsync(int id);
        Task<LogAcces> CreateAsync(LogAcces logAcces);
        Task UpdateAsync(int id, LogAcces logAcces);
        Task DeleteAsync(int id);
    }
}