using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoIdeasApi.CONTRACT.Dto.Ideas
{
    public class CrearIdeaRequestDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Proposito { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;   // "#FFAA00" o nombre
        public Guid RubroId { get; set; }                  // opcional
    }
}
