using Microsoft.EntityFrameworkCore;
using ControleAcces.Api.Data;
using ControleAcces.Api.Models;

namespace ControleAcces.Api.Repositories
{
    // Implémentation concrète de IZoneRepository, qui utilise Entity Framework Core
    // et notre ApplicationDbContext pour parler réellement à la base PostgreSQL.
    public class ZoneRepository : IZoneRepository
    {
        private readonly ApplicationDbContext _context;

        // Le DbContext est fourni automatiquement par .NET grâce à l'enregistrement
        // effectué dans Program.cs (AddDbContext). C'est ce qu'on appelle l'injection de dépendances.
        public ZoneRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Zone>> GetAllAsync()
        {
            // ToListAsync() exécute la requête SQL "SELECT * FROM Zones" en arrière-plan.
            return await _context.Zones.ToListAsync();
        }

        public async Task<Zone?> GetByIdAsync(int id)
        {
            // FindAsync cherche une zone par sa clé primaire (IdZone).
            return await _context.Zones.FindAsync(id);
        }

        public async Task<Zone> CreateAsync(Zone zone)
        {
            // Ajoute la zone au suivi d'Entity Framework, puis SaveChangesAsync
            // traduit cet ajout en une vraie requête SQL INSERT.
            _context.Zones.Add(zone);
            await _context.SaveChangesAsync();
            return zone;
        }

        public async Task UpdateAsync(Zone zone)
        {
            _context.Zones.Update(zone);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var zone = await _context.Zones.FindAsync(id);
            if (zone != null)
            {
                _context.Zones.Remove(zone);
                await _context.SaveChangesAsync();
            }
        }
    }
}