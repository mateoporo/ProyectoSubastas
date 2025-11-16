using ProyectoSubastas.Models;
using ProyectoSubastas.Repository;
using System.Collections.Generic;

namespace ProyectoSubastas.Services
{
    public class PostorService
    {
        private readonly PostorRepository repository;

        public PostorService()
        {
            repository = new PostorRepository();
        }

        public bool CrearPostor(Postor postor)
        {
            Postor existente = repository.ObtenerPorMail(postor.Mail);
            if (existente != null)
            {
                return false;
            }

            repository.Crear(postor);
            return true;
        }

        public bool ModificarPostor(Postor postor)
        {
            Postor existente = repository.ObtenerPorId(postor.IdPostor);
            if (existente == null)
            {
                return false;
            }

            repository.Modificar(postor);
            return true;
        }

        public bool EliminarPostor(int id)
        {
            Postor existente = repository.ObtenerPorId(id);
            if (existente == null)
            {
                return false;
            }

            repository.Eliminar(id);
            return true;
        }

        public Postor ObtenerPostorPorMail(string mail)
        {
            return repository.ObtenerPorMail(mail);
        }
    }
}