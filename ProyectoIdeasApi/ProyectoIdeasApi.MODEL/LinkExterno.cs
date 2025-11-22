using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoIdeasApi.MODEL
{
    public class LinkExterno
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nombre { get; set; } = string.Empty; // Ej: "WhatsApp", "Discord"
        public string Url { get; set; } = string.Empty;    // Ej: https://chat.whatsapp...

        public Guid GrupoId { get; set; }
        public Grupo Grupo { get; set; }
    }
}
