using ControleAcces.Api.Models;
using ControleAcces.Api.Models.Dto;
using ControleAcces.Api.Repositories;
using ControleAcces.Api.Services;
using Moq;
using Xunit;

namespace ControleAcces.Api.Tests
{
    // Chaque test suit le même schéma en trois temps, une convention très répandue
    // appelée "Arrange, Act, Assert" :
    // - Arrange : on prépare les données et les mocks (les fausses versions des Repositories)
    // - Act : on appelle la méthode qu'on veut tester
    // - Assert : on vérifie que le résultat correspond à ce qu'on attendait
    public class VerificationAccesServiceTests
    {
        private static (Mock<IBadgeRepository>, Mock<IDroitAccesRepository>, Mock<ILogAccesRepository>) CreerMocks()
        {
            return (new Mock<IBadgeRepository>(), new Mock<IDroitAccesRepository>(), new Mock<ILogAccesRepository>());
        }

        [Fact]
        public async Task VerifierAsync_BadgeInconnu_RefuseAvecLaBonneRaison()
        {
            var (badgeRepo, droitRepo, logRepo) = CreerMocks();
            badgeRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Badge?)null);

            var service = new VerificationAccesService(badgeRepo.Object, droitRepo.Object, logRepo.Object);
            var requete = new VerificationAccesRequest { IdBadge = 99, IdZone = 1, EmpreinteValidee = true };

            var resultat = await service.VerifierAsync(requete);

            Assert.False(resultat.Autorise);
            Assert.Contains("Badge inconnu.", resultat.Raisons);
        }

        [Fact]
        public async Task VerifierAsync_BadgeInactif_RefuseAvecLaBonneRaison()
        {
            var (badgeRepo, droitRepo, logRepo) = CreerMocks();
            var badgeInactif = new Badge { IdBadge = 1, UidBadge = "B-1", Actif = false, IdUtilisateur = 1 };
            badgeRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(badgeInactif);
            droitRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<DroitAcces>());

            var service = new VerificationAccesService(badgeRepo.Object, droitRepo.Object, logRepo.Object);
            var requete = new VerificationAccesRequest { IdBadge = 1, IdZone = 1, EmpreinteValidee = true };

            var resultat = await service.VerifierAsync(requete);

            Assert.False(resultat.Autorise);
            Assert.Contains("Badge désactivé.", resultat.Raisons);
        }

        [Fact]
        public async Task VerifierAsync_EmpreinteNonValidee_RefuseAvecLaBonneRaison()
        {
            var (badgeRepo, droitRepo, logRepo) = CreerMocks();
            var badgeActif = new Badge { IdBadge = 1, UidBadge = "B-1", Actif = true, IdUtilisateur = 1 };
            badgeRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(badgeActif);
            droitRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<DroitAcces>());

            var service = new VerificationAccesService(badgeRepo.Object, droitRepo.Object, logRepo.Object);
            var requete = new VerificationAccesRequest { IdBadge = 1, IdZone = 1, EmpreinteValidee = false };

            var resultat = await service.VerifierAsync(requete);

            Assert.False(resultat.Autorise);
            Assert.Contains("Empreinte biométrique non reconnue.", resultat.Raisons);
        }

        [Fact]
        public async Task VerifierAsync_AucunDroitAcces_RefuseAvecLaBonneRaison()
        {
            var (badgeRepo, droitRepo, logRepo) = CreerMocks();
            var badgeActif = new Badge { IdBadge = 1, UidBadge = "B-1", Actif = true, IdUtilisateur = 1 };
            badgeRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(badgeActif);
            droitRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<DroitAcces>());

            var service = new VerificationAccesService(badgeRepo.Object, droitRepo.Object, logRepo.Object);
            var requete = new VerificationAccesRequest { IdBadge = 1, IdZone = 1, EmpreinteValidee = true };

            var resultat = await service.VerifierAsync(requete);

            Assert.False(resultat.Autorise);
            Assert.Contains("Aucun droit d'accès valide pour cette zone.", resultat.Raisons);
        }

        [Fact]
        public async Task VerifierAsync_DroitAccesExpire_RefuseAvecLaBonneRaison()
        {
            var (badgeRepo, droitRepo, logRepo) = CreerMocks();
            var badgeActif = new Badge { IdBadge = 1, UidBadge = "B-1", Actif = true, IdUtilisateur = 1 };
            badgeRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(badgeActif);

            var droitExpire = new DroitAcces
            {
                IdDroit = 1,
                IdBadge = 1,
                IdZone = 1,
                DateDebut = DateTime.UtcNow.AddYears(-1),
                DateFin = DateTime.UtcNow.AddDays(-1)
            };
            droitRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<DroitAcces> { droitExpire });

            var service = new VerificationAccesService(badgeRepo.Object, droitRepo.Object, logRepo.Object);
            var requete = new VerificationAccesRequest { IdBadge = 1, IdZone = 1, EmpreinteValidee = true };

            var resultat = await service.VerifierAsync(requete);

            Assert.False(resultat.Autorise);
            Assert.Contains("Aucun droit d'accès valide pour cette zone.", resultat.Raisons);
        }

        [Fact]
        public async Task VerifierAsync_ToutesLesConditionsRemplies_Autorise()
        {
            var (badgeRepo, droitRepo, logRepo) = CreerMocks();
            var badgeActif = new Badge { IdBadge = 1, UidBadge = "B-1", Actif = true, IdUtilisateur = 1 };
            badgeRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(badgeActif);

            var droitValide = new DroitAcces
            {
                IdDroit = 1,
                IdBadge = 1,
                IdZone = 1,
                DateDebut = DateTime.UtcNow.AddDays(-1),
                DateFin = DateTime.UtcNow.AddDays(1)
            };
            droitRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<DroitAcces> { droitValide });

            var service = new VerificationAccesService(badgeRepo.Object, droitRepo.Object, logRepo.Object);
            var requete = new VerificationAccesRequest { IdBadge = 1, IdZone = 1, EmpreinteValidee = true };

            var resultat = await service.VerifierAsync(requete);

            Assert.True(resultat.Autorise);
            Assert.Empty(resultat.Raisons);

            logRepo.Verify(r => r.CreateAsync(It.IsAny<LogAcces>()), Times.Once);
        }
    }
}
