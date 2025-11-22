using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoIdeasApi.MODEL
{
    public class Usuario
    {
        public Guid Id { get; set; }                      // PK = FK a Inscripto
        public string EmailLogin { get; set; } = "";      // puede = Inscripto.Email
        public string PasswordHash { get; set; } = "";
        public bool EmailConfirmado { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }

        public Guid? MiembroId { get; set; }
        public Miembro? Miembro{ get; set; }
    }
}

