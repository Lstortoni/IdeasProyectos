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

        public Guid PropietarioId { get; set; }
        public Miembro Propietario { get; set; } = null!;

        public Guid IntimoId { get; set; }
        public Miembro Intimo { get; set; } = null!;

        public DateTime FechaAgregado { get; set; } = DateTime.UtcNow;

        public string? Nota { get; set; }
    }
}
