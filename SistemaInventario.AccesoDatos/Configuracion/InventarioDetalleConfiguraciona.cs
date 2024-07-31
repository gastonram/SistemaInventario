using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaInventario.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Configuracion
{
    public class InventarioDetalleConfiguraciona : IEntityTypeConfiguration<InventarioDetalle>//aca le decimos que utilice el modelo que creamos
    {

        public void Configure(EntityTypeBuilder<InventarioDetalle> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.InventarioId).IsRequired();
            builder.Property(b => b.ProductoId).IsRequired();
            builder.Property(b => b.StockAnterior).IsRequired();
            builder.Property(b => b.Cantidad).IsRequired();


            //Relaciones 
            builder.HasOne(b => b.Inventario).WithMany()
                .HasForeignKey(b => b.InventarioId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(b => b.Producto).WithMany()
                .HasForeignKey(b => b.ProductoId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
