using ProyectoSubastas.Controllers;
using ProyectoSubastas.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoSubastas.Views
{
    public partial class CrearModificarSubasta : Form
    {
        private readonly bool crearSubasta;
        private readonly SubastaController subastaController;
        public CrearModificarSubasta(bool crearSubasta = true)
        {
            InitializeComponent();
            this.crearSubasta = crearSubasta;
            subastaController = new SubastaController();
            if (!crearSubasta)
            {
                gpDatosSubasta.Text = "Modificar Subasta";
                btnAccion.Text = "Modificar Subasta";
                this.Text = "Modificar Subasta";
            }
            else
            {
                gpDatosSubasta.Text = "Crear Subasta";
                btnAccion.Text = "Crear Subasta";
                this.Text = "Crear Subasta";
            }
            dateFechaInicio.ValueChanged += (s, e) =>
            {
                dateFechaFin.MinDate = dateFechaInicio.Value;
            };
        }

        private void btnAccion_Click(object sender, EventArgs e)
        {
            string articulo = txtArticulo.Text.Trim();
            decimal pujaInicial = numPujaInicial.Value;
            decimal pujaAumento = numPujaAumento.Value;
            DateTime fechaInicio = dateFechaInicio.Value;
            DateTime fechaFin = dateFechaFin.Value;

            if (string.IsNullOrWhiteSpace(articulo))
            {
                MessageBox.Show("Debe ingresar un artículo.");
                return;
            }

            if (fechaInicio < DateTime.Today)
            {
                MessageBox.Show("La fecha de inicio no puede ser anterior al día de hoy.");
                return;
            }

            if (fechaFin <= fechaInicio)
            {
                MessageBox.Show("La fecha de fin debe ser mayor a la fecha de inicio.");
                return;
            }

            if (pujaInicial <= 0 || pujaAumento <= 0)
            {
                MessageBox.Show("Las pujas deben ser mayores a cero.");
                return;
            }

            if (!crearSubasta)
            {
                //MODIFICAR SUBASTA
            }
            else
            {
                subastaController.CrearSubasta(articulo, pujaInicial, pujaAumento, fechaInicio, fechaFin, SesionUsuario.SubastadorActual.IdSubastador);
                MessageBox.Show("Subasta creada correctamente");
            }
            this.Hide();
        }
    }
}
