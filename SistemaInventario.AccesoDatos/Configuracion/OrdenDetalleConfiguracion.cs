using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaInventario.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Configuracion
{
    public class OrdenDetalleConfiguracion : IEntityTypeConfiguration<OrdenDetalle>
    {

        public void Configure(EntityTypeBuilder<OrdenDetalle> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b =>b.OrdenId).IsRequired();
            builder.Property(b => b.ProductoId).IsRequired();
            builder.Property(b => b.Cantidad).IsRequired();
            builder.Property(b => b.Precio).IsRequired();



            /* Relaciones del comdelo*/
            builder.HasOne(x=>x.Orden).WithMany()
                .HasForeignKey(x=>x.OrdenId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Producto).WithMany()
                .HasForeignKey(x => x.ProductoId)
                .OnDelete(DeleteBehavior.NoAction);





        }
    }
}
