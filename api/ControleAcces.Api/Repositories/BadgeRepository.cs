using Microsoft.EntityFrameworkCore;
using ControleAcces.Api.Data;
using ControleAcces.Api.Models;

namespace ControleAcces.Api.Repositories

{
    // Implémentation concrète de IBadgeRepository, qui utilise Entity Framework Core
    // et notre ApplicationDbContext pour parler réellement à la base PostgreSQL.
    public class BadgeRepository : IBadgeRepository
    {
        private readonly ApplicationDbContext _context;

        // Le DbContext est fourni automatiquement par .NET grâce à l'enregistrement
        // effectué dans Program.cs (AddDbContext). C'est ce qu'on appelle l'injection de dépendances.
        public BadgeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Badge>> GetAllAsync()
        {
            // ToListAsync() exécute la requête SQL "SELECT * FROM Badges" en arrière-plan.
            return await _context.Badges.ToListAsync();
        }

        public async Task<Badge?> GetByIdAsync(int id)
        {
            // FindAsync cherche un badge par sa clé primaire (IdBadge).
            return await _context.Badges.FindAsync(id);
        }

        public async Task<Badge> CreateAsync(Badge badge)
        {
            // Ajoute le badge au suivi d'Entity Framework, puis SaveChangesAsync
            // traduit cet ajout en une vraie requête SQL INSERT.
            _context.Badges.Add(badge);
            await _context.SaveChangesAsync();
            return badge;
        }

        public async Task UpdateAsync(Badge badge)
        {
            _context.Badges.Update(badge);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var badge = await _context.Badges.FindAsync(id);
            if (badge != null)
            {
                _context.Badges.Remove(badge);
                await _context.SaveChangesAsync();
            }
        }
    }
}