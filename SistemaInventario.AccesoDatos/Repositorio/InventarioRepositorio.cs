using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio
{
    public class InventarioRepositorio : Repositorio<Inventario>, IInventarioRepositorio
    {
        //el padre me pide trabajar con un repositorio por lo que en la clase le tenemos que pasar la entidad que va a trabajar
        private readonly ApplicationDbContext _db;

        public InventarioRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Actualizar(Inventario inventario)
        {
            //captura por medio del dbcontext el registro por medio del id
            var inventarioBD = _db.Productos.FirstOrDefault(s => s.Id == inventario.Id);
            if (inventarioBD != null)
            {
                inventario.BodegaId = inventario.BodegaId;
                inventario.FechaFinal = inventario.FechaFinal;
                inventario.Estado = inventario.Estado;

                _db.SaveChanges();
            }
        }

    }
}
