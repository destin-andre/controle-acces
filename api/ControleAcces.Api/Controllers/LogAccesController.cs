using Microsoft.AspNetCore.Mvc;
using ControleAcces.Api.Models;
using ControleAcces.Api.Services;

namespace ControleAcces.Api.Controllers
{
    // [ApiController] active des comportements pratiques par défaut (validation automatique, etc.)
    // [Route] définit l'adresse de base de ce Controller : ici, /api/logacces
    [ApiController]
    [Route("api/[controller]")]
    public class LogAccesController : ControllerBase
    {
        private readonly ILogAccesService _service;

        // Le Controller ne connaît que le Service, jamais le Repository ni la base directement.
        public LogAccesController(ILogAccesService service)
        {
            _service = service;
        }

        // GET /api/logacces
        // Renvoie la liste de tous les logs d'accès.
        [HttpGet]
        public async Task<ActionResult<List<LogAcces>>> GetAll()
        {
            var logs = await _service.GetAllAsync();
            return Ok(logs);
        }

        // GET /api/logacces/5
        // Renvoie un log d'accès précis, ou une erreur 404 s'il n'existe pas.
        [HttpGet("{id}")]
        public async Task<ActionResult<LogAcces>> GetById(int id)
        {
            var log = await _service.GetByIdAsync(id);
            if (log == null)
            {
                return NotFound();
            }
            return Ok(log);
            
        }
                // POST /api/logacces
        // Crée un nouveau log d'accès à partir des données envoyées dans le corps de la requête.
        [HttpPost]
        public async Task<ActionResult<LogAcces>> Create(LogAcces logAcces)
        {
            try
            {
                var nouveauLog = await _service.CreateAsync(logAcces);
                return CreatedAtAction(nameof(GetById), new { id = nouveauLog.IdLog }, nouveauLog);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT /api/logacces/5
        // Met à jour un log d'accès existant.
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, LogAcces logAcces)
        {
            try
            {
                await _service.UpdateAsync(id, logAcces);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE /api/logacces/5
        // Supprime un log d'accès.
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}