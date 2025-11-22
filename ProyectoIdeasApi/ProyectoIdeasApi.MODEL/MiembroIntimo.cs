using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoIdeasApi.MODEL
{
    public class MiembroIntimo
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid MiembroId { get; set; }
        public Miembro Miembro { get; set; }

        public Guid IntimoId { get; set; }
        public Miembro Intimo { get; set; }

        public DateTime FechaAgregado { get; set; } = DateTime.UtcNow;

        public string? Nota { get; set; }
    }
}
