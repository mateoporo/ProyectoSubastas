using ProyectoSubastas.Controllers;
using System;
using System.Windows.Forms;

namespace ProyectoSubastas.Views.Postor
{
    public partial class CrearCuenta : Form
    {
        private readonly string tipoUsuario;

        public CrearCuenta(string tipoUsuario)
        {
            InitializeComponent();
            this.tipoUsuario = tipoUsuario;
            this.Text = $"Crear {tipoUsuario}";
            groupBox1.Text = $"Datos {tipoUsuario}";
            btnCrearUsuario.Text = $"Crear {tipoUsuario}";
        }

        private void btnCrearUsuario_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text.Trim();
            string mail = txtMail.Text.Trim();

            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(mail))
            {
                MessageBox.Show("Complete todos los campos.");
                return;
            }

            if (tipoUsuario == "Postor")
            {
                // Llamada al controlador Postor
                var controller = new PostorController();
                controller.CrearPostor(nombre, mail);

                MessageBox.Show("Postor creado correctamente");
            }
            else
            {
                // Llamada al controlador Subastador
                var controller = new SubastadorController();
                controller.CrearSubastador(nombre, mail);

                MessageBox.Show("Subastador creado correctamente");
            }

            this.Close();
        }
    }
}
