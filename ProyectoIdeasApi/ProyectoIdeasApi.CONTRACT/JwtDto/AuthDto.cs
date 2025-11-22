using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoIdeasApi.CONTRACT.JwtDto
{
    public class AuthDto
    {
        public sealed record LoginRequestDto(string Email, string Password);
        public sealed record LoginResponseDto(string Token,
            DateTime ExpiresAtUtc,
            string user);

        public sealed record CurrentUserDto(int Id, string Email, string Name, string[] Roles);
    }
}
