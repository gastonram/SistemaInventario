﻿using Microsoft.EntityFrameworkCore;
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
    public class InventarioConfiguraciona : IEntityTypeConfiguration<Inventario>//aca le decimos que utilice el modelo que creamos
    {

        public void Configure(EntityTypeBuilder<Inventario> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.BodegaId).IsRequired();
            builder.Property(b => b.UsuarioAplicacionId).IsRequired();
            builder.Property(b => b.FechaInicial).IsRequired();
            builder.Property(b => b.FechaFinal).IsRequired();
            builder.Property(b => b.Estado).IsRequired();

            //Relaciones 
            builder.HasOne(b => b.Bodega).WithMany()
                .HasForeignKey(b => b.BodegaId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(b => b.UsuarioAplicacion).WithMany()
                .HasForeignKey(b => b.UsuarioAplicacionId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
