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
            gpDatosUsuario.Text = $"Gestionar cuenta {tipoUsuario}";
            CargarDatosUsuario();
        }

        private void CargarDatosUsuario()
        {
            // Ruta base del proyecto (bin/Debug → se retrocede a raiz de proyecto)
            string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\Images\\");
            if (tipoUsuario == "Postor" && SesionUsuario.PostorActual != null)
            {
                txtNombre.Text = SesionUsuario.PostorActual.Nombre;
                txtMail.Text = SesionUsuario.PostorActual.Mail;
                btnPujar.Enabled = true;
                btnEgresoSubasta.Enabled = true;
                picTipoUsuario.Image = Image.FromFile(Path.Combine(basePath, "postor.png"));
            }
            else if (tipoUsuario == "Subastador" && SesionUsuario.SubastadorActual != null)
            {
                txtNombre.Text = SesionUsuario.SubastadorActual.Nombre;
                txtMail.Text = SesionUsuario.SubastadorActual.Mail;
                btnCrearSubasta.Enabled = true;
                btnModificarSubasta.Enabled = true;
                btnEliminarSubasta.Enabled = true;
                picTipoUsuario.Image = Image.FromFile(Path.Combine(basePath, "subastador.png"));
            }
        }
    }
}
