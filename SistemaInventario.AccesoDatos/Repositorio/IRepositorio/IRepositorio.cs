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
        T Obtener(int id);

        IEnumerable<T> ObtenerTodos(
            Expression<Func<T, bool>> filtro = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> ordenarPor = null,
            string incluirPropiedades = null,
            bool seguimientoEntidades = true
            );

        T ObtenerPrimero(
            Expression<Func<T, bool>> filtro = null,
            string incluirPropiedades = null,
            bool seguimientoEntidades = true
            );

        void agregar(T entidad);

        void Remover(int id);

        void Remover(IEnumerable<T> entidad);

    }
}
