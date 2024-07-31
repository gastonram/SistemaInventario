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
    public class BodegaProductoRepositorio : Repositorio<BodegaProducto>, IBodegaProductoRepositorio
    {
        //el padre me pide trabajar con un repositorio por lo que en la clase le tenemos que pasar la entidad que va a trabajar
        private readonly ApplicationDbContext _db;

        public BodegaProductoRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Actualizar(BodegaProducto bodegaproducto)
        {
            //captura por medio del dbcontext el registro por medio del id
            var bodegaproductoBD = _db.Productos.FirstOrDefault(s => s.Id == bodegaproducto.Id);
            if (bodegaproductoBD != null)
            {
                bodegaproducto.Cantidad = bodegaproducto.Cantidad;

                _db.SaveChanges();
            }
        }

    }
}
