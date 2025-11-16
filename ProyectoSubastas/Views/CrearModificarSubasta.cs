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
        private readonly Subasta subastaActual;
        private readonly SubastaController subastaController;
        private readonly PanelUsuario panelPadre;
        public CrearModificarSubasta(Subasta subasta = null, PanelUsuario panelPadre = null)
        {
            InitializeComponent();
            this.panelPadre = panelPadre;
            subastaActual = subasta;
            subastaController = new SubastaController();
            ConfigurarFormulario();
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

            if (fechaInicio < DateTime.Now)
            {
                MessageBox.Show("La fecha y hora de inicio no puede ser anterior al momento actual.");
                return;
            }
            if (fechaFin <= fechaInicio)
            {
                MessageBox.Show("La fecha y hora de fin debe ser mayor a la fecha y hora de inicio.");
                return;
            }

            if (pujaInicial <= 0 || pujaAumento <= 0)
            {
                MessageBox.Show("Las pujas deben ser mayores a cero.");
                return;
            }

            if (subastaActual != null)
            {
                subastaController.ModificarSubasta(subastaActual.IdSubasta, articulo, pujaInicial, pujaAumento, fechaInicio, fechaFin, SesionUsuario.SubastadorActual.IdSubastador);
                MessageBox.Show("Subasta modificada correctamente");
            }
            else
            {
                subastaController.CrearSubasta(articulo, pujaInicial, pujaAumento, fechaInicio, fechaFin, SesionUsuario.SubastadorActual.IdSubastador);
                MessageBox.Show("Subasta creada correctamente");
            }
            panelPadre?.CargarGrillaSubastas();
            this.Hide();
        }


        private void ConfigurarFormulario()
        {
            if (subastaActual == null)
            {
                Text = "Crear Subasta";
                gpDatosSubasta.Text = "Crear Subasta";
                btnAccion.Text = "Crear Subasta";

                dateFechaInicio.MinDate = DateTime.Now;
                dateFechaFin.MinDate = dateFechaInicio.Value;

                dateFechaInicio.ValueChanged += (s, e) =>
                {
                    dateFechaFin.MinDate = dateFechaInicio.Value.AddSeconds(1); // evita que sea igual a inicio
                    if (dateFechaFin.Value <= dateFechaInicio.Value)
                    {
                        dateFechaFin.Value = dateFechaInicio.Value.AddDays(1);
                    }
                };

                return;
            }

            Text = "Modificar Subasta";
            gpDatosSubasta.Text = "Modificar Subasta";
            btnAccion.Text = "Modificar Subasta";

            txtArticulo.Text = subastaActual.Articulo;
            numPujaInicial.Value = subastaActual.PujaInicial;
            numPujaAumento.Value = subastaActual.PujaAumento;

            dateFechaInicio.Value = subastaActual.FechaInicio;
            dateFechaFin.Value = subastaActual.FechaFin;

            dateFechaInicio.MinDate = DateTime.Now <= subastaActual.FechaInicio
                ? DateTime.Now
                : subastaActual.FechaInicio;

            dateFechaFin.MinDate = subastaActual.FechaInicio.AddSeconds(1);

            dateFechaInicio.ValueChanged += (s, e) =>
            {
                dateFechaFin.MinDate = dateFechaInicio.Value.AddSeconds(1);
                if (dateFechaFin.Value <= dateFechaInicio.Value)
                {
                    dateFechaFin.Value = dateFechaInicio.Value.AddDays(1);
                }
            };
        }
    }
}
