using ControleAcces.Api.Models;
using ControleAcces.Api.Repositories;

namespace ControleAcces.Api.Services

{
    public class EmpreinteService : IEmpreinteService
    {
        private readonly IEmpreinteRepository _repository;

        public EmpreinteService(IEmpreinteRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Empreinte>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Empreinte?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Empreinte> CreateAsync(Empreinte empreinte)
        {
            if (empreinte.TemplatesBiometrique is not { Length: > 0 })
            {
                throw new ArgumentException(
                    "Les templates biométriques de l'empreinte sont obligatoires.");
            }

            return await _repository.CreateAsync(empreinte);
        }

        public async Task UpdateAsync(int id, Empreinte empreinte)
        {
            var existant = await _repository.GetByIdAsync(id);
            if (existant == null)
            {
                throw new KeyNotFoundException($"Aucune empreinte trouvée avec l'id {id}.");
            }

            existant.TemplatesBiometrique = empreinte.TemplatesBiometrique;
            existant.DateEnrolement = empreinte.DateEnrolement;

            await _repository.UpdateAsync(existant);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}