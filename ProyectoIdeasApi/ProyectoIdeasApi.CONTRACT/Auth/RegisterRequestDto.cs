using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoIdeasApi.CONTRACT.Auth
{
    public class RegisterRequestDto
    {
        [Required, StringLength(200)]
        public string Nombre { get; set; } = "";

        [Required, EmailAddress, StringLength(200)]
        public string Email { get; set; } = "";

        [StringLength(50)]
        public string? Telefono { get; set; }


        // Credenciales
        [Required, StringLength(128, MinimumLength = 8)]
        public string Password { get; set; } = "";

        [Required, Compare(nameof(Password), ErrorMessage = "Las contraseñas no coinciden.")]
        public string PasswordConfirm { get; set; } = "";
    }
}
