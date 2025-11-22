using ProyectoIdeasApi.CONTRACT.Auth;
using ProyectoIdeasApi.CONTRACT.JwtDto;
using ProyectoIdeasApi.INTERFACES.Jwt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoIdeasApi.SERVICES
{
    public class AuthService : IAuthService
    {


        Task<AuthDto.CurrentUserDto?> IAuthService.GetCurrentAsync(ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }

        Task<AuthDto.LoginResponseDto?> IAuthService.LoginAsync(AuthDto.LoginRequestDto dto, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        Task<RegisterResponseDto> IAuthService.RegisterAsync(RegisterRequestDto req, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
