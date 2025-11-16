using ProyectoSubastas.Models;
using ProyectoSubastas.Services;
using System.Collections.Generic;

namespace ProyectoSubastas.Controllers
{
    public class PostorController
    {
        private readonly PostorService service;
        private readonly SubastaService subastaService;
        private readonly OfertaController ofertaController;

        public PostorController()
        {
            service = new PostorService();
            subastaService = new SubastaService();
            ofertaController = new OfertaController();
        }

        public bool CrearPostor(string nombre, string mail)
        {
            Postor p = new Postor
            {
                Nombre = nombre,
                Mail = mail
            };

            return service.CrearPostor(p);
        }

        public bool ModificarPostor(int id, string nombre, string mail)
        {
            Postor p = new Postor
            {
                IdPostor = id,
                Nombre = nombre,
                Mail = mail
            };

            return service.ModificarPostor(p);
        }

        public bool EliminarPostor(int id)
        {
            return service.EliminarPostor(id);
        }

        public Postor ObtenerPorMail(string mail)
        {
            return service.ObtenerPostorPorMail(mail);
        }

        public bool Pujar(int idSubasta, int idPostor, out decimal montoPuja)
        {
            montoPuja = 0;
            var subasta = subastaService.ObtenerSubastaPorId(idSubasta);
            if (subasta == null || !subasta.Activa)
                return false;

            var ultimaOferta = ofertaController.ObtenerUltimaOferta(idSubasta);

            if (ultimaOferta != null)
            {
                montoPuja = ultimaOferta.Monto + subasta.PujaAumento;
            }
            else
            {
                montoPuja = subasta.PujaAumento;
            }

            bool exito = ofertaController.CrearOferta(idSubasta, idPostor, montoPuja, subasta);

            return exito;
        }

        public bool SePuedeEliminar(int idPostor, out string mensaje)
        {
            mensaje = "";
            var subastas = subastaService.ListarSubastas();
            bool tieneOfertas = subastas.Any(s => s.Participantes.Any(p => p.IdPostor == idPostor));
            if (tieneOfertas)
            {
                mensaje = "El postor tiene ofertas registradas en subastas y no puede ser eliminado.";
                return false;
            }
            return true;
        }
    }
}