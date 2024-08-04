﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos.ViewModels;
using SistemaInventario.Utilidades;
using System.Security.Claims;

namespace SistemaInventario.Areas.Inventario.Controllers
{
    [Area("Inventario")]//siempre debemos indicarle el atributo del area a la que pertence
    public class CarroController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        public CarroController(IUnidadTrabajo unidadTrabajo)//siempre debemos inyectar la unidad de trabajo
        {
            _unidadTrabajo = unidadTrabajo;
        }

        [BindProperty]//para poder usar e instanciar el objeto CarroComprasVM en la vista
        public CarroComprasVM CarroComprasVM { get; set; }

        [Authorize]// le indicamos que solo los usuarios logueados puedan acceder a esta vista
        public async Task<IActionResult> Index()
        {
            //capturo al usuario conectado
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            CarroComprasVM = new CarroComprasVM();
            CarroComprasVM.Orden = new Modelos.Orden();
            CarroComprasVM.CarroCompraLista = await _unidadTrabajo.CarroCompra.ObtenerTodos(u => u.UsuarioAplicacionId == claim.Value,
                                                                                            incluirPropiedades: "Producto");

            CarroComprasVM.Orden.TotalOrden = 0;
            CarroComprasVM.Orden.UsuarioAplicacionId = claim.Value;

            foreach (var lista in CarroComprasVM.CarroCompraLista)
            {
                lista.Precio = lista.Producto.Precio;//siempre muestra el precio actual del producto
                CarroComprasVM.Orden.TotalOrden += (lista.Precio * lista.Cantidad);
            }

            return View(CarroComprasVM);
        }

        [Authorize]
        public async Task<IActionResult> mas(int carroId)
        {
            var carroCompras = await _unidadTrabajo.CarroCompra.ObtenerPrimero(c => c.Id == carroId);//obtenemos el carro de compras
            carroCompras.Cantidad += 1;//aumentamos la cantidad
            await _unidadTrabajo.Guardar();
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> menos(int carroId)
        {
            var carroCompras = await _unidadTrabajo.CarroCompra.ObtenerPrimero(c => c.Id == carroId);//obtenemos el carro de compras
            if (carroCompras.Cantidad == 1)//si la cantidad es 1 hay que remover el registro
            {
                //removemos el registro y actualizamos la sesion
                var carroLista = await _unidadTrabajo.CarroCompra.ObtenerTodos(u => u.UsuarioAplicacionId == carroCompras.UsuarioAplicacionId);//obtenemos la lista de carros de compras del usuario
                var numeroProductos = carroLista.Count();
                _unidadTrabajo.CarroCompra.Remover(carroCompras.Id);
                await _unidadTrabajo.Guardar();

                HttpContext.Session.SetInt32(DS.ssCarroCompras, numeroProductos - 1);
            }
            else
            {
                carroCompras.Cantidad -= 1;//disminuimos la cantidad
                await _unidadTrabajo.Guardar();

            }

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> remover(int carroId)
        {
            //Remueve el registro del carro de compras y actualiza la sesion
            var carroCompras = await _unidadTrabajo.CarroCompra.ObtenerPrimero(c => c.Id == carroId);//obtenemos el carro de compras
                                                                                                     //removemos el registro y actualizamos la sesion
            var carroLista = await _unidadTrabajo.CarroCompra.ObtenerTodos(u => u.UsuarioAplicacionId == carroCompras.UsuarioAplicacionId);//obtenemos la lista de carros de compras del usuario
            var numeroProductos = carroLista.Count();
            _unidadTrabajo.CarroCompra.Remover(carroCompras.Id);
            await _unidadTrabajo.Guardar();

            HttpContext.Session.SetInt32(DS.ssCarroCompras, numeroProductos - 1);
            return RedirectToAction("Index");
        }

    }
}
