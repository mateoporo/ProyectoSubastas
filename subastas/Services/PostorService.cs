using System;
using System.Collections.Generic;
using ProyectoSubastas.Models;
using ProyectoSubastas.Repository;

namespace ProyectoSubastas.Services
{
    public class PostorService : IDisposable
    {
        private readonly PostorRepository _repo;

        public PostorService(string dbPath = null)
        {
            _repo = new PostorRepository(dbPath);
        }

        public Postor CrearPostor(string nombre, string mail)
        {
            var model = new Postor(0, nombre, mail);
            var errors = model.Validate();
            if (errors.Count > 0)
                throw new ArgumentException(string.Join("; ", GetErrorMessages(errors)));

            return _repo.Create(model);
        }

        public Postor ObtenerPorId(int id)
        {
            return _repo.GetById(id);
        }

        public List<Postor> ObtenerTodos()
        {
            return _repo.GetAll();
        }

        public void ActualizarPostor(Postor actualizado)
        {
            var errors = actualizado.Validate();
            if (errors.Count > 0)
                throw new ArgumentException(string.Join("; ", GetErrorMessages(errors)));

            var ok = _repo.Update(actualizado);
            if (!ok)
                throw new InvalidOperationException("No se pudo actualizar el postor (id no encontrado).");
        }

        public void EliminarPostor(int id)
        {
            var ok = _repo.Delete(id);
            if (!ok)
                throw new InvalidOperationException("No se pudo eliminar el postor (id no encontrado).");
        }

        private IEnumerable<string> GetErrorMessages(IList<System.ComponentModel.DataAnnotations.ValidationResult> errs)
        {
            foreach (var e in errs) yield return e.ErrorMessage;
        }

        public void Dispose()
        {
            _repo?.Dispose();
        }
    }
}
