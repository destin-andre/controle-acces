using Microsoft.EntityFrameworkCore;
using ControleAcces.Api.Data;
using ControleAcces.Api.Models;

namespace ControleAcces.Api.Repositories

{
    // Implémentation concrète de IEmpreinteRepository, qui utilise Entity Framework Core
    // et notre ApplicationDbContext pour parler réellement à la base PostgreSQL.
    public class EmpreinteRepository : IEmpreinteRepository
    {
        private readonly ApplicationDbContext _context;

        // Le DbContext est fourni automatiquement par .NET grâce à l'enregistrement
        // effectué dans Program.cs (AddDbContext). C'est ce qu'on appelle l'injection de dépendances.
        public EmpreinteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Empreinte>> GetAllAsync()
        {
            return await _context.Empreintes
                .Include(empreinte => empreinte.Utilisateur)
                .ToListAsync();
        }

        public async Task<Empreinte?> GetByIdAsync(int id)
        {
            return await _context.Empreintes
                .Include(empreinte => empreinte.Utilisateur)
                .FirstOrDefaultAsync(empreinte => empreinte.IdEmpreinte == id);
        }

        public async Task<Empreinte> CreateAsync(Empreinte empreinte)
        {
            _context.Empreintes.Add(empreinte);
            await _context.SaveChangesAsync();
            return await GetByIdAsync(empreinte.IdEmpreinte) ?? empreinte;
        }

        public async Task UpdateAsync(Empreinte empreinte)
        {
            _context.Empreintes.Update(empreinte);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var empreinte = await _context.Empreintes.FindAsync(id);
            if (empreinte != null)
            {
                _context.Empreintes.Remove(empreinte);
                await _context.SaveChangesAsync();
            }
        }
    }
}