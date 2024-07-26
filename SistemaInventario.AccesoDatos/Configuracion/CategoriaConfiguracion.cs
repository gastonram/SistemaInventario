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
    public class CategoriaConfiguracion : IEntityTypeConfiguration<Categoria>
    {

        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Nombre).IsRequired().HasMaxLength(60);
            builder.Property(b => b.Descripcion).IsRequired().HasMaxLength(120);
            builder.Property(b => b.Estado).IsRequired();
        }
    }
}
