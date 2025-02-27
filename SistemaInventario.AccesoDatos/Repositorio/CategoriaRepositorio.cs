﻿using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio
{//es la implementacion de Irepositorio
    public class CategoriaRepositorio : Repositorio<Categoria>, ICategoriaRepositorio
    {
        //el padre me pide trabajar con un repositorio por lo que en la clase le tenemos que pasar la entidad que va a trabajar
        private readonly ApplicationDbContext _db;

        public CategoriaRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Actualizar(Categoria categoria)
        {
            //captura por medio del dbcontext el registro por medio del id
            var categoriaBD = _db.Categorias.FirstOrDefault(s => s.Id == categoria.Id);
            if (categoriaBD != null)
            {
                //actualiza los campos de la entidad
                categoriaBD.Nombre = categoria.Nombre;
                categoriaBD.Descripcion = categoria.Descripcion;
                categoriaBD.Estado = categoria.Estado;
                _db.SaveChanges();
            }
        }
    }
}
