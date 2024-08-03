using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.Modelos
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="El Numero de Serie es Requerido")]
        [MaxLength(60,ErrorMessage ="El valor maximo es de 60 caracteres")]
        public string NumeroSerie { get; set; }

        [Required(ErrorMessage = "La Descripcion es Requerido")]
        [MaxLength(150, ErrorMessage = "El valor maximo es de 150 caracteres")]
        public string Descripcion { get; set; }

        
        [Required(ErrorMessage = "El Precio es Requerido")]
        public double Precio { get; set; }

        [Required(ErrorMessage = "El Costo es Requerido")]
        public double Costo { get; set; }

        public string ImagenUrl { get; set; }

        public bool Estado { get; set; }

        [Required(ErrorMessage = "La Categoria es Requerida")]
        public int CategoriaId { get; set; }

        [ForeignKey("CategoriaId")]
        public Categoria Categoria { get; set; }

        [Required(ErrorMessage = "La Marca es Requerida")]
        public int MarcaId { get; set; }//para relacionarlos con la tabla marca

        [ForeignKey("MarcaId")]//para relacionarlos con la tabla marca debe ser exactamente el mismo nombre que la propiedad
        public Marca Marca { get; set; }

        public int? PadreId { get; set; }//una propiedad padre que se relacione con la misma tabla de mi modelo Producto
                                         //, necesitamos que se grabe como null en la base de datos porque sino se guarda como 0 y trae problemas
        public virtual Producto Padre { get; set; }//relacion con la misma tabla de Producto, recursividad. un Producto puede estar relacionado a un mismo Producto


    }
}
