using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio
{
    //interfaz que se encarga de manejar la unidad de trabajo,IDisposable se encarga de liberar recursos no administrados, objetos que esten consuminedo recursos innecesariamente
    public interface IUnidadTrabajo : IDisposable
    {

        IBodegaRepositorio Bodega { get; }

        Task Guardar();
    }
}
