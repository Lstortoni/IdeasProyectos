using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoIdeasApi.CONTRACT.Dto.Miembro
{
    public class AddIntimoDto
    {
        [Required]
        public Guid IntimoId { get; set; }

        [StringLength(300)]
        public string? Nota { get; set; }
    }
}
