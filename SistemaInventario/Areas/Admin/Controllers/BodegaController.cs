﻿using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repositorio;
using SistemaInventario.Modelos;

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

        public async Task<IActionResult> Upsert(int? id)//de tipo get
        {
            Bodega bodega = new Bodega();

            if (id == null)
            {
                //creamos una nueva bodega
                bodega.Estado = true;
                return View(bodega);
            }
            //actualizamos Bodega
            bodega = await _unidadTrabajo.Bodega.Obtener(id.GetValueOrDefault());
            if (bodega == null)
            {
                return NotFound();
            }
            return View(bodega);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Bodega bodega)
        {
            if (ModelState.IsValid)//valida que el modelo queestoy recibiendo sea valido
            {
                if (bodega.Id == 0)//se trata de un uevo registro
                {
                    await _unidadTrabajo.Bodega.Agregar(bodega);//los metodos stan en la carpeta IRepositorio
                }
                else
                {
                    _unidadTrabajo.Bodega.Actualizar(bodega);
                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            return View(bodega);//si el modelo no es valido lo regresa a la vista 
        }   


        #region API

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todas = await _unidadTrabajo.Bodega.ObtenerTodos();
            return Json(new { data = todas });//en el javascript se lo va a referenciar por el nombre de data
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)//este metodo se encuentra en el javascript bodega.js
        {
            var bodegaDesdeDb = await _unidadTrabajo.Bodega.Obtener(id);
            if (bodegaDesdeDb == null)
            {
                return Json(new { success = false, message = "Error al borrar la bodega" });
            }
            _unidadTrabajo.Bodega.Remover(bodegaDesdeDb.Id);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Bodega borrada con exito" });
        }

        #endregion
    }
}
