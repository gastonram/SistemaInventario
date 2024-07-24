using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repositorio;

namespace SistemaInventario.Areas.Admin.Controllers
{
    [Area("Admin")]//siempre debemos indicar a que area pertenece el controllador con el que estamnos trabajando, sino da error
    public class BodegaController : Controller
    {
        //referenciamos a nuestra unidad de trabajo
        private readonly IUnidadTrabajo _unidadTrabajo;

        public BodegaController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        //debemos importar una libreria para que las bodegas se desplieguen en un datable con un metodo api 
        public IActionResult Index()
        {
            return View();
        }



        #region API

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todas = await _unidadTrabajo.Bodega.ObtenerTodos();
            return Json(new { data = todas });//en el javascript se lo va a referenciar por el nombre de data
        }

        #endregion
    }
}
