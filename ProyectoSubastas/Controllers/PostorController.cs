using ProyectoSubastas.Models;
using ProyectoSubastas.Services;
using System.Collections.Generic;

namespace ProyectoSubastas.Controllers
{
    public class PostorController
    {
        private readonly PostorService service;

        public PostorController()
        {
            service = new PostorService();
        }

        public List<Postor> ListarPostores()
        {
            return service.ListarPostores();
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
    }
}