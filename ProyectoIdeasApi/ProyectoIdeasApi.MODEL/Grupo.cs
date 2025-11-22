using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoIdeasApi.MODEL
{
    public class Grupo
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Nombre { get; set; } = string.Empty;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Ahora: una colección de links externos
        public List<LinkExterno> LinksExternos { get; set; } = new();

        // Relación con la IdeaConcreta
        public Guid IdeaConcretaId { get; set; }
        public IdeaConcreta IdeaConcreta { get; set; }

        // Colección de miembros del grupo
        public List<MiembroGrupo> Miembros { get; set; } = new();
    }
}
