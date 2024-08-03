using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.Modelos
{
    public class Inventario
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UsuarioAplicacionId { get; set; }

        [ForeignKey("UsuarioAplicacionId")]
        public UsuarioAplicacion UsuarioAplicacion { get; set; }

        [Required]
        public DateTime FechaInicial { get; set; }

        [Required]
        public DateTime FechaFinal { get; set; }

        [Required(ErrorMessage ="Debe seleccionar una Bodega para continuar")]//los mensajes de error se pueden personalizar de esta forma para que se vean en la vista
        public int BodegaId { get; set; }

        [ForeignKey("BodegaId")]
        public Bodega Bodega { get; set; }

        [Required]
        public bool Estado { get; set; }

    }
}
