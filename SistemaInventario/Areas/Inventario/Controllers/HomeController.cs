using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using SistemaInventario.Modelos.Especificaciones;
using SistemaInventario.Modelos.ViewModel;

using System.Diagnostics;

namespace SistemaInventario.Areas.Inventario.Controllers
{
    [Area("Inventario")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnidadTrabajo _unidadTrabajo;

        public HomeController(ILogger<HomeController> logger,IUnidadTrabajo unidadTrabajo )
        {
            _logger = logger;
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index(int pagenumber = 1,string busqueda="",string busquedaActual="")
        {
            if ( !String.IsNullOrEmpty(busqueda))
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
                resultado = _unidadTrabajo.Producto.ObtenerTodosPaginado(parametros,p=>p.Descripcion.Contains(busqueda));

            }

            //Sentencia para configurar la paginacion
            ViewData["TotalPAginas"] = resultado.MetaData.TotalPages;
            ViewData["TotalRegistros"] = resultado.MetaData.TotalCount;
            ViewData["Pagesize"] = resultado.MetaData.PageSize;
            ViewData["PageNumber"] = pagenumber;
            ViewData["Previo"] = "disabled";//clase css para deshabilitar el boton
            ViewData["Siguiente"] = "";

            if(pagenumber > 1) { ViewData["Previo"] = ""; }
            if (resultado.MetaData.TotalPages <= pagenumber) { ViewData["siguiente"] = "disabled"; }


            return View(resultado);
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
