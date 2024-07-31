using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using SistemaInventario.Utilidades;

namespace SistemaInventario.Areas.Admin.Controllers
{
    [Area("Admin")]//siempre debemos indicar a que area pertenece el controllador con el que estamnos trabajando, sino da error
    [Authorize(Roles = DS.Rol_Admin)]
    public class MarcaController : Controller
    {
        //referenciamos a nuestra unidad de trabajo
        private readonly IUnidadTrabajo _unidadTrabajo;

        public MarcaController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        //debemos importar una libreria para que las bodegas se desplieguen en un datable con un metodo api 
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)//de tipo get
        {
            Marca marca = new Marca();

            if (id == null)
            {
                //creamos una nueva categoria
                marca.Estado = true;
                return View(marca);
            }
            //actualizamos categoria
            marca = await _unidadTrabajo.Marca.Obtener(id.GetValueOrDefault());
            if (marca == null)
            {
                return NotFound();
            }
            return View(marca);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Marca marca)
        {
            if (ModelState.IsValid)//valida que el modelo queestoy recibiendo sea valido
            {
                if (marca.Id == 0)//se trata de un uevo registro
                {
                    await _unidadTrabajo.Marca.Agregar(marca);//los metodos stan en la carpeta IRepositorio
                    TempData[DS.Exitosa] = "Se creo la Marca correctamente";
                }
                else
                {
                    _unidadTrabajo.Marca.Actualizar(marca);
                    TempData[DS.Exitosa] = "Se modifico la Marca de manera exitosa";

                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error al guardar la Marca";
            return View(marca);//si el modelo no es valido lo regresa a la vista 
        }   


        #region API

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todas = await _unidadTrabajo.Marca.ObtenerTodos();
            return Json(new { data = todas });//en el javascript se lo va a referenciar por el nombre de data
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)//este metodo se encuentra en el javascript categoria.js
        {
            var marcaDesdeDb = await _unidadTrabajo.Marca.Obtener(id);
            if (marcaDesdeDb == null)
            {
                return Json(new { success = false, message = "Error al borrar la Marca" });
            }
            _unidadTrabajo.Marca.Remover(marcaDesdeDb.Id);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Marca borrada con exito" });
        }


        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id =0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.Marca.ObtenerTodos();
            if(id==0)
            {
                valor = lista.Any(x => x.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            }
            else
            {
                valor = lista.Any(x => x.Nombre.ToLower().Trim() == nombre.ToLower().Trim() && x.Id != id);

            }
            if (valor)
            {
                return Json(new {data = true});
            }
            return Json(new { data = false });
        }

        #endregion
    }
}
