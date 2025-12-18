using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoIdeasApi.CONTRACT.Dto.Ideas
{
    public class CrearIdeaRequestDto
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(300)]
        public string Proposito { get; set; } = string.Empty;

        // Opcional → NO Required
        [StringLength(20)]
        public string Color { get; set; } = string.Empty;

        // Obligatorio → SI Required
        [Required]
        public Guid RubroId { get; set; }
    }
}
