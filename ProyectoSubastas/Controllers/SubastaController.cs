using ProyectoSubastas.Models;
using ProyectoSubastas.Repository;
using ProyectoSubastas.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSubastas.Controllers
{
    public class SubastaController
    {
        private readonly SubastaService service;

        public SubastaController()
        {
            service = new SubastaService();
        }

        public List<Subasta> ListarSubastas()
        {
            return service.ListarSubastas();
        }

        public bool CrearSubasta(string articulo, decimal pujaInicial, decimal pujaAumento, DateTime fechaInicio, DateTime fechaFin, int idSubastador)
        {
            Subasta s = new Subasta
            {
                Articulo = articulo,
                PujaInicial = pujaInicial,
                PujaAumento = pujaAumento,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                IdSubastador = idSubastador
            };

            return service.CrearSubasta(s);
        }

        public bool ModificarSubasta(int idSubasta, string articulo, decimal pujaInicial, decimal pujaAumento,DateTime fechaInicio, DateTime fechaFin, int idSubastador)
        {
            Subasta s = new Subasta
            {
                IdSubasta = idSubasta,
                Articulo = articulo,
                PujaInicial = pujaInicial,
                PujaAumento = pujaAumento,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                IdSubastador = idSubastador
            };

            return service.ModificarSubasta(s);
        }

        public bool EliminarSubasta(int id)
        {
            return service.EliminarSubasta(id);
        }

        public Subasta ObtenerSubasta(int id)
        {
            return service.ObtenerSubastaPorId(id);
        }

        public bool PuedeEliminar(int idSubasta)
        {
            var subasta = ObtenerSubasta(idSubasta);
            if (subasta == null)
                return false;

            return subasta.Participantes == null || subasta.Participantes.Count == 0;
        }

        public string ObtenerGanadorSubasta(int idSubasta)
        {
            var subasta = ObtenerSubasta(idSubasta);
            if (subasta == null)
                return "No se pudo encontrar la subasta.";

            var ultimaOferta = subasta.ObtenerUltimaOferta();
            if (ultimaOferta == null)
                return $"La subasta '{subasta.Articulo}' finalizó sin ofertas. No hay ganador.";

            string ganador = ultimaOferta.Postor.Nombre;
            decimal montoAPagar = ultimaOferta.Monto;
            decimal diferencial = montoAPagar - subasta.PujaInicial;

            return $"Subasta: {subasta.Articulo}\n" +
                   $"Ganador: {ganador}\n" +
                   $"Monto a pagar: ${montoAPagar}\n" +
                   $"Diferencial con puja inicial: ${diferencial}";
        }
    }
}
