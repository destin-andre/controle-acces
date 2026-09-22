using ControleAcces.Api.Models;
using ControleAcces.Api.Models.Dto;
using ControleAcces.Api.Repositories;

namespace ControleAcces.Api.Services
{
    public class VerificationAccesService : IVerificationAccesService
    {
        // Ce service a besoin de plusieurs repositories différents, puisque la vérification
        // fait intervenir plusieurs tables : Badge, DroitAcces, et enfin LogAcces pour
        // journaliser le résultat.
        private readonly IBadgeRepository _badgeRepository;
        private readonly IDroitAccesRepository _droitAccesRepository;
        private readonly ILogAccesRepository _logAccesRepository;

        public VerificationAccesService(
            IBadgeRepository badgeRepository,
            IDroitAccesRepository droitAccesRepository,
            ILogAccesRepository logAccesRepository)
        {
            _badgeRepository = badgeRepository;
            _droitAccesRepository = droitAccesRepository;
            _logAccesRepository = logAccesRepository;
        }

        public async Task<VerificationAccesResponse> VerifierAsync(VerificationAccesRequest requete)
        {
            var raisons = new List<string>();

            // Vérification 1 : le badge existe-t-il, et est-il actif ?
            var badge = await _badgeRepository.GetByIdAsync(requete.IdBadge);
            if (badge == null)
            {
                raisons.Add("Badge inconnu.");
            }
            else if (!badge.Actif)
            {
                raisons.Add("Badge désactivé.");
            }

            // Vérification 2 : l'empreinte a-t-elle été validée par le capteur ?
            if (!requete.EmpreinteValidee)
            {
                raisons.Add("Empreinte biométrique non reconnue.");
            }

            // Vérification 3 : un droit d'accès existe-t-il pour ce badge sur cette zone,
            // et sommes-nous dans sa période de validité ?
            // On ne fait cette vérification que si le badge existe réellement,
            // pour éviter une recherche inutile.
            if (badge != null)
            {
                var droitsAcces = await _droitAccesRepository.GetAllAsync();
                var maintenant = DateTime.UtcNow;

                var droitValide = droitsAcces.Any(droit =>
                    droit.IdBadge == requete.IdBadge &&
                    droit.IdZone == requete.IdZone &&
                    droit.DateDebut <= maintenant &&
                    droit.DateFin >= maintenant);

                if (!droitValide)
                {
                    raisons.Add("Aucun droit d'accès valide pour cette zone.");
                }
            }

            var accesAutorise = raisons.Count == 0;

            // Dans tous les cas, la tentative est journalisée, qu'elle soit autorisée ou non.
            var log = new LogAcces
            {
                Horodatage = DateTime.UtcNow,
                Methode = requete.EmpreinteValidee ? "badge+empreinte" : "badge",
                Resultat = accesAutorise ? "autorise" : "refuse",
                IdBadge = requete.IdBadge,
                IdZone = requete.IdZone,
                Raisons = raisons.Count > 0 ? string.Join("; ", raisons) : null
            };
            await _logAccesRepository.CreateAsync(log);

            return new VerificationAccesResponse
            {
                Autorise = accesAutorise,
                Raisons = raisons
            };
        }
    }
}
