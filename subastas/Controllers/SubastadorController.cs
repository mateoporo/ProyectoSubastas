using System;
using System.Collections.Generic;
using ProyectoSubastas.Models;
using ProyectoSubastas.Services;

namespace ProyectoSubastas.Controllers
{
    public class SubastadorController : IDisposable
    {
        private readonly SubastadorService _service;

        public SubastadorController(string dbPath = "bd_subastas.db")
        {
            _service = new SubastadorService(dbPath);
        }

        public Subastador Crear(string nombre, string mail)
        {
            return _service.CrearSubastador(nombre, mail);
        }

        public Subastador Obtener(int id)
        {
            return _service.ObtenerPorId(id);
        }

        public List<Subastador> Listar()
        {
            return _service.ObtenerTodos();
        }

        public void Actualizar(Subastador s)
        {
            _service.ActualizarSubastador(s);
        }

        public void Eliminar(int id)
        {
            _service.EliminarSubastador(id);
        }

        public void Dispose()
        {
            _service?.Dispose();
        }
    }
}