﻿using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio
{
    public class UsuarioAplicacionRepositorio : Repositorio<UsuarioAplicacion>, IUsuarioAplicacionRepositorio
    {
        //el padre me pide trabajar con un repositorio por lo que en la clase le tenemos que pasar la entidad que va a trabajar
        private readonly ApplicationDbContext _db;

        public UsuarioAplicacionRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        
    }
}
