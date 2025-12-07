using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoIdeasApi.CONTRACT.Dto.Ideas;
using ProyectoIdeasApi.INTERFACES.Services;
using ProyectoIdeasApi.SERVICES;

namespace ProyectoIdeasApi.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]   // Todas las acciones requieren usuario autenticado
    public class IdeasController : BaseController
    {
        private readonly IIdeaCreadaService _ideasService;

        public IdeasController(IIdeaCreadaService ideasService)
        {
            _ideasService = ideasService;
        }

        // ---------------------------------------------------
        // POST api/ideas
        // Crear una idea
        // ---------------------------------------------------
        [HttpPost]
        public async Task<ActionResult<IdeaConcretaDto>> CrearIdea(
            [FromBody] CrearIdeaRequestDto dto,
            CancellationToken ct)
        {
            var miembroId = GetMiembroId();
            if (miembroId == Guid.Empty)
                return Unauthorized();

            var idea = await _ideasService.CrearIdeaAsync(miembroId, dto);

            return CreatedAtAction(nameof(GetById), new { id = idea.Id }, idea);
        }

        // ---------------------------------------------------
        // GET api/ideas/abiertas
        // Listar ideas abiertas
        // ---------------------------------------------------
        [HttpGet("abiertas")]
        [AllowAnonymous]  // Si querés que todos puedan leer ideas
        public async Task<ActionResult<List<IdeaConcretaDto>>> ObtenerAbiertas(CancellationToken ct)
        {
            var result = await _ideasService.ListarIdeasAbiertasAsync();
            return Ok(result);
        }

        // ---------------------------------------------------
        // GET api/ideas/mias
        // Listar ideas creadas por el usuario logueado
        // ---------------------------------------------------
        [HttpGet("mias")]
        public async Task<ActionResult<List<IdeaConcretaDto>>> MisIdeas(CancellationToken ct)
        {
            var miembroId = GetMiembroId();
            if (miembroId == Guid.Empty)
                return Unauthorized();

            var result = await _ideasService.ListarIdeasCreadasPorAsync(miembroId);
            return Ok(result);
        }

        // ---------------------------------------------------
        // GET api/ideas/{id}
        // Obtener detalle de una idea
        // ---------------------------------------------------
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<IdeaConcretaDto>> GetById(Guid id, CancellationToken ct)
        {
            var lista = await _ideasService.ListarIdeasAbiertasAsync();
            // Ajustaremos cuando tengas servicio GetById, por ahora lo dejamos placeholder.
            var idea = lista.FirstOrDefault(i => i.Id == id);

            if (idea is null)
                return NotFound();

            return Ok(idea);
        }

        // ---------------------------------------------------
        // POST api/ideas/{id}/interesarse
        // Usuario logueado se interesa en una idea
        // ---------------------------------------------------
        [HttpPost("{id:guid}/interesarse")]
        public async Task<ActionResult> Interesarse(Guid id, CancellationToken ct)
        {
            var miembroId = GetMiembroId();
            if (miembroId == Guid.Empty)
                return Unauthorized();

            await _ideasService.InteresarseAsync(miembroId, id);
            return Ok();
        }

        // ---------------------------------------------------
        // POST api/ideas/{id}/crear-grupo
        // Crea un grupo con los miembros seleccionados
        // ---------------------------------------------------
        [HttpPost("{id:guid}/crear-grupo")]
        public async Task<ActionResult> CrearGrupo(
            Guid id,
            [FromBody] List<Guid> miembrosSeleccionados,
            CancellationToken ct)
        {
            var grupo = await _ideasService.CrearGrupoDesdeIdeaAsync(id, miembrosSeleccionados);
            return Ok(grupo);
        }

      
    }
}
