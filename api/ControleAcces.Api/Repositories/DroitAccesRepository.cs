using Microsoft.EntityFrameworkCore;
using ControleAcces.Api.Data;
using ControleAcces.Api.Models;

namespace ControleAcces.Api.Repositories
{
    // Implémentation concrète de IDroitAccesRepository, qui utilise Entity Framework Core
    // et notre ApplicationDbContext pour parler réellement à la base PostgreSQL.
    public class DroitAccesRepository : IDroitAccesRepository
    {
        private readonly ApplicationDbContext _context;

        // Le DbContext est fourni automatiquement par .NET grâce à l'enregistrement
        // effectué dans Program.cs (AddDbContext). C'est ce qu'on appelle l'injection de dépendances.
        public DroitAccesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<DroitAcces>> GetAllAsync()
        {
            return await _context.DroitsAcces.ToListAsync();
        }

        public async Task<DroitAcces?> GetByIdAsync(int id)
        {
            return await _context.DroitsAcces.FindAsync(id);
        }

        public async Task<DroitAcces> CreateAsync(DroitAcces droitAcces)
        {
            _context.DroitsAcces.Add(droitAcces);
            await _context.SaveChangesAsync();
            return droitAcces;
        }

        public async Task UpdateAsync(DroitAcces droitAcces)
        {
            _context.DroitsAcces.Update(droitAcces);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var droitAcces = await _context.DroitsAcces.FindAsync(id);
            if (droitAcces != null)
            {
                _context.DroitsAcces.Remove(droitAcces);
                await _context.SaveChangesAsync();
            }
        }
    }
}