using ControleAcces.Api.Models.Dto;

namespace ControleAcces.Api.Services
{
    // Ce service contient la logique métier centrale du système : décider si un accès
    // doit être autorisé ou refusé, à partir d'une tentative envoyée par le module embarqué.
    public interface IVerificationAccesService
    {
        Task<VerificationAccesResponse> VerifierAsync(VerificationAccesRequest requete);
    }
}
