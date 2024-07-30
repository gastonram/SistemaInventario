using Microsoft.EntityFrameworkCore;
using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos.Especificaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio
{
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        private readonly ApplicationDbContext _bd;
        internal DbSet<T> dbSet;

        //constructor que recibe el contexto de la base de datos y lo asigna a la variable _bd y asigna el dbSet a la entidad generica
        public Repositorio(ApplicationDbContext bd)
        {
            _bd = bd;
            this.dbSet = _bd.Set<T>();
        }

        public async Task Agregar(T entidad)
        {
           await dbSet.AddAsync(entidad);//agrega la entidad a la base de datos
        }

        public async Task<T> Obtener(int id)
        {
            return await dbSet.FindAsync(id); //busca la entidad por el id
        }

        public async Task<IEnumerable<T>> ObtenerTodos(Expression<Func<T, bool>> filtro,
            Func<IQueryable<T>, IOrderedQueryable<T>> ordenarPor, string incluirPropiedades, bool seguimientoEntidades)
        {
            IQueryable<T> query = dbSet;
            if (filtro != null)
            {
                query = query.Where(filtro);//aplica el filtro a la consulta select/* from table where filtro
            }
            if (incluirPropiedades != null)
            {
                //es una cadena de caracteres, la recorro caracter por caracter y la separo por comas y la convierto en un array de string y la recorro con un foreach y la incluyo en la consulta 
                foreach (var propiedad in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(propiedad);//incluye las propiedades de la entidad
                }
            }
            if (ordenarPor != null)
            {
                //ejecuta la consulta y la ordena por lo que se envia por parametro
                return await ordenarPor(query).ToListAsync();//ordena la consulta
            }
            if (!seguimientoEntidades)
            {
                query = query.AsNoTracking();//no sigue las entidades
            }

            return await query.ToListAsync();//ejecuta la consulta
        }
        public async Task<T> ObtenerPrimero(Expression<Func<T, bool>> filtro, string incluirPropiedades, bool seguimientoEntidades)
        {
            IQueryable<T> query = dbSet;
            if (filtro != null)
            {
                query = query.Where(filtro);//aplica el filtro a la consulta select/* from table where filtro
            }
            if (incluirPropiedades != null)
            {
                //es una cadena de caracteres, la recorro caracter por caracter y la separo por comas y la convierto en un array de string y la recorro con un foreach y la incluyo en la consulta 
                foreach (var propiedad in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(propiedad);//incluye las propiedades de la entidad
                }
            }
            
            if (!seguimientoEntidades)
            {
                query = query.AsNoTracking();//no sigue las entidades
            }

            return await query.FirstOrDefaultAsync();//ejecuta la consulta
        }


        public void Remover(int id)
        {
            if (id != 0)
            {
                T entidad = dbSet.Find(id);//busca la entidad por el id
                dbSet.Remove(entidad);//llama al metodo remover
            }
            else
            {
                throw new ArgumentNullException("id", "El id enviado es 0");
            }
        }

        public void Remover(IEnumerable<T> entidad)
        {
            dbSet.RemoveRange(entidad);//remueve un rango de entidades
        }


        //Implementacion del metodo de paginado
        public PagedList<T> ObtenerTodosPaginado(Parametros parametros, Expression<Func<T, bool>> filtro = null, Func<IQueryable<T>, IOrderedQueryable<T>> ordenarPor = null, string incluirPropiedades = null, bool seguimientoEntidades = true)
        {
            IQueryable<T> query = dbSet;
            if (filtro != null)
            {
                query = query.Where(filtro);//aplica el filtro a la consulta select/* from table where filtro
            }
            if (incluirPropiedades != null)
            {
                //es una cadena de caracteres, la recorro caracter por caracter y la separo por comas y la convierto en un array de string y la recorro con un foreach y la incluyo en la consulta 
                foreach (var propiedad in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(propiedad);//incluye las propiedades de la entidad
                }
            }
            if (ordenarPor != null)
            {
                //ejecuta la consulta y la ordena por lo que se envia por parametro
                query = ordenarPor(query);//ordena la consulta
            }
            if (!seguimientoEntidades)
            {
                query = query.AsNoTracking();//no sigue las entidades
            }
            return PagedList<T>.ToPagedList(query, parametros.PageNumber, parametros.Pagesize);
        }
    }
}
