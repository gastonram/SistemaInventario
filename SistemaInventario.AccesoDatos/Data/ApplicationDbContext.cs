using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SistemaInventario.Modelos;
using System.Reflection;

namespace SistemaInventario.AccesoDatos.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        //debemos hacer las migraciones apuntando al paquete donde estan los acceso a datos en SistemaInventario.AccesoDatos
        public DbSet<Bodega> Bodegas { get; set; }
        public DbSet<Categoria>Categorias  { get; set; }
        public DbSet<Marca> Marcas { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            //builder.Entity<Bodega>().ToTable("Bodega");
        }
    }
}
