using ControleAcces.Api.Models;
using ControleAcces.Api.Repositories;

namespace ControleAcces.Api.Services
{
    public class DroitAccesService : IDroitAccesService
    {
        private readonly IDroitAccesRepository _repository;

        public DroitAccesService(IDroitAccesRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<DroitAcces>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<DroitAcces?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<DroitAcces> CreateAsync(DroitAcces droitAcces)
        {
            if (droitAcces.DateFin <= droitAcces.DateDebut)
            {
                throw new ArgumentException("La date de fin doit être postérieure à la date de début.");
            }

            return await _repository.CreateAsync(droitAcces);
        }

        public async Task UpdateAsync(int id, DroitAcces droitAcces)
        {
            var existant = await _repository.GetByIdAsync(id);
            if (existant == null)
            {
                throw new KeyNotFoundException($"Aucun droit d'accès trouvé avec l'id {id}.");
            }

            existant.DateDebut = droitAcces.DateDebut;
            existant.DateFin = droitAcces.DateFin;
            existant.IdBadge = droitAcces.IdBadge;
            existant.IdZone = droitAcces.IdZone;

            await _repository.UpdateAsync(existant);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
