using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.Modelos
{
    public class UsuarioAplicacion: IdentityUser //Tengo que descargar el paquete
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(80)]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "El Apellido es obligatorio")]
        [MaxLength(80)]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "la Direccion es obligatorio")]
        [MaxLength(200)]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "la Ciudad es obligatorio")]
        [MaxLength(60)]
        public string Ciudad { get; set; }

        [Required(ErrorMessage = "El Pais es obligatorio")]
        [MaxLength(60)]
        public string Pais { get; set; }

        [NotMapped]
        public string Role { get; set; }
    }
}
