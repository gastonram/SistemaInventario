using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using SistemaInventario.Modelos.ViewModels;
using SistemaInventario.Utilidades;

namespace SistemaInventario.Areas.Admin.Controllers
{
    [Area("Admin")]//siempre debemos indicar a que area pertenece el controllador con el que estamnos trabajando, sino da error
    [Authorize(Roles = DS.Rol_Admin + "," + DS.Rol_Inventario)]
    public class ProductoController : Controller
    {
        //referenciamos a nuestra unidad de trabajo
        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductoController(IUnidadTrabajo unidadTrabajo, IWebHostEnvironment webHostEnvironment)
        {
            _unidadTrabajo = unidadTrabajo;
            _webHostEnvironment = webHostEnvironment;
        }

        //debemos importar una libreria para que las bodegas se desplieguen en un datable con un metodo api 
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)//de tipo get
        {
            ProductoVM productoVM = new ProductoVM()
            {
                Producto = new Producto(),
                CategoriaLista = _unidadTrabajo.Producto.ObtenerTodosDropdownList("Categoria"),
                MarcaLista = _unidadTrabajo.Producto.ObtenerTodosDropdownList("Marca"),
                PadreLista = _unidadTrabajo.Producto.ObtenerTodosDropdownList("Producto")
            };
            if (id == null)
            {
                //Crear nuevo Producto
                productoVM.Producto.Estado = true;
                return View(productoVM);
            }
            else
            {
                productoVM.Producto = await _unidadTrabajo.Producto.Obtener(id.GetValueOrDefault());
                if (productoVM.Producto == null)
                {
                    return NotFound();
                }
                return View(productoVM);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ProductoVM productoVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;//captura los archivos que se estan enviando desde el formulario
                string webRootPath = _webHostEnvironment.WebRootPath;//captura la ruta de la carpeta wwwroot

                if (productoVM.Producto.Id == 0)
                {
                    //Nuevo Producto
                    string upload = webRootPath + DS.ImagenRuta;
                    string filename = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);//capturo la estension del primer archivo del array

                    using (var filesStream = new FileStream(Path.Combine(upload, filename + extension), FileMode.Create))
                    {
                        files[0].CopyTo(filesStream);//copiamos el archivo a la ruta que le indicamos en espacio en memoria
                    }
                    productoVM.Producto.ImagenUrl = filename + extension;
                    await _unidadTrabajo.Producto.Agregar(productoVM.Producto);
                }
                else
                {
                    //Editar Producto
                    var objProducto = await _unidadTrabajo.Producto.ObtenerPrimero(p => p.Id == productoVM.Producto.Id, seguimientoEntidades: false);
                    if (files.Count > 0)// si el usuario selecciono una nueva imagen para el Producto existente
                    {
                        string upload = webRootPath + DS.ImagenRuta;
                        string filename = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);//tomo el primer elemento del array files y obtengo la extension del archivo

                        //Borrar imagen anterior
                        var anteriorFilePath = Path.Combine(upload, objProducto.ImagenUrl);
                        if (System.IO.File.Exists(anteriorFilePath))//busco si existe en mis carpetas de archivo
                        {
                            System.IO.File.Delete(anteriorFilePath);//si existe lo borro
                        }
                        using (var fileStream = new FileStream(Path.Combine(upload, filename + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }
                        productoVM.Producto.ImagenUrl = filename + extension;
                    }// caso contrario si no se carga una nueva imagen conservo la imagen anterior
                    else
                    {
                        productoVM.Producto.ImagenUrl = objProducto.ImagenUrl;
                    }

                    _unidadTrabajo.Producto.Actualizar(productoVM.Producto);
                }
                TempData[DS.Exitosa] = "Producto guardado con exito";//esto es un mensaje que se va a mostrar en la vista index
                await _unidadTrabajo.Guardar();
                return View("Index");
            }
            else
            {
                productoVM.CategoriaLista = _unidadTrabajo.Producto.ObtenerTodosDropdownList("Categoria");
                productoVM.MarcaLista = _unidadTrabajo.Producto.ObtenerTodosDropdownList("Marca");
                productoVM.PadreLista = _unidadTrabajo.Producto.ObtenerTodosDropdownList("Producto");

                return View(productoVM);
            }
        }

        #region API

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todas = await _unidadTrabajo.Producto.ObtenerTodos(incluirPropiedades: "Categoria,Marca");//esto nos traera los datos del Producto y tambien a que categoria y marca pertenecen
            return Json(new { data = todas });//en el javascript se lo va a referenciar por el nombre de data
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)//este metodo se encuentra en el javascript categoria.js
        {
            var productoDesdeDb = await _unidadTrabajo.Producto.Obtener(id);
            if (productoDesdeDb == null)
            {
                return Json(new { success = false, message = "Error al borrar el Producto" });
            }
            //debemos eliminar la imagen antes de eliminar el Producto
            string upload = _webHostEnvironment.WebRootPath + DS.ImagenRuta;
            var anteriorFilePath = Path.Combine(upload, productoDesdeDb.ImagenUrl);
            if (System.IO.File.Exists(anteriorFilePath))//busco si existe en mis carpetas de archivo
            {
                System.IO.File.Delete(anteriorFilePath);//si existe lo borro
            }

            _unidadTrabajo.Producto.Remover(productoDesdeDb.Id);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Producto borrado con exito" });
        }


        [ActionName("ValidarSerie")]
        public async Task<IActionResult> ValidarSerie(string serie, int id = 0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.Producto.ObtenerTodos();
            if (id == 0)
            {
                valor = lista.Any(x => x.NumeroSerie.ToLower().Trim() == serie.ToLower().Trim());
            }
            else
            {
                valor = lista.Any(x => x.NumeroSerie.ToLower().Trim() == serie.ToLower().Trim() && x.Id != id);

            }
            if (valor)
            {
                return Json(new { data = true });
            }
            return Json(new { data = false });
        }

        #endregion
    }
}
