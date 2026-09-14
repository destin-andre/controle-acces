using ControleAcces.Api.Models;
using ControleAcces.Api.Repositories;

namespace ControleAcces.Api.Services
{
    public class BadgeService : IBadgeService
    {
        private readonly IBadgeRepository _repository;

        public BadgeService(IBadgeRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Badge>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Badge?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Badge> CreateAsync(Badge badge)
        {
            if (string.IsNullOrWhiteSpace(badge.UidBadge))
            {
                throw new ArgumentException("L'identifiant du badge (UidBadge) est obligatoire.");
            }

            return await _repository.CreateAsync(badge);
        }

        public async Task UpdateAsync(int id, Badge badge)
        {
            var existant = await _repository.GetByIdAsync(id);
            if (existant == null)
            {
                throw new KeyNotFoundException($"Aucun badge trouvé avec l'id {id}.");
            }

            existant.UidBadge = badge.UidBadge;
            existant.DateAttribution = badge.DateAttribution;
            existant.Actif = badge.Actif;
            existant.IdUtilisateur = badge.IdUtilisateur;

            await _repository.UpdateAsync(existant);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}