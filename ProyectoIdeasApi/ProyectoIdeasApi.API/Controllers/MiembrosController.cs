using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoIdeasApi.CONTRACT.Dto.Miembro;
using ProyectoIdeasApi.INTERFACES.Services;

namespace ProyectoIdeasApi.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MiembrosController : ControllerBase
    {
        private readonly IMiembroService _miembroService;

        public MiembrosController(IMiembroService miembroService)
        {
            _miembroService = miembroService;
        }

        // ---------------------------------------------------
        // GET: api/miembros/{id}
        // Obtener un miembro por Id (podés usarlo para vistas públicas/perfil)
        // ---------------------------------------------------
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<MiembroDto>> GetById(Guid id, CancellationToken ct)
        {
            var miembro = await _miembroService.GetByIdAsync(id, ct);

            if (miembro is null)
                return NotFound();

            return Ok(miembro);
        }

        // ---------------------------------------------------
        // GET: api/miembros
        // Listar todos los miembros (si después querés, podés limitar a admin)
        // ---------------------------------------------------
        [HttpGet]
        public async Task<ActionResult<List<MiembroDto>>> GetAll(CancellationToken ct)
        {
            var miembros = await _miembroService.GetAllAsync(ct);
            return Ok(miembros);
        }

        // ---------------------------------------------------
        // GET: api/miembros/me
        // Obtener MI propio perfil (usando miembroId del token)
        // ---------------------------------------------------
        [HttpGet("me")]
        public async Task<ActionResult<MiembroDto>> GetMe(CancellationToken ct)
        {
            var miembroId = GetMiembroIdFromClaims();
            if (miembroId == Guid.Empty)
                return Unauthorized();

            var miembro = await _miembroService.GetByIdAsync(miembroId, ct);

            if (miembro is null)
                return NotFound();

            return Ok(miembro);
        }

        // ---------------------------------------------------
        // PUT: api/miembros/me
        // Actualizar MI propio perfil
        // ---------------------------------------------------
        [HttpPut("me")]
        public async Task<ActionResult> UpdateMe(
            [FromBody] MiembroDto dto,
            CancellationToken ct)
        {
            var miembroId = GetMiembroIdFromClaims();
            if (miembroId == Guid.Empty)
                return Unauthorized();

            // Aseguramos que solo pueda editarse a sí mismo
            dto.Id = miembroId;

            await _miembroService.UpdateAsync(dto, ct);

            return NoContent();
        }

        // ---------------------------------------------------
        // Helper: obtener miembroId desde los claims del JWT
        // ---------------------------------------------------
        private Guid GetMiembroIdFromClaims()
        {
            var claim = User.FindFirst("miembroId");
            if (claim is null) return Guid.Empty;

            return Guid.TryParse(claim.Value, out var id) ? id : Guid.Empty;
        }
    }
}
