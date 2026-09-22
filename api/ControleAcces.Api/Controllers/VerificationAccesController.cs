using Microsoft.AspNetCore.Mvc;
using ControleAcces.Api.Models.Dto;
using ControleAcces.Api.Services;

namespace ControleAcces.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VerificationAccesController : ControllerBase
    {
        private readonly IVerificationAccesService _service;

        public VerificationAccesController(IVerificationAccesService service)
        {
            _service = service;
        }

        // POST /api/verificationacces
        // Vérifie si un badge a le droit d'accéder à une zone, journalise la tentative.
        [HttpPost]
        public async Task<ActionResult<VerificationAccesResponse>> VerifierAcces(VerificationAccesRequest requete)
        {
            var reponse = await _service.VerifierAsync(requete);
            return Ok(reponse);
        }
    }
}