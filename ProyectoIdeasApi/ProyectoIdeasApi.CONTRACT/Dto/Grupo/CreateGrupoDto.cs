using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoIdeasApi.CONTRACT.Dto.Grupo
{
    public class CreateGrupoDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }

        public Guid IdeaConcretaId { get; set; }
    }
}
