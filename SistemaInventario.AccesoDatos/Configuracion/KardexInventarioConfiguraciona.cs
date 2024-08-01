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
    public class KardexInventarioConfiguraciona : IEntityTypeConfiguration<KardexInventario>//aca le decimos que utilice el modelo que creamos
    {

        public void Configure(EntityTypeBuilder<KardexInventario> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.BodeaProductoId).IsRequired();
            builder.Property(b => b.Tipo).IsRequired();
            builder.Property(b => b.Detalle).IsRequired();
            builder.Property(b => b.StockAnterior).IsRequired();
            builder.Property(b => b.Cantidad).IsRequired();
            builder.Property(b => b.Costo).IsRequired();
            builder.Property(b => b.Stock).IsRequired();
            builder.Property(b => b.Total).IsRequired();
            builder.Property(b => b.UsuarioAplicacionId).IsRequired();
            builder.Property(b => b.FechaRegistro).IsRequired();


            //Relaciones 
            builder.HasOne(b => b.BodegaProducto).WithMany()
                .HasForeignKey(b => b.BodeaProductoId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(b => b.UsuarioAplicacion).WithMany()
                .HasForeignKey(b => b.UsuarioAplicacionId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
