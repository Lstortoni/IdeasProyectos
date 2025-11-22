using ProyectoIdeasApi.MODEL.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoIdeasApi.MODEL
{
    public class MiembroGrupo
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime FechaIngreso { get; set; } = DateTime.UtcNow;

        // Rol dentro del grupo
        public RolEnGrupo Rol { get; set; } = RolEnGrupo.Participante;

        // El miembro asociado
        public Guid MiembroId { get; set; }
        public Miembro Miembro { get; set; }

        // A qué grupo pertenece
        public Guid GrupoId { get; set; }
        public Grupo Grupo { get; set; }

        // Fecha de entrada al grupoA ver

        // valores posibles: "creador", "participante"
    }
}
