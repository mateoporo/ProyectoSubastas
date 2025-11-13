using ProyectoSubastas.Models;
using ProyectoSubastas.Repository;
using System.Collections.Generic;

namespace ProyectoSubastas.Services
{
    public class SubastadorService
    {
        private readonly SubastadorRepository repository;

        public SubastadorService()
        {
            repository = new SubastadorRepository();
        }

        public List<Subastador> ListarSubastadores()
        {
            return repository.ObtenerTodos();
        }

        public bool CrearSubastador(Subastador subastador)
        {
            Subastador existente = repository.ObtenerPorMail(subastador.Mail);
            if (existente != null)
            {
                return false; // Ya existe un subastador con ese mail
            }

            repository.Crear(subastador);
            return true;
        }

        public bool ModificarSubastador(Subastador subastador)
        {
            Subastador existente = repository.ObtenerPorId(subastador.IdSubastador);
            if (existente == null)
            {
                return false; // No existe el subastador a modificar
            }

            repository.Modificar(subastador);
            return true;
        }

        public bool EliminarSubastador(int id)
        {
            Subastador existente = repository.ObtenerPorId(id);
            if (existente == null)
            {
                return false;
            }

            repository.Eliminar(id);
            return true;
        }

        public Subastador ObtenerSubastadorPorId(int id)
        {
            return repository.ObtenerPorId(id);
        }
    }
}
