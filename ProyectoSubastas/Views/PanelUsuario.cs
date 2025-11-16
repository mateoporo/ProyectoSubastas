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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProyectoSubastas.Views
{
    public partial class PanelUsuario : Form
    {
        private readonly string tipoUsuario;
        private readonly PostorController postorController;
        private readonly SubastadorController subastadorController;
        private readonly SubastaController subastaController;

        public PanelUsuario(string tipoUsuario)
        {
            InitializeComponent();
            postorController = new PostorController();
            subastadorController = new SubastadorController();
            subastaController = new SubastaController();
            this.tipoUsuario = tipoUsuario;
            gpDatosUsuario.Text = $"Gestionar cuenta {tipoUsuario}";
            CargarDatosUsuario();
            CargarGrillaSubastas();
        }

        private void CargarDatosUsuario()
        {
            // Ruta base del proyecto (bin/Debug → se retrocede a raiz de proyecto)
            string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\Images\\");
            if (tipoUsuario == "Postor" && SesionUsuario.PostorActual != null)
            {
                txtNombre.Text = SesionUsuario.PostorActual.Nombre;
                txtMail.Text = SesionUsuario.PostorActual.Mail;
                //btnPujar.Visible = true;
                //btnEgresoSubasta.Visible = true;
                picTipoUsuario.Image = Image.FromFile(Path.Combine(basePath, "postor.png"));
                //btnModificarSubasta.Visible = false;
                //btnEliminarSubasta.Visible = false;
                //btnCrearSubasta.Visible = false;
            }
            else if (tipoUsuario == "Subastador" && SesionUsuario.SubastadorActual != null)
            {
                txtNombre.Text = SesionUsuario.SubastadorActual.Nombre;
                txtMail.Text = SesionUsuario.SubastadorActual.Mail;
                //btnCrearSubasta.Visible = true;
                //btnModificarSubasta.Visible = true;
                //btnEliminarSubasta.Visible = true;
                picTipoUsuario.Image = Image.FromFile(Path.Combine(basePath, "subastador.png"));
            }
        }

        private void btnModificarUsuario_Click(object sender, EventArgs e)
        {
            txtNombre.Enabled = true;
            txtMail.Enabled = true;
            btnGuardarUsuarioModificado.Enabled = true;
            btnModificarUsuario.Enabled = false;
            btnEliminarUsuario.Enabled = false;
            gpSubastas.Enabled = false;
        }

        private void btnGuardarUsuarioModificado_Click(object sender, EventArgs e)
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
                postorController.ModificarPostor(SesionUsuario.PostorActual.IdPostor, nombre, mail);
                MessageBox.Show("Postor modificado correctamente");
            }
            else
            {
                subastadorController.ModificarSubastador(SesionUsuario.SubastadorActual.IdSubastador, nombre, mail);
                MessageBox.Show("Subastador modificado correctamente");
            }
            txtNombre.Enabled = false;
            txtMail.Enabled = false;
            btnGuardarUsuarioModificado.Enabled = false;
            btnModificarUsuario.Enabled = true;
            btnEliminarUsuario.Enabled = true;
            gpSubastas.Enabled = true;
        }

        private void btnEliminarUsuario_Click(object sender, EventArgs e)
        {
            string aclaracion;
            if (tipoUsuario == "Postor")
            {
                aclaracion = "Al eliminar su cuenta, será dado de baja de cualquier subasta en la que participe.";
            }
            else
            {
                aclaracion = "Al eliminar su cuenta, todas las subastas que haya creado serán eliminadas.";
            }
            var respuesta = MessageBox.Show("¿Está seguro de eliminar su cuenta como " + tipoUsuario + "?\n\n" + aclaracion, "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (respuesta == DialogResult.Yes)
            {
                if (tipoUsuario == "Postor")
                {
                    postorController.EliminarPostor(SesionUsuario.PostorActual.IdPostor);
                    // queda por hacer, dar de baja al postor a eliminar en las subastas que se encuentre participando
                }
                else
                {
                    subastadorController.EliminarSubastador(SesionUsuario.SubastadorActual.IdSubastador);
                    // queda por hacer, eliminar todas las subastas que el subastador a eliminar haya creado
                }
                MessageBox.Show("Usuario eliminado correctamente. Se redirigirá automaticamente al login de la aplicación.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Login loginForm = new Login();
                loginForm.Show();
                this.Hide();
            }
        }

        private void PanelUsuario_FormClosing(object sender, FormClosingEventArgs e)
        {
            var respuesta = MessageBox.Show("¿Desea cerrar el panel de usuario? Si continúa se redirigirá automaticamente al login de la aplicación.", "Confirmar salida", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (respuesta == DialogResult.No)
            {
                e.Cancel = true; // cancela el cierre
                return;
            }
            Login loginForm = new Login();
            loginForm.Show();
            this.Hide();
        }

        private void btnCrearSubasta_Click(object sender, EventArgs e)
        {
            CrearModificarSubasta crearSubastaForm = new CrearModificarSubasta(null, this);
            crearSubastaForm.ShowDialog();
        }

        private void btnModificarSubasta_Click(object sender, EventArgs e)
        {
            if (dgvSubastas.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione una subasta para modificar.");
                return;
            }
            int id = Convert.ToInt32(dgvSubastas.SelectedRows[0].Cells["colId"].Value);
            if (!VerificarSubastaActiva(id))
                return;
            Subasta sub = subastaController.ObtenerSubasta(id);
            CrearModificarSubasta modificarSubastaForm = new CrearModificarSubasta(sub, this);
            modificarSubastaForm.ShowDialog();
        }

        public void CargarGrillaSubastas()
        {
            try
            {
                List<Subasta> lista = subastaController.ListarSubastas();

                dgvSubastas.Rows.Clear();

                var col = (DataGridViewComboBoxColumn)dgvSubastas.Columns["colParticipantes"];
                col.Items.Clear();

                // Obtener todos los nombres de participantes de todas las subastas y aplicar Distinct
                var todosNombres = lista
                    .SelectMany(s => s.Participantes)
                    .Select(p => p.Nombre)
                    .ToList();

                if (todosNombres.Count == 0)
                    col.Items.Add("Sin participantes");
                else
                {
                    foreach (var nombre in todosNombres)
                        col.Items.Add(nombre);
                }

                foreach (var s in lista)
                {
                    var ultimaOferta = s.ObtenerUltimaOferta();
                    string valorUltimaOferta = ultimaOferta != null
                        ? $"{ultimaOferta.Monto} por el postor '{ultimaOferta.Postor.Nombre}'"
                        : "Sin ofertas";

                    int rowIndex = dgvSubastas.Rows.Add(
                        s.IdSubasta,
                        s.Articulo,
                        valorUltimaOferta,
                        s.PujaInicial,
                        s.PujaAumento,
                        s.FechaFin.ToString("dd/MM/yyyy HH:mm"),
                        s.Subastador.Nombre,
                        s.Activa ? "ACTIVA" : "FINALIZADA",
                        s.Subastador.IdSubastador
                    );

                    var cell = (DataGridViewComboBoxCell)dgvSubastas.Rows[rowIndex].Cells["colParticipantes"];
                    cell.Items.Clear();

                    if (s.Participantes.Any())
                    {
                        foreach (var p in s.Participantes)
                            cell.Items.Add(p.Nombre);
                        cell.Value = s.Participantes.First().Nombre;
                    }
                    else
                    {
                        cell.Items.Add("Sin participantes");
                        cell.Value = "Sin participantes";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar subastas: " + ex.Message);
            }
        }

        private void dgvSubastas_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSubastas.SelectedRows.Count == 0)
            {
                btnModificarSubasta.Visible = false;
                btnEliminarSubasta.Visible = false;
                btnPujar.Visible = false;
                btnEgresoSubasta.Visible = false;
                btnVerGanador.Visible = false;
                return;
            }

            var fila = dgvSubastas.SelectedRows[0];
            string estadoSubasta = Convert.ToString(fila.Cells["colEstado"].Value); // ACTIVA / FINALIZADA

            if (SesionUsuario.SubastadorActual != null)
            {
                btnCrearSubasta.Visible = true;
                btnModificarSubasta.Visible = true;
                btnEliminarSubasta.Visible = true;
                int idSubastadorFila = Convert.ToInt32(fila.Cells["colIdSubastador"].Value);
                bool esMismoSubastador = (idSubastadorFila == SesionUsuario.SubastadorActual.IdSubastador);

                if (estadoSubasta == "FINALIZADA")
                {
                    btnModificarSubasta.Enabled = false;
                    btnEliminarSubasta.Enabled = false;
                    btnVerGanador.Visible = true;
                }
                else
                {
                    btnModificarSubasta.Enabled = esMismoSubastador;
                    btnEliminarSubasta.Enabled = esMismoSubastador;
                    btnVerGanador.Visible = false;
                }

                btnPujar.Visible = false;
                btnEgresoSubasta.Visible = false;
            }
            else
            {
                btnPujar.Visible = true;
                btnEgresoSubasta.Visible = true;
                if (estadoSubasta == "FINALIZADA")
                {
                    btnPujar.Enabled = false;
                    btnEgresoSubasta.Enabled = false;
                    btnVerGanador.Visible = true;
                }
                else
                {
                    btnPujar.Enabled = true;
                    btnEgresoSubasta.Enabled = true;
                    btnVerGanador.Visible = false;
                }

                btnModificarSubasta.Visible = false;
                btnEliminarSubasta.Visible = false;
            }
        }

        private void btnEliminarSubasta_Click(object sender, EventArgs e)
        {
            if (dgvSubastas.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione una subasta para eliminar.");
                return;
            }

            int idSubasta = Convert.ToInt32(dgvSubastas.SelectedRows[0].Cells["colId"].Value);

            if (!VerificarSubastaActiva(idSubasta))
                return;

            if (!subastaController.PuedeEliminar(idSubasta))
            {
                MessageBox.Show("No se puede eliminar la subasta porque hay participantes activos.");
                return;
            }

            var confirm = MessageBox.Show("¿Está seguro de eliminar esta subasta?",
                                          "Confirmar eliminación",
                                          MessageBoxButtons.YesNo,
                                          MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                if (subastaController.EliminarSubasta(idSubasta))
                {
                    MessageBox.Show("Subasta eliminada correctamente.");
                    CargarGrillaSubastas();
                }
                else
                {
                    MessageBox.Show("No se pudo eliminar la subasta.");
                }
            }
        }

        private void btnPujar_Click(object sender, EventArgs e)
        {
            if (dgvSubastas.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione una subasta para pujar.");
                return;
            }

            int idSubasta = Convert.ToInt32(dgvSubastas.SelectedRows[0].Cells["colId"].Value);
            if (!VerificarSubastaActiva(idSubasta))
                return;

            decimal montoPuja;

            bool exito = postorController.Pujar(idSubasta, SesionUsuario.PostorActual.IdPostor, out montoPuja);

            if (exito)
            {
                MessageBox.Show($"Oferta realizada correctamente. Monto: {montoPuja:C}");
                CargarGrillaSubastas();
            }
            else
            {
                MessageBox.Show("No se pudo realizar la oferta. La subasta podría no estar activa.");
            }
        }

        private void btnVerGanador_Click(object sender, EventArgs e)
        {
            if (dgvSubastas.SelectedRows.Count == 0)
                return;

            int idSubasta = Convert.ToInt32(dgvSubastas.SelectedRows[0].Cells["colId"].Value);
            string resultado = subastaController.ObtenerGanadorSubasta(idSubasta);

            MessageBox.Show(resultado, "Resultado de la subasta", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool VerificarSubastaActiva(int idSubasta)
        {
            var subasta = subastaController.ObtenerSubasta(idSubasta);
            if (!subasta.Activa)
            {
                MessageBox.Show($"La subasta '{subasta.Articulo}' ya finalizó.");
                CargarGrillaSubastas();
                return false;
            }
            return true;
        }
    }
}
