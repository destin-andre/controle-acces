using ControleAcces.Api.Models;
using ControleAcces.Api.Repositories;

namespace ControleAcces.Api.Services
{
    // Implémentation concrète de IUtilisateurService.
    // Le Service ne parle jamais directement à la base de données : il passe toujours
    // par le Repository, qu'il reçoit via son constructeur (injection de dépendances).
    public class UtilisateurService : IUtilisateurService
    {
        private readonly IUtilisateurRepository _repository;

        public UtilisateurService(IUtilisateurRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Utilisateur>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Utilisateur?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Utilisateur> CreateAsync(Utilisateur utilisateur)
        {
            // Exemple de règle métier : on vérifie que l'email n'est pas vide
            // avant même d'essayer de l'enregistrer en base.
            if (string.IsNullOrWhiteSpace(utilisateur.Email))
            {
                throw new ArgumentException("L'email de l'utilisateur est obligatoire.");
            }

            return await _repository.CreateAsync(utilisateur);
        }

        public async Task UpdateAsync(int id, Utilisateur utilisateur)
        {
            var existant = await _repository.GetByIdAsync(id);
            if (existant == null)
            {
                throw new KeyNotFoundException($"Aucun utilisateur trouvé avec l'id {id}.");
            }

            // On met à jour uniquement les champs modifiables, en gardant le même identifiant.
            existant.Nom = utilisateur.Nom;
            existant.Prenom = utilisateur.Prenom;
            existant.Email = utilisateur.Email;
            existant.Poste = utilisateur.Poste;
            existant.EnActivite = utilisateur.EnActivite;

            await _repository.UpdateAsync(existant);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
