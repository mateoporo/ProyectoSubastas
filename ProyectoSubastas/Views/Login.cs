using ProyectoSubastas.Controllers;
using ProyectoSubastas.Models;
using ProyectoSubastas.Views.Postor;
using System;
using System.Windows.Forms;

namespace ProyectoSubastas.Views
{
    public partial class Login : Form
    {
        private readonly PostorController postorController;
        private readonly SubastadorController subastadorController;

        public Login()
        {
            InitializeComponent();
            postorController = new PostorController();
            subastadorController = new SubastadorController();
        }

        private void btnCrearPostor_Click(object sender, EventArgs e)
        {
            var form = new CrearCuenta("Postor");
            form.ShowDialog();
        }

        private void btnCrearSubastador_Click(object sender, EventArgs e)
        {
            CrearCuenta form = new CrearCuenta("Subastador");
            form.ShowDialog();
        }

        private void btnIniciarSesion_Click(object sender, EventArgs e)
        {
            string mail = txtMail.Text.Trim();
            string tipo = cbTipoUsuario.SelectedItem?.ToString();

            if (string.IsNullOrWhiteSpace(mail))
            {
                MessageBox.Show("Ingrese el mail.");
                return;
            }

            SesionUsuario.Clear();

            if (tipo == "Postor")
            {
                var postor = postorController.ObtenerPorMail(mail);
                if (postor == null)
                {
                    MessageBox.Show("No existe un Postor con este mail.");
                    return;
                }

                SesionUsuario.PostorActual = postor;
                MessageBox.Show("Postor logueado correctamente.");

                PanelUsuario panel = new PanelUsuario("Postor");
                panel.Show();
                this.Hide();
            }
            else if (tipo == "Subastador")
            {
                var subastador = subastadorController.ObtenerPorMail(mail);
                if (subastador == null)
                {
                    MessageBox.Show("No existe un Subastador con este mail.");
                    return;
                }

                SesionUsuario.SubastadorActual = subastador;
                MessageBox.Show("Subastador logueado correctamente.");

                PanelUsuario panel = new PanelUsuario("Subastador");
                panel.Show();
                this.Hide();
            }
        }
    }
}
