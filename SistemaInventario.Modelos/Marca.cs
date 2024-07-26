using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.Modelos
{
    public class Marca
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Nombre Requerido")]
        [MaxLength(60, ErrorMessage = "La longitud maxima del nombre es 60 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Descripcion Requerido")]
        [MaxLength(200, ErrorMessage = "La longitud maxima de la descripcion es 200 caracteres")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Estado Requerido")]
        public bool Estado { get; set; }
    }
}
