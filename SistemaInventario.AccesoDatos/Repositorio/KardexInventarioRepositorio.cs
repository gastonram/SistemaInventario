using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
    public class KardexInventarioRepositorio : Repositorio<KardexInventario>, IKardexInventarioRepositorio
    {
        //el padre me pide trabajar con un repositorio por lo que en la clase le tenemos que pasar la entidad que va a trabajar
        private readonly ApplicationDbContext _db;

        public KardexInventarioRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task RegistrarKardex(int bodegaProductoId, string tipo, string detalle, int stockAnterior, int cantidad, string usuarioId)
        {
            var bodegaProducto = await _db.BodegasProductos.Include(b => b.Producto).FirstOrDefaultAsync(b => b.Id == bodegaProductoId);

            if (tipo == "Entrada")//si el tipo es entrada
            {
                KardexInventario kardex = new KardexInventario();
                {
                    kardex.BodeaProductoId = bodegaProductoId;
                    kardex.Tipo = tipo;
                    kardex.Detalle = detalle;
                    kardex.StockAnterior = stockAnterior;
                    kardex.Cantidad = cantidad;
                    kardex.Costo = bodegaProducto.Producto.Costo;
                    kardex.Stock = stockAnterior + cantidad;//se suma la cantidad de productos que se compraron
                    kardex.Total = kardex.Stock * kardex.Costo;
                    kardex.UsuarioAplicacionId = usuarioId;
                    kardex.FechaRegistro = DateTime.Now;

                    await _db.KardexInventarios.AddAsync(kardex);
                    await _db.SaveChangesAsync();

                };
            }
            if (tipo == "Salida")
            {
                KardexInventario kardex = new KardexInventario();
                {
                    kardex.BodeaProductoId = bodegaProductoId;
                    kardex.Tipo = tipo;
                    kardex.Detalle = detalle;
                    kardex.StockAnterior = stockAnterior;
                    kardex.Cantidad = cantidad;
                    kardex.Costo = bodegaProducto.Producto.Costo;
                    kardex.Stock = stockAnterior - cantidad;//se resta la cantidad de productos que se vendieron
                    kardex.Total = kardex.Stock * kardex.Costo;
                    kardex.UsuarioAplicacionId = usuarioId;
                    kardex.FechaRegistro = DateTime.Now;

                    await _db.KardexInventarios.AddAsync(kardex);
                    await _db.SaveChangesAsync();

                };
            }
        }
    }
}
