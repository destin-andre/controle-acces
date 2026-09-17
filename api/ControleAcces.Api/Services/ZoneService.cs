using ControleAcces.Api.Models;
using ControleAcces.Api.Repositories;

namespace ControleAcces.Api.Services
{
    public class ZoneService : IZoneService
    {
        private readonly IZoneRepository _repository;

        public ZoneService(IZoneRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Zone>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Zone?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Zone> CreateAsync(Zone zone)
        {
            if (string.IsNullOrWhiteSpace(zone.NomZone))
            {
                throw new ArgumentException("Le nom de la zone (NomZone) est obligatoire.");
            }

            return await _repository.CreateAsync(zone);
        }

        public async Task UpdateAsync(int id, Zone zone)
        {
            var existant = await _repository.GetByIdAsync(id);
            if (existant == null)
            {
                throw new KeyNotFoundException($"Aucune zone trouvée avec l'id {id}.");
            }

            existant.NomZone = zone.NomZone;
            existant.NiveauSecurite = zone.NiveauSecurite;

            await _repository.UpdateAsync(existant);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}