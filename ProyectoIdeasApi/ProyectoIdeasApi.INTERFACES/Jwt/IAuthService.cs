using ProyectoIdeasApi.CONTRACT.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static ProyectoIdeasApi.CONTRACT.JwtDto.AuthDto;

namespace ProyectoIdeasApi.INTERFACES.Jwt
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto, CancellationToken ct = default);
        Task<CurrentUserDto?> GetCurrentAsync(ClaimsPrincipal user);

        Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto req, CancellationToken ct = default);
    }
}
