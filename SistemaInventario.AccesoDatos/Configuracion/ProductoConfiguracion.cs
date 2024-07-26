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
    public class ProductoConfiguracion : IEntityTypeConfiguration<Producto>//aca le decimos que utilice el modelo que creamos
    {

        public void Configure(EntityTypeBuilder<Producto> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.NumeroSerie).IsRequired().HasMaxLength(60);
            builder.Property(b => b.Descripcion).IsRequired().HasMaxLength(150);
            builder.Property(b => b.Estado).IsRequired();
            builder.Property(b => b.Precio).IsRequired();
            builder.Property(b => b.Costo).IsRequired();
            builder.Property(b => b.CategoriaId).IsRequired();
            builder.Property(b => b.MarcaId).IsRequired();
            builder.Property(b => b.ImagenUrl).IsRequired(false);
            builder.Property(b => b.PadreId).IsRequired(false);

            /*relaciones*/
            builder.HasOne(b => b.Categoria).WithMany()
                   .HasForeignKey(b => b.CategoriaId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(b => b.Marca).WithMany()
                   .HasForeignKey(b => b.MarcaId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(b => b.Padre).WithMany()
                   .HasForeignKey(b => b.PadreId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
