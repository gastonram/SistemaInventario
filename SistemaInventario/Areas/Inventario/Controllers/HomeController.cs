using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using SistemaInventario.Modelos.Especificaciones;
using SistemaInventario.Modelos.ViewModel;
using SistemaInventario.Modelos.ViewModels;
using SistemaInventario.Utilidades;
using System.Diagnostics;
using System.Security.Claims;

namespace SistemaInventario.Areas.Inventario.Controllers
{
    [Area("Inventario")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnidadTrabajo _unidadTrabajo;
        [BindProperty]//le estás diciendo al framework que estas propiedades deben ser vinculadas automáticamente al modelo cuando se recibe una solicitud POST.
        public CarroComprasVM CarroCompraVM { get; set; }


        public HomeController(ILogger<HomeController> logger, IUnidadTrabajo unidadTrabajo)
        {
            _logger = logger;
            _unidadTrabajo = unidadTrabajo;
        }

        public async Task<IActionResult> Index(int pagenumber = 1, string busqueda = "", string busquedaActual = "")
        {
            //controlar la sesion
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;//capturamos el usuario que esta logueado
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);//buscamos el id del usuario
            if(claim != null)//si el usuario esta logueado
            {
                var carroLista = await _unidadTrabajo.CarroCompra.ObtenerTodos(c => c.UsuarioAplicacionId == claim.Value);
                var numeroProductos = carroLista.Count();
                HttpContext.Session.SetInt32(DS.ssCarroCompras, numeroProductos);
            }

            if (!String.IsNullOrEmpty(busqueda))
            {
                pagenumber = 1;
            }
            else
            {
                busqueda = busquedaActual;
            }
            ViewData["BusquedaActual"] = busqueda;


            if (pagenumber < 0) { pagenumber = 1; }

            Parametros parametros = new Parametros()
            {
                PageNumber = pagenumber,
                Pagesize = 4
            };
            var resultado = _unidadTrabajo.Producto.ObtenerTodosPaginado(parametros);

            //sentencia para la configuracion de la busqueda
            if (!String.IsNullOrEmpty(busqueda))
            {
                resultado = _unidadTrabajo.Producto.ObtenerTodosPaginado(parametros, p => p.Descripcion.Contains(busqueda));

            }

            //Sentencia para configurar la paginacion
            ViewData["TotalPAginas"] = resultado.MetaData.TotalPages;
            ViewData["TotalRegistros"] = resultado.MetaData.TotalCount;
            ViewData["Pagesize"] = resultado.MetaData.PageSize;
            ViewData["PageNumber"] = pagenumber;
            ViewData["Previo"] = "disabled";//clase css para deshabilitar el boton
            ViewData["Siguiente"] = "";

            if (pagenumber > 1) { ViewData["Previo"] = ""; }
            if (resultado.MetaData.TotalPages <= pagenumber) { ViewData["siguiente"] = "disabled"; }


            return View(resultado);
        }

        public async Task<IActionResult> Detalle(int id)//el nombre debe ser igual al asp-rout de la vista y recibe de parametro el id
        {
            CarroCompraVM = new CarroComprasVM();
            CarroCompraVM.Compania = await _unidadTrabajo.Compania.ObtenerPrimero();
            CarroCompraVM.Producto = await _unidadTrabajo.Producto.ObtenerPrimero(p => p.Id == id, incluirPropiedades: "Marca,Categoria");//filtro por el id que me envia la vista

            var bodegaProducto = await _unidadTrabajo.BodegaProducto.ObtenerPrimero(p => p.ProductoId == id &&
                                                                                    p.BodegaId == CarroCompraVM.Compania.BodegaVentaId);//me traigo el stock del deposito de venta

            if (bodegaProducto == null)//si no me trae nada le pongo el stock en 0
            {
                CarroCompraVM.Stock = 0;
            }
            else
            {
                CarroCompraVM.Stock = bodegaProducto.Cantidad;//le asigno la cantidad
            }
            CarroCompraVM.CarroCompra = new CarroCompra()
            {//inicializo el carro de compras con sus propias variables lo voy a utilizar en el post
                Producto = CarroCompraVM.Producto,
                ProductoId = CarroCompraVM.Producto.Id
            };
            return View(CarroCompraVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]//validamos que solo los usuarios puedan agregar al carro
        public async Task<IActionResult> Detalle(CarroComprasVM carroComprasVM)
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;//capturamos el usuario que esta logueado
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);//buscamos el id del usuario
            carroComprasVM.CarroCompra.UsuarioAplicacionId = claim.Value;//le asignamos el id del usuario al carro de compras

            CarroCompra carroBD = await _unidadTrabajo.CarroCompra.ObtenerPrimero(p => p.UsuarioAplicacionId == claim.Value &&
                                                                            p.ProductoId == carroComprasVM.CarroCompra.ProductoId);//buscamos si el producto ya esta en el carro de compras
            if (carroBD == null)//si no esta en el carro de compras
            {
                await _unidadTrabajo.CarroCompra.Agregar(carroComprasVM.CarroCompra);//agregamos el producto al carro de compras
            }
            else
            {
                carroBD.Cantidad += carroComprasVM.CarroCompra.Cantidad;//si ya esta en el carro de compras le sumamos la cantidad
                _unidadTrabajo.CarroCompra.Actualizar(carroBD);//actualizamos el carro de compras
            }
            await _unidadTrabajo.Guardar();
            TempData[DS.Exitosa] = "Producto agregado al carro de compras";
            //agrgo valor a sesion carro de compras
            var carroLista = await _unidadTrabajo.CarroCompra.ObtenerTodos(c => c.UsuarioAplicacionId == claim.Value);//de esta forma tengo la lista que solo le corresponde a este usuario
            var numeroProductos = carroLista.Count();
            HttpContext.Session.SetInt32(DS.ssCarroCompras, numeroProductos);//le asigno la cantidad de productos que tiene en el carro de compras

            return RedirectToAction("Index");

        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
