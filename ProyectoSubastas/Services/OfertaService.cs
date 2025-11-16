using ProyectoSubastas.Models;
using ProyectoSubastas.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSubastas.Services
{
    public class OfertaService
    {
        private readonly OfertaRepository repository;

        public OfertaService()
        {
            repository = new OfertaRepository();
        }

        public List<Oferta> ListarOfertasPorSubasta(int idSubasta)
        {
            return repository.ObtenerPorSubasta(idSubasta);
        }

        public Oferta ObtenerUltimaOferta(int idSubasta)
        {
            return repository.ObtenerUltimaOferta(idSubasta);
        }

        public bool CrearOferta(Oferta oferta, Subasta subasta)
        {
            if (subasta == null)
                return false;
            if (!subasta.Activa)
                return false;
            var ultima = ObtenerUltimaOferta(subasta.IdSubasta);
            decimal minimoEsperado = ultima != null ? ultima.Monto + subasta.PujaAumento : subasta.PujaInicial;
            if (oferta.Monto < minimoEsperado)
                return false;
            repository.Crear(oferta);
            return true;
        }

        public bool EliminarOferta(int id)
        {
            Oferta existente = repository.ObtenerPorId(id);
            if (existente == null)
            {
                return false;
            }
            repository.Eliminar(id);
            return true;
        }
    }
}
