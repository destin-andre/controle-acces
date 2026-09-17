using Microsoft.AspNetCore.Mvc;
using ControleAcces.Api.Models;
using ControleAcces.Api.Services;

namespace ControleAcces.Api.Controllers
{
    // [ApiController] active des comportements pratiques par défaut (validation automatique, etc.)
    // [Route] définit l'adresse de base de ce Controller : ici, /api/zones
    [ApiController]
    [Route("api/[controller]")]
    public class ZonesController : ControllerBase
    {
        private readonly IZoneService _service;

        // Le Controller ne connaît que le Service, jamais le Repository ni la base directement.
        public ZonesController(IZoneService service)
        {
            _service = service;
        }

        // GET /api/zones
        // Renvoie la liste de toutes les zones.
        [HttpGet]
        public async Task<ActionResult<List<Zone>>> GetAll()
        {
            var zones = await _service.GetAllAsync();
            return Ok(zones);
        }

        // GET /api/zones/5
        // Renvoie une zone précise, ou une erreur 404 si elle n'existe pas.
        [HttpGet("{id}")]
        public async Task<ActionResult<Zone>> GetById(int id)
        {
            var zone = await _service.GetByIdAsync(id);
            if (zone == null)
            {
                return NotFound();
            }
            return Ok(zone);
        }

        // POST /api/zones
        // Crée une nouvelle zone à partir des données envoyées dans le corps de la requête.
        [HttpPost]
        public async Task<ActionResult<Zone>> Create(Zone zone)
        {
            try
            {
                var nouvelleZone = await _service.CreateAsync(zone);
                // Renvoie un code 201 (Created), avec l'URL pour récupérer cette nouvelle zone.
                return CreatedAtAction(nameof(GetById), new { id = nouvelleZone.IdZone }, nouvelleZone);
            }
            catch (ArgumentException ex)
            {
                // Si le Service a rejeté la création (par exemple NomZone manquant),
                // on renvoie une erreur 400 (Bad Request) avec le message explicatif.
                return BadRequest(ex.Message);
            }
        }

        // PUT /api/zones/5
        // Met à jour une zone existante à partir des données envoyées dans le corps de la requête.
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Zone zone)
        {
            try
            {
                await _service.UpdateAsync(id, zone);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE /api/zones/5
        // Supprime une zone existante.
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}