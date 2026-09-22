using Microsoft.EntityFrameworkCore;
using ControleAcces.Api.Data;
using ControleAcces.Api.Repositories;
using ControleAcces.Api.Services;

// Point d'entrée de l'application : c'est ici que tout démarre.
// "builder" permet de configurer les services dont l'API a besoin avant de la lancer.
var builder = WebApplication.CreateBuilder(args);

// Active le système de Controllers (nécessaire pour utiliser des classes comme UtilisateursController).
builder.Services.AddControllers();

// Enregistre notre ApplicationDbContext auprès de l'application.
// Chaque fois qu'un Repository aura besoin de parler à la base de données,
// .NET lui fournira automatiquement une instance déjà connectée.
// "UseNpgsql" indique qu'on utilise PostgreSQL, avec l'adresse définie dans appsettings.json.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Enregistre le Repository des utilisateurs : chaque fois qu'une classe demande
// un IUtilisateurRepository dans son constructeur, .NET lui fournira une instance
// de UtilisateurRepository, valable pendant toute la durée d'une requête HTTP.
builder.Services.AddScoped<IUtilisateurRepository, UtilisateurRepository>();

// Enregistre le Service des utilisateurs, qui porte la logique métier
// (règles de validation, vérifications) et s'appuie lui-même sur le Repository.
builder.Services.AddScoped<IUtilisateurService, UtilisateurService>();
builder.Services.AddScoped<IBadgeRepository, BadgeRepository>();
builder.Services.AddScoped<IBadgeService, BadgeService>();
builder.Services.AddScoped<IZoneRepository, ZoneRepository>();
builder.Services.AddScoped<IZoneService, ZoneService>();
builder.Services.AddScoped<IEmpreinteRepository, EmpreinteRepository>();
builder.Services.AddScoped<IEmpreinteService, EmpreinteService>();
builder.Services.AddScoped<IDroitAccesRepository, DroitAccesRepository>();
builder.Services.AddScoped<IDroitAccesService, DroitAccesService>();
builder.Services.AddScoped<ILogAccesRepository, LogAccesRepository>();
builder.Services.AddScoped<ILogAccesService, LogAccesService>();
builder.Services.AddScoped<IVerificationAccesService, VerificationAccesService>();
// Active la génération automatique de la documentation technique de l'API (format OpenAPI).
builder.Services.AddOpenApi();

// Construit l'application avec tous les services configurés ci-dessus.
var app = builder.Build();

// En environnement de développement uniquement, expose la documentation OpenAPI.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Redirige automatiquement les requêtes HTTP non sécurisées vers HTTPS.
app.UseHttpsRedirection();

// Active la vérification des autorisations (utile plus tard pour restreindre certains endpoints).
app.UseAuthorization();

// Relie les routes définies dans les Controllers (par exemple GET /utilisateurs) à l'application.
app.MapControllers();

// Démarre le serveur et le laisse tourner en attente de requêtes.
app.Run();