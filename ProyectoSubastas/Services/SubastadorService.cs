using System;
using System.Collections.Generic;
using ProyectoSubastas.Models;
using ProyectoSubastas.Repository;

namespace ProyectoSubastas.Services
{
    public class SubastadorService : IDisposable
    {
        private readonly SubastadorRepository _repo;

        public SubastadorService(string dbPath = "bd_subastas.db")
        {
            _repo = new SubastadorRepository(dbPath);
        }

        public Subastador CrearSubastador(string nombre, string mail)
        {
            var model = new Subastador(0, nombre, mail);
            var errors = model.Validate();
            if (errors.Count > 0)
                throw new ArgumentException(string.Join("; ", GetErrorMessages(errors)));

            return _repo.Create(model);
        }

        public Subastador ObtenerPorId(int id)
        {
            return _repo.GetById(id);
        }

        public List<Subastador> ObtenerTodos()
        {
            return _repo.GetAll();
        }

        public void ActualizarSubastador(Subastador actualizado)
        {
            var errors = actualizado.Validate();
            if (errors.Count > 0)
                throw new ArgumentException(string.Join("; ", GetErrorMessages(errors)));

            var ok = _repo.Update(actualizado);
            if (!ok)
                throw new InvalidOperationException("No se pudo actualizar el subastador (id no encontrado).");
        }

        public void EliminarSubastador(int id)
        {
            var ok = _repo.Delete(id);
            if (!ok)
                throw new InvalidOperationException("No se pudo eliminar el subastador (id no encontrado).");
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