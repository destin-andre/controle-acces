using Microsoft.EntityFrameworkCore;
using ControleAcces.Api.Data;
using ControleAcces.Api.Models;

namespace ControleAcces.Api.Repositories
{
    // Implémentation concrète de IUtilisateurRepository, qui utilise Entity Framework Core
    // et notre ApplicationDbContext pour parler réellement à la base PostgreSQL.
    public class UtilisateurRepository : IUtilisateurRepository
    {
        private readonly ApplicationDbContext _context;

        // Le DbContext est fourni automatiquement par .NET grâce à l'enregistrement
        // effectué dans Program.cs (AddDbContext). C'est ce qu'on appelle l'injection de dépendances.
        public UtilisateurRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Utilisateur>> GetAllAsync()
        {
            // ToListAsync() exécute la requête SQL "SELECT * FROM Utilisateurs" en arrière-plan.
            return await _context.Utilisateurs.ToListAsync();
        }

        public async Task<Utilisateur?> GetByIdAsync(int id)
        {
            // FindAsync cherche un utilisateur par sa clé primaire (IdUtilisateur).
            return await _context.Utilisateurs.FindAsync(id);
        }

        public async Task<Utilisateur> CreateAsync(Utilisateur utilisateur)
        {
            // Ajoute l'utilisateur au suivi d'Entity Framework, puis SaveChangesAsync
            // traduit cet ajout en une vraie requête SQL INSERT.
            _context.Utilisateurs.Add(utilisateur);
            await _context.SaveChangesAsync();
            return utilisateur;
        }

        public async Task UpdateAsync(Utilisateur utilisateur)
        {
            _context.Utilisateurs.Update(utilisateur);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var utilisateur = await _context.Utilisateurs.FindAsync(id);
            if (utilisateur != null)
            {
                _context.Utilisateurs.Remove(utilisateur);
                await _context.SaveChangesAsync();
            }
        }
    }
}