using Microsoft.AspNetCore.Mvc;
using ControleAcces.Api.Models;
using ControleAcces.Api.Services;

namespace ControleAcces.Api.Controllers
{
    // [ApiController] active des comportements pratiques par défaut (validation automatique, etc.)
    // [Route] définit l'adresse de base de ce Controller : ici, /api/utilisateurs
    [ApiController]
    [Route("api/[controller]")]
    public class UtilisateursController : ControllerBase
    {
        private readonly IUtilisateurService _service;

        // Le Controller ne connaît que le Service, jamais le Repository ni la base directement.
        public UtilisateursController(IUtilisateurService service)
        {
            _service = service;
        }

        // GET /api/utilisateurs
        // Renvoie la liste de tous les utilisateurs.
        [HttpGet]
        public async Task<ActionResult<List<Utilisateur>>> GetAll()
        {
            var utilisateurs = await _service.GetAllAsync();
            return Ok(utilisateurs);
        }

        // GET /api/utilisateurs/5
        // Renvoie un utilisateur précis, ou une erreur 404 s'il n'existe pas.
        [HttpGet("{id}")]
        public async Task<ActionResult<Utilisateur>> GetById(int id)
        {
            var utilisateur = await _service.GetByIdAsync(id);
            if (utilisateur == null)
            {
                return NotFound();
            }
            return Ok(utilisateur);
        }

        // POST /api/utilisateurs
        // Crée un nouvel utilisateur à partir des données envoyées dans le corps de la requête.
        [HttpPost]
        public async Task<ActionResult<Utilisateur>> Create(Utilisateur utilisateur)
        {
            try
            {
                var nouvelUtilisateur = await _service.CreateAsync(utilisateur);
                // Renvoie un code 201 (Created), avec l'URL pour récupérer ce nouvel utilisateur.
                return CreatedAtAction(nameof(GetById), new { id = nouvelUtilisateur.IdUtilisateur }, nouvelUtilisateur);
            }
            catch (ArgumentException ex)
            {
                // Si le Service a rejeté la création (par exemple email manquant),
                // on renvoie une erreur 400 (Bad Request) avec le message explicatif.
                return BadRequest(ex.Message);
            }
        }

        // PUT /api/utilisateurs/5
        // Met à jour un utilisateur existant.
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Utilisateur utilisateur)
        {
            try
            {
                await _service.UpdateAsync(id, utilisateur);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE /api/utilisateurs/5
        // Supprime un utilisateur.
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
