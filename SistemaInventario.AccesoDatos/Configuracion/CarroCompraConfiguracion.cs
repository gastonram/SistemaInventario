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
    public class CarroCompraConfiguracion : IEntityTypeConfiguration<CarroCompra>
    {

        public void Configure(EntityTypeBuilder<CarroCompra> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.UsuarioAplicacionId).IsRequired();
            builder.Property(b => b.ProductoId).IsRequired();
            builder.Property(b => b.Cantidad).IsRequired();



            /* Relaciones del comdelo*/
            builder.HasOne(x=>x.UsuarioAplicacion).WithMany()
                .HasForeignKey(x=>x.UsuarioAplicacionId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Producto).WithMany()
                .HasForeignKey(x => x.ProductoId)
                .OnDelete(DeleteBehavior.NoAction);

            

        }
    }
}
