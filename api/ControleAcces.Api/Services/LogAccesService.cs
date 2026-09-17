using ControleAcces.Api.Models;
using ControleAcces.Api.Repositories;

namespace ControleAcces.Api.Services
{
    public class LogAccesService : ILogAccesService
    {
        private readonly ILogAccesRepository _repository;

        public LogAccesService(ILogAccesRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<LogAcces>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<LogAcces?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<LogAcces> CreateAsync(LogAcces logAcces)
        {
            if (string.IsNullOrWhiteSpace(logAcces.Resultat))
            {
                throw new ArgumentException("Le résultat du log d'accès est obligatoire.");
            }

            return await _repository.CreateAsync(logAcces);
        }

        public async Task UpdateAsync(int id, LogAcces logAcces)
        {
            var existant = await _repository.GetByIdAsync(id);
            if (existant == null)
            {
                throw new KeyNotFoundException($"Aucun log trouvé avec l'id {id}.");
            }

            existant.Horodatage = logAcces.Horodatage;
            existant.Methode = logAcces.Methode;
            existant.Resultat = logAcces.Resultat;
            existant.IdBadge = logAcces.IdBadge;
            existant.IdZone = logAcces.IdZone;

            await _repository.UpdateAsync(existant);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}