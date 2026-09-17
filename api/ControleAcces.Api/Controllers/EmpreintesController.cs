using Microsoft.AspNetCore.Mvc;
using ControleAcces.Api.Models;
using ControleAcces.Api.Services;

namespace ControleAcces.Api.Controllers

{
    // [ApiController] active des comportements pratiques par défaut (validation automatique, etc.)
    // [Route] définit l'adresse de base de ce Controller : ici, /api/empreintes
    [ApiController]
    [Route("api/[controller]")]
    public class EmpreintesController : ControllerBase
    {
        private readonly IEmpreinteService _service;

        // Le Controller ne connaît que le Service, jamais le Repository ni la base directement.
        public EmpreintesController(IEmpreinteService service)
        {
            _service = service;
        }

        // GET /api/empreintes
        // Renvoie la liste de toutes les empreintes.
        [HttpGet]
        public async Task<ActionResult<List<Empreinte>>> GetAll()
        {
            var empreintes = await _service.GetAllAsync();
            return Ok(empreintes);
        }

        // GET /api/empreintes/5
        // Renvoie une empreinte précise, ou une erreur 404 si elle n'existe pas.
        [HttpGet("{id}")]
        public async Task<ActionResult<Empreinte>> GetById(int id)
        {
            var empreinte = await _service.GetByIdAsync(id);

            if (empreinte == null)
            {
                return NotFound();
            }

            return Ok(empreinte);
        }

        // POST /api/empreintes
        // Crée une nouvelle empreinte à partir des données envoyées dans le corps de la requête.
        [HttpPost]
        public async Task<ActionResult<Empreinte>> Create(Empreinte empreinte)
        {
            try
            {
                var nouvelleEmpreinte = await _service.CreateAsync(empreinte);
                return CreatedAtAction(nameof(GetById), new { id = nouvelleEmpreinte.IdEmpreinte }, nouvelleEmpreinte);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT /api/empreintes/5
        // Met à jour une empreinte existante.
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Empreinte empreinte)
        {
            try
            {
                await _service.UpdateAsync(id, empreinte);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE /api/empreintes/5
        // Supprime une empreinte.
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}