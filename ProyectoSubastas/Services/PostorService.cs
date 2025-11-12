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

        public List<Postor> ListarPostores()
        {
            return repository.ObtenerTodos();
        }

        public bool CrearPostor(Postor postor)
        {
            Postor existente = repository.GetByMail(postor.Mail);
            if (existente != null)
            {
                return false; // Ya existe un postor con ese mail
            }

            repository.Crear(postor);
            return true;
        }

        public bool ModificarPostor(Postor postor)
        {
            Postor existente = repository.GetById(postor.IdPostor);
            if (existente == null)
            {
                return false; // No existe el postor a modificar
            }

            repository.Modificar(postor);
            return true;
        }

        public bool EliminarPostor(int id)
        {
            Postor existente = repository.GetById(id);
            if (existente == null)
            {
                return false;
            }

            repository.Eliminar(id);
            return true;
        }
    }
}