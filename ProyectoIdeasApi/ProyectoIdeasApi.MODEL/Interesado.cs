using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoIdeasApi.MODEL
{
    public class Interesado
    {
        public Guid Id { get; set; } 


        public DateTime FechaInteres { get; set; } = DateTime.UtcNow;
        public string Comentario { get; set; } = string.Empty;



        public Guid MiembroId { get; set; }
        public Miembro Miembro { get; set; }

        public Guid? IdeaConcretaId { get; set; }
        public IdeaConcreta? IdeaConcreta { get; set; }


    }
}
