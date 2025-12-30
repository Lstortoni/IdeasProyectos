using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoIdeasApi.CONTRACT.Dto.Miembro;
using ProyectoIdeasApi.INTERFACES.Services;
using ProyectoIdeasApi.SERVICES;

namespace ProyectoIdeasApi.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MiembrosController : BaseController
    {
        private readonly IMiembroService _miembroService;

        public MiembrosController(IMiembroService miembroService) { 
        
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
            var miembroId = GetMiembroId();
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
            [FromBody] UpdateMiembroDto dto,
            CancellationToken ct)
        {
            var miembroId = GetMiembroId();
            if (miembroId == Guid.Empty)
                return Unauthorized();

         
            await _miembroService.UpdateAsync(miembroId,dto, ct);

            return NoContent();
        }


        // POST api/miembros/me/intimos
        [HttpPost("me/intimos")]
        public async Task<IActionResult> MarcarComoIntimo(
            [FromBody] AddIntimoDto dto,
            CancellationToken ct)
        {
            var miembroId = GetMiembroId();
            if (miembroId == Guid.Empty)
                return Unauthorized();

            await _miembroService.MarcarComoIntimoAsync(miembroId, dto.IntimoId, dto.Nota, ct);
            return NoContent();
        }

        [HttpDelete("me/intimos/{intimoId:guid}")]
        public async Task<IActionResult> QuitarIntimo(Guid intimoId, CancellationToken ct)
        {
            var miembroId = GetMiembroId();
            if (miembroId == Guid.Empty)
                return Unauthorized();

            await _miembroService.QuitarIntimoAsync(miembroId, intimoId, ct);
            return NoContent();
        }



        [HttpGet("me/intimos")]
        public async Task<ActionResult<List<MiembroDto>>> ListarIntimos(CancellationToken ct)
        {
            var miembroId = GetMiembroId();
            if (miembroId == Guid.Empty)
                return Unauthorized();

            var intimos = await _miembroService.ListarIntimosAsync(miembroId, ct);
            return Ok(intimos);
        }
    }
}
