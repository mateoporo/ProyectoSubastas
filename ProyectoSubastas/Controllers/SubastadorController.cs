using ProyectoSubastas.Models;
using ProyectoSubastas.Services;
using System.Collections.Generic;

namespace ProyectoSubastas.Controllers
{
    public class SubastadorController
    {
        private readonly SubastadorService service;
        private readonly SubastaService subastaService;

        public SubastadorController()
        {
            service = new SubastadorService();
            subastaService = new SubastaService();
        }

        public bool CrearSubastador(string nombre, string mail)
        {
            Subastador s = new Subastador
            {
                Nombre = nombre,
                Mail = mail
            };

            return service.CrearSubastador(s);
        }

        public bool ModificarSubastador(int id, string nombre, string mail)
        {
            Subastador s = new Subastador
            {
                IdSubastador = id,
                Nombre = nombre,
                Mail = mail
            };

            return service.ModificarSubastador(s);
        }

        public bool EliminarSubastador(int id)
        {
            return service.EliminarSubastador(id);
        }

        public Subastador ObtenerPorMail(string mail)
        {
            return service.ObtenerSubastadorPorMail(mail);
        }

        public bool SePuedeEliminar(int idSubastador, out string mensaje)
        {
            mensaje = "";
            var subastas = subastaService.ListarSubastas();
            bool tieneSubastas = subastas.Any(s => s.IdSubastador == idSubastador);
            if (tieneSubastas)
            {
                mensaje = "El subastador tiene subastas registradas y no puede ser eliminado.";
                return false;
            }
            return true;
        }
    }
}