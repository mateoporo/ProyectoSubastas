using ProyectoSubastas.Models;
using ProyectoSubastas.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSubastas.Services
{
    public class SubastaService
    {
        private readonly SubastaRepository repository;

        public SubastaService()
        {
            repository = new SubastaRepository();
        }

        public List<Subasta> ListarSubastas()
        {
            return repository.ObtenerTodas();
        }

        public Subasta ObtenerSubastaPorId(int id)
        {
            return repository.ObtenerPorId(id);
        }

        public bool CrearSubasta(Subasta subasta)
        {
            var errores = subasta.Validate();
            if (errores.Count > 0)
            {
                return false;
            }

            // validar FECHAS
            if (subasta.FechaInicio >= subasta.FechaFin)
            {
                return false; // la subasta debe terminar después de iniciar
            }

            // validar valores positivos
            if (subasta.PujaInicial <= 0 || subasta.PujaAumento <= 0)
            {
                return false;
            }

            repository.Crear(subasta);
            return true;
        }

        public bool ModificarSubasta(Subasta subasta)
        {
            Subasta existente = repository.ObtenerPorId(subasta.IdSubasta);
            if (existente == null)
            {
                return false;
            }

            var errores = subasta.Validate();
            if (errores.Count > 0)
            {
                return false;
            }

            if (subasta.FechaInicio >= subasta.FechaFin)
            {
                return false;
            }

            repository.Modificar(subasta);
            return true;
        }

        public bool EliminarSubasta(int id)
        {
            Subasta existente = repository.ObtenerPorId(id);
            if (existente == null)
            {
                return false;
            }

            repository.Eliminar(id);
            return true;
        }
    }
}
