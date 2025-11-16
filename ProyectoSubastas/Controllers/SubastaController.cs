using ProyectoSubastas.Models;
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

        // ------------------------------------------------------------
        // LISTAR SUBASTAS
        // ------------------------------------------------------------
        public List<Subasta> ListarSubastas()
        {
            return service.ListarSubastas();
        }

        // ------------------------------------------------------------
        // CREAR SUBASTA
        // ------------------------------------------------------------
        public bool CrearSubasta(
            string articulo,
            decimal pujaInicial,
            decimal pujaAumento,
            DateTime fechaInicio,
            DateTime fechaFin,
            int idSubastador
        )
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

        // ------------------------------------------------------------
        // MODIFICAR SUBASTA
        // ------------------------------------------------------------
        public bool ModificarSubasta(
            int idSubasta,
            string articulo,
            decimal pujaInicial,
            decimal pujaAumento,
            DateTime fechaInicio,
            DateTime fechaFin,
            int idSubastador
        )
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

        // ------------------------------------------------------------
        // ELIMINAR SUBASTA
        // ------------------------------------------------------------
        public bool EliminarSubasta(int id)
        {
            return service.EliminarSubasta(id);
        }

        // ------------------------------------------------------------
        // OBTENER POR ID
        // ------------------------------------------------------------
        public Subasta ObtenerSubasta(int id)
        {
            return service.ObtenerSubastaPorId(id);
        }
    }
}
