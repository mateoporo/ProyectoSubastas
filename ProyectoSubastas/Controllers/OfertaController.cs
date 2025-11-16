using ProyectoSubastas.Models;
using ProyectoSubastas.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSubastas.Controllers
{
    public class OfertaController
    {
        private readonly OfertaService service;

        public OfertaController()
        {
            service = new OfertaService();
        }

        public List<Oferta> ListarPorSubasta(int idSubasta)
        {
            return service.ListarOfertasPorSubasta(idSubasta);
        }

        public Oferta ObtenerUltimaOferta(int idSubasta)
        {
            return service.ObtenerUltimaOferta(idSubasta);
        }

        public bool CrearOferta(int idSubasta, int idPostor, decimal monto, Subasta subasta)
        {
            Oferta o = new Oferta
            {
                IdSubasta = idSubasta,
                IdPostor = idPostor,
                Monto = monto,
                FechaOferta = DateTime.Now
            };

            return service.CrearOferta(o, subasta);
        }

        public bool EliminarOferta(int idOferta)
        {
            return service.EliminarOferta(idOferta);
        }
    }
}
