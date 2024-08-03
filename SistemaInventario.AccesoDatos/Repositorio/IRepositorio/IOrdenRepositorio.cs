using SistemaInventario.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio.IRepositorio
{
    public interface IOrdenRepositorio : IRepositorio<Orden>
    {
        //recibe una bodega y la actualiza en la base de datos
        //se maneja de manera individual ya que cada objeto es particular
        void Actualizar(Orden orden);

    }
}
