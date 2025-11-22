using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoIdeasApi.CONTRACT.Auth
{
    public class RegisterResponseDto
    {
        public Guid Id { get; init; }                 // Id del Inscripto/AuthUser
        public string Nombre { get; init; } = "";
        public string Email { get; init; } = "";
        public bool EmailConfirmado { get; init; }    // por ahora false si no hay verificación
        public string Token { get; init; } = "";      // JWT emitido
    }
}
