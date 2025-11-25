using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoIdeasApi.CONTRACT.Dto.Ideas
{
    public class IdeaConcretaDto
    {
        public Guid Id { get; set; }

        public string Nombre { get; set; } = string.Empty;
        public string Proposito { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;

        public DateTime FechaCreacion { get; set; }

        public Guid CreadorId { get; set; }
        public Guid RubroId { get; set; }

        public int CantidadInteresados { get; set; }
        public bool TieneGrupo { get; set; }
    }
}
