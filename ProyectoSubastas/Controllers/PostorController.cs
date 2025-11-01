using System;
using System.Collections.Generic;
using ProyectoSubastas.Models;
using ProyectoSubastas.Services;

namespace ProyectoSubastas.Controllers
{
    public class PostorController : IDisposable
    {
        private readonly PostorService _service;

        public PostorController(string dbPath = null)
        {
            _service = new PostorService(dbPath);
        }

        public Postor Crear(string nombre, string mail)
        {
            return _service.CrearPostor(nombre, mail);
        }

        public Postor Obtener(int id)
        {
            return _service.ObtenerPorId(id);
        }

        public List<Postor> Listar()
        {
            return _service.ObtenerTodos();
        }

        public void Actualizar(Postor p)
        {
            _service.ActualizarPostor(p);
        }

        public void Eliminar(int id)
        {
            _service.EliminarPostor(id);
        }

        public void Dispose()
        {
            _service?.Dispose();
        }
    }
}
