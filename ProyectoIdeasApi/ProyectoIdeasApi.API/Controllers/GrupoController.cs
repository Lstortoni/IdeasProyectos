using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoIdeasApi.CONTRACT.Dto.Grupo;
using ProyectoIdeasApi.INTERFACES.Jwt;
using ProyectoIdeasApi.INTERFACES.Services;
using ProyectoIdeasApi.SERVICES;

namespace ProyectoIdeasApi.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]   // Todas las acciones requieren usuario autenticado
    public class GrupoController: BaseController
    {
        private readonly IGrupoService _grupoService;

       
        public GrupoController(IGrupoService grupoService, ILogService logService):base(logService)
        {
            _grupoService = grupoService;
        }

        // GET: api/grupos
        [HttpGet]
        public async Task<ActionResult<List<GrupoDto>>> GetAll(CancellationToken ct)
        {
            var grupos = await _grupoService.GetAllAsync(ct);
            return Ok(grupos);
        }

        // GET: api/grupos/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<GrupoDto>> GetById(Guid id, CancellationToken ct)
        {
            var grupo = await _grupoService.GetByIdAsync(id, ct);
            if (grupo is null)
                return NotFound();

            return Ok(grupo);
        }

        // GET: api/grupos/mios
        // Grupos donde participa el miembro logueado
        [HttpGet("mios")]
        public async Task<ActionResult<List<GrupoDto>>> GetMyGroups(CancellationToken ct)
        {
            var miembroId = GetMiembroIdFromClaims();
            if (miembroId == Guid.Empty)
                return Unauthorized();

            var grupos = await _grupoService.GetByMiembroAsync(miembroId, ct);
            return Ok(grupos);
        }

        // GET: api/grupos/idea/{ideaId}
        // Grupos creados a partir de una idea
        [HttpGet("idea/{ideaId:guid}")]
        public async Task<ActionResult<List<GrupoDto>>> GetByIdea(Guid ideaId, CancellationToken ct)
        {
            var grupos = await _grupoService.GetByIdeaAsync(ideaId, ct);
            return Ok(grupos);
        }

        // Helper: obtener miembroId desde el token
        private Guid GetMiembroIdFromClaims()
        {
            var claim = User.FindFirst("miembroId");
            if (claim is null) return Guid.Empty;

            return Guid.TryParse(claim.Value, out var id) ? id : Guid.Empty;
        }
    }
}
