using ProyectoSubastas.Models;
using ProyectoSubastas.Services;
using System.Collections.Generic;

namespace ProyectoSubastas.Controllers
{
    public class SubastadorController
    {
        private readonly SubastadorService service;

        public SubastadorController()
        {
            service = new SubastadorService();
        }

        public List<Subastador> ListarSubastadores()
        {
            return service.ListarSubastadores();
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

        public Subastador ObtenerSubastadorPorId(int id)
        {
            return service.ObtenerSubastadorPorId(id);
        }

        public Subastador ObtenerPorMail(string mail)
        {
            return service.ObtenerSubastadorPorMail(mail);
        }
    }
}