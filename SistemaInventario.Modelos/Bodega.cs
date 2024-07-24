using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.Modelos
{

    
    public class Bodega
    {
        [Key]
        public int Id { get; set; }

        [Required (ErrorMessage ="El nombre es un atributo requerido")]
        [MaxLength(60, ErrorMessage = "El nombre no puede tener mas de 50 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripcion es un atributo requerido")]
        [MaxLength(120, ErrorMessage = "la descripcion no puede tener mas de 120 caracteres")]
        public string Descripcion { get; set;}

        [Required(ErrorMessage = "El estado es un atributo requerido")]
        public bool Estado { get; set;}
    }
}
