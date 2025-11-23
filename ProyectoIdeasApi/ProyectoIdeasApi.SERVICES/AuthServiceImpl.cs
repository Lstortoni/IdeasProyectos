using Microsoft.Extensions.Options;
using ProyectoIdeasApi.CONTRACT.Auth;
using ProyectoIdeasApi.CONTRACT.JwtDto;
using ProyectoIdeasApi.INTERFACES.Infrastructure;
using ProyectoIdeasApi.INTERFACES.Jwt;
using ProyectoIdeasApi.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static ProyectoIdeasApi.CONTRACT.JwtDto.AuthDto;

namespace ProyectoIdeasApi.SERVICES
{
    public class AuthServiceImpl : IAuthService
    {
        private readonly IUsuarioRepository _usuarios;
        private readonly IJwtTokenService _jwt;
        private readonly IPasswordHasher _passwordHasher;
        private readonly JwtOptions _opt;

        public AuthServiceImpl(
            IUsuarioRepository usuarios,
            IJwtTokenService jwt,
            IPasswordHasher passwordHasher,
            IOptions<JwtOptions> opt)
        {
            _usuarios = usuarios;
            _jwt = jwt;
            _passwordHasher = passwordHasher;
            _opt = opt.Value;
        }

        // =====================================
        // GET CURRENT USER
        // =====================================
        public Task<CurrentUserDto?> GetCurrentAsync(ClaimsPrincipal principal)
        {
            if (principal?.Identity is null || !principal.Identity.IsAuthenticated)
                return Task.FromResult<CurrentUserDto?>(null);

            var id = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0";
            var email = principal.FindFirst(ClaimTypes.Email)?.Value ?? "";
            var name = principal.FindFirst(ClaimTypes.Name)?.Value ?? "";
            var roles = principal.FindAll(ClaimTypes.Role).Select(c => c.Value).ToArray();

            return Task.FromResult<CurrentUserDto?>(new(id, email, name, roles));
        }
        // =====================================
        // LOGIN
        // =====================================
        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto, CancellationToken ct = default)
        {
            var email = dto.Email.Trim().ToLowerInvariant();

            // Buscar
            var user = await _usuarios.GetByEmailAsync(email, ct);
            if (user is null)
                return null;

            // Validar password
            if (!_passwordHasher.Verify(dto.Password, user.PasswordHash))
                return null;

            var nombre = user.Miembro?.Nombre ?? "Usuario";

            // CurrentUser
            var current = new CurrentUserDto(
                user.Id,
                user.EmailLogin,
                nombre,
                new[] { "Usuario" }
            );

            var token = _jwt.GenerateToken(current);
            var expires = DateTime.UtcNow.AddMinutes(_opt.ExpMinutes);

            return new LoginResponseDto(token, expires, nombre);
        }

        // =====================================
        // REGISTER
        // =====================================
        public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto req, CancellationToken ct = default)
        {
            var email = req.Email.Trim().ToLowerInvariant();

            // 1) Validar duplicado
            if (await _usuarios.ExistsByEmailAsync(email, ct))
                throw new InvalidOperationException("El email ya está registrado.");

            // 2) Crear Usuario + Miembro
            var usuario = new Usuario
            {
                Id = Guid.NewGuid(),
                EmailLogin = email,
                PasswordHash = _passwordHasher.Hash(req.Password),
                EmailConfirmado = false,
                CreatedAt = DateTime.UtcNow
            };

            var miembro = new Miembro
            {
                Id = Guid.NewGuid(),
                Nombre = req.Nombre,
                Apellido = req.Apellido,
                Email = email,
                Telefono = req.Telefono,
                AutoDescripcion = req.AutoDescripcion
            };

            usuario = await _usuarios.CreateUsuarioConMiembroAsync(usuario, miembro, ct);

            // 3) Token
            var current = new CurrentUserDto(
                usuario.Id,
                usuario.EmailLogin,
                usuario.Miembro!.Nombre,
                new[] { "Usuario" }
            );

            var token = _jwt.GenerateToken(current);

            return new RegisterResponseDto
            {
                Id = usuario.Id,
                Email = usuario.EmailLogin,
                Nombre = usuario.Miembro.Nombre,
                Token = token,
                EmailConfirmado = usuario.EmailConfirmado
            };
        }
    }
}
