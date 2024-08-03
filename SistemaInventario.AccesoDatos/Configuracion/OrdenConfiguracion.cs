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
    public class OrdenConfiguracion : IEntityTypeConfiguration<Orden>
    {

        public void Configure(EntityTypeBuilder<Orden> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.UsuarioAplicacionId).IsRequired();
            builder.Property(b => b.FechaOrden).IsRequired();
            builder.Property(b => b.TotalOrden).IsRequired();
            builder.Property(b => b.EstadoOrden).IsRequired();
            builder.Property(b => b.EstadoPago).IsRequired();
            builder.Property(b => b.NombresCliente).IsRequired();
            builder.Property(b => b.NumeroEnvio).IsRequired(false);
            builder.Property(b => b.Carrier).IsRequired(false);
            builder.Property(b => b.TransaccionId).IsRequired(false);
            builder.Property(b => b.Telefono).IsRequired(false);
            builder.Property(b => b.Direccion).IsRequired(false);
            builder.Property(b => b.Ciudad).IsRequired(false);
            builder.Property(b => b.Pais).IsRequired(false);



            /* Relaciones del comdelo*/
            builder.HasOne(x=>x.UsuarioAplicacion).WithMany()
                .HasForeignKey(x=>x.UsuarioAplicacionId)
                .OnDelete(DeleteBehavior.NoAction);


            

        }
    }
}
