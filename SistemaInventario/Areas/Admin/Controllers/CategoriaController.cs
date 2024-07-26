using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using SistemaInventario.Utilidades;

namespace SistemaInventario.Areas.Admin.Controllers
{
    [Area("Admin")]//siempre debemos indicar a que area pertenece el controllador con el que estamnos trabajando, sino da error
    public class CategoriaController : Controller
    {
        //referenciamos a nuestra unidad de trabajo
        private readonly IUnidadTrabajo _unidadTrabajo;

        public CategoriaController(IUnidadTrabajo unidadTrabajo)
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
            Categoria categoria = new Categoria();

            if (id == null)
            {
                //creamos una nueva categoria
                categoria.Estado = true;
                return View(categoria);
            }
            //actualizamos categoria
            categoria = await _unidadTrabajo.Categoria.Obtener(id.GetValueOrDefault());
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Categoria categoria)
        {
            if (ModelState.IsValid)//valida que el modelo queestoy recibiendo sea valido
            {
                if (categoria.Id == 0)//se trata de un uevo registro
                {
                    await _unidadTrabajo.Categoria.Agregar(categoria);//los metodos stan en la carpeta IRepositorio
                    TempData[DS.Exitosa] = "Se creo la Categoria correctamente";
                }
                else
                {
                    _unidadTrabajo.Categoria.Actualizar(categoria);
                    TempData[DS.Exitosa] = "Se modifico la Categoria de manera exitosa";

                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error al guardar la Categoria";
            return View(categoria);//si el modelo no es valido lo regresa a la vista 
        }   


        #region API

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todas = await _unidadTrabajo.Categoria.ObtenerTodos();
            return Json(new { data = todas });//en el javascript se lo va a referenciar por el nombre de data
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)//este metodo se encuentra en el javascript categoria.js
        {
            var categoriaDesdeDb = await _unidadTrabajo.Categoria.Obtener(id);
            if (categoriaDesdeDb == null)
            {
                return Json(new { success = false, message = "Error al borrar la categoria" });
            }
            _unidadTrabajo.Categoria.Remover(categoriaDesdeDb.Id);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Categoria borrada con exito" });
        }


        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id =0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.Categoria.ObtenerTodos();
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
