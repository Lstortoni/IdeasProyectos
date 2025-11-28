
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProyectoIdeasApi.CONTRACT.JwtDto;
using ProyectoIdeasApi.INTERFACES.Jwt;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static ProyectoIdeasApi.CONTRACT.JwtDto.AuthDto;



namespace ProyectoIdeasApi.INFRASTRUCTURE.Jwt
{
    public sealed class JwtTokenService:IJwtTokenService
    {
        private readonly JwtOptions _opt;
        public JwtTokenService(IOptions<JwtOptions> opt) => _opt = opt.Value;

        public string GenerateToken(CurrentUserDto user)
        {
            var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.Name),
            new("miembroId", user.MiembroId.ToString())
        };
            claims.AddRange(user.Roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_opt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _opt.Issuer,
                audience: _opt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_opt.ExpMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
