using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoIdeasApi.CONTRACT.Dto.Miembro
{
    public class CrearMiembroRequestDto
    {

        [Required, StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Apellido { get; set; } = string.Empty;

        [StringLength(20)]
        public string Telefono { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(200)]
        public string Email { get; set; } = string.Empty;

        [StringLength(500)]
        public string AutoDescripcion { get; set; } = string.Empty;

    }
}
