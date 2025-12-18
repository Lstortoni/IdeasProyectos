using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoIdeasApi.MODEL
{
    public class IdeaConcreta
    {
        public Guid Id { get; set; }

        // Datos principales
        public string Nombre { get; set; } = string.Empty;

        // Descripción / propósito de la idea
        public string Proposito { get; set; } = string.Empty;

        // Color visual para identificar la idea (ej: "#FFAA00" o "green")
        public string? Color { get; set; } = null;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;


        public bool Activa { get; set; } = true;
        // --- Relaciones ---

        public bool EstaAbierta() { 
            return Activa && Grupo.Id == Guid.Empty;
        }

        // Quien creó la idea
        public Guid CreadorId { get; set; }
        public Miembro Creador { get; set; }= null!;

        // Lista de personas interesadas en esta idea
        public List<Interesado> Interesados { get; set; } = new();

        // Grupo asociado a la idea (puede no existir todavía)
        public Grupo Grupo { get; set; } = null!;

        public Guid RubroId { get; set; }
        public Rubro? Rubro { get; set; }
    }
}
