using Microsoft.EntityFrameworkCore;
using ControleAcces.Api.Data;
using ControleAcces.Api.Models;

namespace ControleAcces.Api.Repositories
{
    // Implémentation concrète de ILogAccesRepository, qui utilise Entity Framework Core
    // et notre ApplicationDbContext pour parler réellement à la base PostgreSQL.
    public class LogAccesRepository : ILogAccesRepository
    {
        private readonly ApplicationDbContext _context;

        // Le DbContext est fourni automatiquement par .NET grâce à l'enregistrement
        // effectué dans Program.cs (AddDbContext). C'est ce qu'on appelle l'injection de dépendances.
        public LogAccesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<LogAcces>> GetAllAsync()
        {
            return await _context.LogAcces
                .Include(log => log.Badge)
                .Include(log => log.Zone)
                .ToListAsync();
        }

        public async Task<LogAcces?> GetByIdAsync(int id)
        {
            return await _context.LogAcces
                .Include(log => log.Badge)
                .Include(log => log.Zone)
                .FirstOrDefaultAsync(log => log.IdLog == id);
        }

        public async Task<LogAcces> CreateAsync(LogAcces logAcces)
        {
            _context.LogAcces.Add(logAcces);
            await _context.SaveChangesAsync();
            return await GetByIdAsync(logAcces.IdLog) ?? logAcces;
        }

        public async Task UpdateAsync(LogAcces logAcces)
        {
            _context.LogAcces.Update(logAcces);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var logAcces = await _context.LogAcces.FindAsync(id);
            if (logAcces != null)
            {
                _context.LogAcces.Remove(logAcces);
                await _context.SaveChangesAsync();
            }
        }
    }
}