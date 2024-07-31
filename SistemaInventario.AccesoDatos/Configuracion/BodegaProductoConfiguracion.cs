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
    public class BodegaProductoConfiguracion : IEntityTypeConfiguration<BodegaProducto>//aca le decimos que utilice el modelo que creamos
    {

        public void Configure(EntityTypeBuilder<BodegaProducto> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.BodegaId).IsRequired();
            builder.Property(b => b.ProductoId).IsRequired();
            builder.Property(b => b.Cantidad).IsRequired();

            //Relaciones 
            builder.HasOne(b => b.Bodega).WithMany()
                .HasForeignKey(b => b.BodegaId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(b => b.Producto).WithMany()
                .HasForeignKey(b => b.ProductoId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
