using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.Modelos
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Nombre Requerido")]
        [MaxLength(60,ErrorMessage ="Nombre debe ser con un maximo de 60 Caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Descripcion Requerido")]
        [MaxLength(150, ErrorMessage = "Descripcion debe ser con un maximo de 150 Caracteres")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El Estado Requerido")]
        public bool Estado { get; set; }

    }
}
