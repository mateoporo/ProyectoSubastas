using ProyectoSubastas.Views.Postor;
using System;
using System.Windows.Forms;

namespace ProyectoSubastas.Views
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
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
    }
}
