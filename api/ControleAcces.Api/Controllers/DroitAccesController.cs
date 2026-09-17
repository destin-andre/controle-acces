using Microsoft.AspNetCore.Mvc;
using ControleAcces.Api.Models;
using ControleAcces.Api.Services;

namespace ControleAcces.Api.Controllers
{
    // [ApiController] active des comportements pratiques par défaut (validation automatique, etc.)
    // [Route] définit l'adresse de base de ce Controller : ici, /api/droitacces
    // (attention : "DroitAccesController" donne "/api/droitacces", sans "s" après "acces")
    [ApiController]
    [Route("api/[controller]")]
    public class DroitAccesController : ControllerBase
    {
        private readonly IDroitAccesService _service;

        public DroitAccesController(IDroitAccesService service)
        {
            _service = service;
        }

        // GET /api/droitacces
        [HttpGet]
        public async Task<ActionResult<List<DroitAcces>>> GetAll()
        {
            var droitsAcces = await _service.GetAllAsync();
            return Ok(droitsAcces);
        }

        // GET /api/droitacces/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DroitAcces>> GetById(int id)
        {
            var droitAcces = await _service.GetByIdAsync(id);
            if (droitAcces == null)
            {
                return NotFound();
            }
            return Ok(droitAcces);
        }

        // POST /api/droitacces
        [HttpPost]
        public async Task<ActionResult<DroitAcces>> Create(DroitAcces droitAcces)
        {
            try
            {
                var nouveauDroitAcces = await _service.CreateAsync(droitAcces);
                return CreatedAtAction(nameof(GetById), new { id = nouveauDroitAcces.IdDroit }, nouveauDroitAcces);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT /api/droitacces/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, DroitAcces droitAcces)
        {
            try
            {
                await _service.UpdateAsync(id, droitAcces);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE /api/droitacces/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}