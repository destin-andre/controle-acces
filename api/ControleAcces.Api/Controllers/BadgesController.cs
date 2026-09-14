using Microsoft.AspNetCore.Mvc;
using ControleAcces.Api.Models;
using ControleAcces.Api.Services;

namespace ControleAcces.Api.Controllers
{
    // [ApiController] active des comportements pratiques par défaut (validation automatique, etc.)
    // [Route] définit l'adresse de base de ce Controller : ici, /api/badges
    [ApiController]
    [Route("api/[controller]")]
    public class BadgesController : ControllerBase
    {
        private readonly IBadgeService _service;

        // Le Controller ne connaît que le Service, jamais le Repository ni la base directement.
        public BadgesController(IBadgeService service)
        {
            _service = service;
        }

        // GET /api/badges
        // Renvoie la liste de tous les badges.
        [HttpGet]
        public async Task<ActionResult<List<Badge>>> GetAll()
        {
            var badges = await _service.GetAllAsync();
            return Ok(badges);
        }

        // GET /api/badges/5
        // Renvoie un badge précis, ou une erreur 404 s'il n'existe pas.
        [HttpGet("{id}")]
        public async Task<ActionResult<Badge>> GetById(int id)
        {
            var badge = await _service.GetByIdAsync(id);
            if (badge == null)
            {
                return NotFound();
            }
            return Ok(badge);
        }

        // POST /api/badges
        // Crée un nouveau badge à partir des données envoyées dans le corps de la requête.
        [HttpPost]
        public async Task<ActionResult<Badge>> Create(Badge badge)
        {
            try
            {
                var nouveauBadge = await _service.CreateAsync(badge);
                // Renvoie un code 201 (Created), avec l'URL pour récupérer ce nouveau badge.
                return CreatedAtAction(nameof(GetById), new { id = nouveauBadge.IdBadge }, nouveauBadge);
            }
            catch (ArgumentException ex)
            {
                // Si le Service a rejeté la création (par exemple UidBadge manquant),
                // on renvoie une erreur 400 (Bad Request) avec le message explicatif.
                return BadRequest(ex.Message);
            }
        }

        // PUT /api/badges/5
        // Met à jour un badge existant.
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Badge badge)
        {
            try
            {
                await _service.UpdateAsync(id, badge);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE /api/badges/5
        // Supprime un badge.
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}