using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoIdeasApi.CONTRACT.Dto.Rubro;
using ProyectoIdeasApi.INTERFACES.Services;
using ProyectoIdeasApi.SERVICES;

namespace ProyectoIdeasApi.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RubrosController: BaseController
    {
        private readonly IRubroService _rubroService;

        public RubrosController(IRubroService rubroService) { 
            _rubroService = rubroService;
        }

        // GET: api/rubros
        [HttpGet]
        public async Task<ActionResult<List<RubroDto>>> GetAll(CancellationToken ct)
        {
            var rubros = await _rubroService.GetAllAsync(ct);
            return Ok(rubros);
        }

        // GET: api/rubros/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<RubroDto>> GetById(Guid id, CancellationToken ct)
        {
            var rubro = await _rubroService.GetByIdAsync(id, ct);
            if (rubro is null)
                return NotFound();

            return Ok(rubro);
        }

        [HttpPost]
        public async Task<ActionResult<RubroDto>> AddRubro(
        [FromBody] CreateRubroDto dto,
        CancellationToken ct)
        {
            var result = await _rubroService.AddRubro(dto, ct);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: api/rubros/{id}
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<List<RubroDto>>> UpdateRubro(
            Guid id,
            [FromBody] UpdateRubroDto dto,
            CancellationToken ct)
        {
            dto.Id = id; // asegurar que el Id sea el de la ruta
            var result = await _rubroService.updateRubro(dto, ct);
            return Ok(result);
        }
    }
}
