using Microsoft.EntityFrameworkCore;
using ControleAcces.Api.Models;

namespace ControleAcces.Api.Data
{
    // Cette classe représente toute la base de données côté C#.
    // Elle hérite de DbContext, la classe de base fournie par Entity Framework Core.
    public class ApplicationDbContext : DbContext
    {
        // Le constructeur reçoit les options de configuration (dont la chaîne de connexion)
        // définies dans Program.cs, et les transmet à la classe parente DbContext.
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Chaque DbSet représente une table de la base de données.
        // Ici, "Utilisateurs" correspond à une table dont chaque ligne est un objet Utilisateur.
        // On ajoutera une ligne similaire pour chaque nouvelle entité (Badges, Zones, etc.).
        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<Badge> Badges { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<Empreinte> Empreintes { get; set; }
        public DbSet<DroitAcces> DroitsAcces { get; set; }
        public DbSet<LogAcces> LogAcces { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Empreinte>()
                .HasOne(empreinte => empreinte.Utilisateur)
                .WithOne(utilisateur => utilisateur.Empreinte)
                .HasForeignKey<Empreinte>(empreinte => empreinte.IdUtilisateur);
        }
    }
}