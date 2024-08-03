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
    public class CompaniaConfiguracion : IEntityTypeConfiguration<Compania>
    {

        public void Configure(EntityTypeBuilder<Compania> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Nombre).IsRequired().HasMaxLength(60);
            builder.Property(b => b.Descripcion).IsRequired().HasMaxLength(120);
            builder.Property(b => b.Pais).IsRequired();
            builder.Property(b => b.Ciudad).IsRequired();
            builder.Property(b => b.Direccion).IsRequired();
            builder.Property(b => b.Telefono).IsRequired();
            builder.Property(b => b.BodegaVentaId).IsRequired();
            builder.Property(b => b.CreadoPorId).IsRequired(false);
            builder.Property(b => b.ActualizadoPorId).IsRequired(false);

            /* Relaciones del comdelo*/
            builder.HasOne(x=>x.Bodega).WithMany()
                .HasForeignKey(x=>x.BodegaVentaId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.CreadoPor).WithMany()
                .HasForeignKey(x => x.CreadoPorId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.ActualizadoPor).WithMany()
                .HasForeignKey(x => x.ActualizadoPorId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
