using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProyectoIdeasApi.CONTRACT.JwtDto.AuthDto;


namespace ProyectoIdeasApi.INTERFACES.Jwt
{
    public interface IJwtTokenService
    {
        string GenerateToken(CurrentUserDto user);
    }
}
