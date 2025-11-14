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
    public partial class PanelUsuario : Form
    {
        private readonly string tipoUsuario;
        public PanelUsuario(string tipoUsuario)
        {
            InitializeComponent();
            this.tipoUsuario = tipoUsuario;
            gpDatosUsuario.Text = tipoUsuario;
            CargarDatosUsuario();
        }

        private void CargarDatosUsuario()
        {
            if (tipoUsuario == "Postor" && SesionUsuario.PostorActual != null)
            {
                txtNombre.Text = SesionUsuario.PostorActual.Nombre;
                txtMail.Text = SesionUsuario.PostorActual.Mail;
                btnPujar.Enabled = true;
                btnEgresoSubasta.Enabled = true;
            }
            else if (tipoUsuario == "Subastador" && SesionUsuario.SubastadorActual != null)
            {
                txtNombre.Text = SesionUsuario.SubastadorActual.Nombre;
                txtMail.Text = SesionUsuario.SubastadorActual.Mail;
                btnCrearSubasta.Enabled = true;
                btnModificarSubasta.Enabled = true;
                btnEliminarSubasta.Enabled = true;
            }
        }
    }
}
