using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoIdeasApi.CONTRACT.Auth;
using ProyectoIdeasApi.INTERFACES.Jwt;
using static ProyectoIdeasApi.CONTRACT.JwtDto.AuthDto;

namespace ProyectoIdeasApi.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST: api/auth/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponseDto>> Login(
            [FromBody] LoginRequestDto dto,
            CancellationToken ct)
        {
            var result = await _authService.LoginAsync(dto, ct);

            if (result is null)
                return Unauthorized(new { message = "Usuario o contraseña inválidos." });

            return Ok(result);
        }

        // POST: api/auth/register
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<RegisterResponseDto>> Register(
            [FromBody] RegisterRequestDto dto,
            CancellationToken ct)
        {
            try
            {
                var result = await _authService.RegisterAsync(dto, ct);
                // 201 Created (aunque no devolvemos Location por ahora)
                return StatusCode(StatusCodes.Status201Created, result);
            }
            catch (InvalidOperationException ex)
            {
                // Por ejemplo: email duplicado
                return Conflict(new { message = ex.Message });
            }
        }

        // GET: api/auth/me
        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<CurrentUserDto>> Me(CancellationToken ct)
        {
            var current = await _authService.GetCurrentAsync(User);

            if (current is null)
                return Unauthorized();

            return Ok(current);
        }
    }
}
