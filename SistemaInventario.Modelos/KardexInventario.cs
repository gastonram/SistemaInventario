using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.Modelos
{
    public class KardexInventario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int BodeaProductoId { get; set; }

        [ForeignKey("BodeaProductoId")]
        public BodegaProducto BodegaProducto { get; set; }

        [Required]
        [MaxLength(100)]
        public string Tipo { get; set; }// marca si entrada de producto o salida en caso de venta

        [Required]
        public string Detalle { get; set; }

        [Required]
        public int StockAnterior { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        public double Costo { get; set; }

        [Required]
        public int Stock { get; set; }

        [Required]
        public double Total { get; set; }

        [Required]
        public string UsuarioAplicacionId { get; set; }

        [ForeignKey("UsuarioAplicacionId")]
        public UsuarioAplicacion UsuarioAplicacion { get; set; }

        [Required]
        public DateTime FechaRegistro { get; set; }

    }
}
