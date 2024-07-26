using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio.IRepositorio
{
    //interfaz que se encarga de manejar la unidad de trabajo,IDisposable se encarga de liberar recursos no administrados, objetos que esten consuminedo recursos innecesariamente
    public interface IUnidadTrabajo : IDisposable
    //esta interfaz se encarga de manejar la unidad de trabajo es decir de manejar todas las operaciones de la base de datos
    {

        IBodegaRepositorio Bodega { get; }
        ICategoriaRepositorio Categoria { get; }

        Task Guardar();
    }
}
