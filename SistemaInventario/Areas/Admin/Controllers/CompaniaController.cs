using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos.ViewModels;
using SistemaInventario.Utilidades;
using System.Security.Claims;

namespace SistemaInventario.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Rol_Admin)]
    public class CompaniaController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        public CompaniaController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }


        public async Task<IActionResult> Upsert()
        {
            CompaniaVM companiaVM = new CompaniaVM()
            {
                Compania = new Modelos.Compania(),
                BodegaLista = _unidadTrabajo.Inventario.ObtenerTodosDropdownLista("Bodega")
            };

            companiaVM.Compania = await _unidadTrabajo.Compania.ObtenerPrimero();

            if (companiaVM.Compania == null)//si no hay compania
            {
                companiaVM.Compania = new Modelos.Compania();//la instanciamos para que no genere error en la vista
            }

            return View(companiaVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(CompaniaVM companiaVM)
        {
            if (ModelState.IsValid)
            {
                TempData[DS.Exitosa] = "Compania Grabada Exitosamente";
                var ClaimIdentity = (ClaimsIdentity)User.Identity;//capturo el usuario de la sesion
                var claim = ClaimIdentity.FindFirst(ClaimTypes.NameIdentifier);

                if (companiaVM.Compania.Id == 0)//creo compania
                {
                    companiaVM.Compania.CreadoPorId = claim.Value;
                    companiaVM.Compania.ActualizadoPorId = claim.Value;
                    companiaVM.Compania.FechaCreacion = DateTime.Now;
                    companiaVM.Compania.FechaActualizacion = DateTime.Now;
                    await _unidadTrabajo.Compania.Agregar(companiaVM.Compania);
                }
                else//actualizo la compania
                {
                    companiaVM.Compania.ActualizadoPorId = claim.Value;
                    companiaVM.Compania.FechaActualizacion = DateTime.Now;
                    _unidadTrabajo.Compania.Actualizar(companiaVM.Compania);
                }

                await _unidadTrabajo.Guardar();
                return RedirectToAction("Index", "Home", new { area = "Inventario" });//despues de grabar redirige a la pagina principal y le especificamos que es de are inventario
            }
            TempData[DS.Error] = "Error al grabar la Compania";
            return View(companiaVM);
        }

    }
}
