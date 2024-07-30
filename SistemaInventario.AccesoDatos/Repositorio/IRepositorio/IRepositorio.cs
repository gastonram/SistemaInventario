using SistemaInventario.Modelos.Especificaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio.IRepositorio
{
    //creamos una interfaz generica para los repositorios que recibe un tipo generico y ejecuta las operaciones basicas de un CRUD
    public interface IRepositorio<T> where T : class
    {
        //Task<T> es un tipo de retorno que se utiliza para operaciones asincronas
        Task<T> Obtener(int id);

        //IEnumerable<T> es una interfaz que define una coleccion de objetos que se pueden enumerar
        Task< IEnumerable<T>> ObtenerTodos(
            Expression<Func<T, bool>> filtro = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> ordenarPor = null,
            string incluirPropiedades = null,
            bool seguimientoEntidades = true
            );

        //paginado de la lista
        PagedList<T> ObtenerTodosPaginado(Parametros parametros, Expression<Func<T, bool>> filtro = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> ordenarPor = null,
            string incluirPropiedades = null,
            bool seguimientoEntidades = true
            );

        Task<T> ObtenerPrimero(
            Expression<Func<T, bool>> filtro = null,
            string incluirPropiedades = null,
            bool seguimientoEntidades = true
            );

        Task Agregar(T entidad);

        //estos metodos no pueden ser asincronos porque no devuelven ningun valor
        void Remover(int id);

        void Remover(IEnumerable<T> entidad);

    }
}
