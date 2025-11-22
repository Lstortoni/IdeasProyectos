using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoIdeasApi.MODEL
{
    public class Miembro
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string AutoDescripcion { get; set; } = string.Empty;

        // Relación: un miembro tiene muchas habilidades
        public List<Habilidad> Habilidades { get; set; } = new();


        public Usuario? Usuario { get; set; }

        public List<MiembroIntimo> Intimos { get; set; } = new();

        // 
        public List<IdeaConcreta> IdeasCreadas { get; set; } = new();


        public List<MiembroGrupo> GruposDondeParticipa { get; set; } = new();
    }
}
