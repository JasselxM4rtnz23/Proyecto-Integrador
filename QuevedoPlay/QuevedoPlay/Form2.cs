using ConsoleApp4.Forms;
using QuevedoPlay.Estilos;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;


namespace QuevedoPlay
{
    public partial class FrmOpciones : Form
    {
        public FrmOpciones()
        {
            InitializeComponent();
            // No llamar a AplicarTema en el constructor: evita ejecutar lógica de UI
            // que depende de tamaños o recursos durante el tiempo de diseño.
        }


        private void button4_Click(object sender, EventArgs e)
        {
            OpcionesTurismoSeguro seguridad = new OpcionesTurismoSeguro();
            seguridad.Show();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Orientacion orientacion = new Orientacion(this);
            orientacion.Show();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FrmActividadesTuristicas form3 = new FrmActividadesTuristicas();
            form3.Show();
            this.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // Aplicar tema en Load (controles ya tienen tamaño y entorno de diseño no ejecutará lógica dañina)
            AplicarTema();
            Redondear(panelOrientacion, 25);
            Redondear(panelActividades, 25);
            Redondear(panelSeguridad, 25);
            Redondear(btnCerrarSesion, 18);

            Redondear(BtnOrientaciónTurística, 18);
            Redondear(btnActividadesTuristicas, 18);
            Redondear(btnTurismoSeguro, 18);

            //Boton x para cerrar la app
            Redondear(button1, 10);
        }
        private void Redondear(Control control, int radio)
        {
            // Proteger contra ejecución en tiempo de diseño o cuando el control
            // no tenga dimensiones válidas (esto evita excepciones en el diseñador).
            if (control == null)
                return;

            // Si el control aún no tiene tamaño o es muy pequeño, salir
            if (control.Width <= radio || control.Height <= radio)
                return;

            try
            {
                GraphicsPath path = new GraphicsPath();
                path.StartFigure();
                path.AddArc(0, 0, radio, radio, 180, 90);
                path.AddArc(control.Width - radio, 0, radio, radio, 270, 90);
                path.AddArc(control.Width - radio, control.Height - radio, radio, radio, 0, 90);
                path.AddArc(0, control.Height - radio, radio, radio, 90, 90);
                path.CloseFigure();

                control.Region = new Region(path);
            }
            catch
            {
                // Silenciar cualquier error para no romper el diseñador.
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var r = MessageBox.Show(
        "¿Cerrar sesión?",
        "Confirmar",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question
    );

            if (r == DialogResult.Yes)
            {
                // volver al login
                var login = new Form1();
                login.Show();

                this.Close(); // cierra Form2 (no toda la app)
            }
        }
        private void AplicarTema()
        {
            // Evitar aplicar el tema durante el tiempo de diseño.
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
                return;

            this.BackColor = TemaApp.FondoPrincipal;

            panelOrientacion.BackColor = TemaApp.FondoPanel;
            panelActividades.BackColor = TemaApp.FondoPanel;
            panelSeguridad.BackColor = TemaApp.FondoPanel;

            EstiloBoton(BtnOrientaciónTurística);
            EstiloBoton(btnActividadesTuristicas);
            EstiloBoton(btnTurismoSeguro);
        }
        private void EstiloBoton(Button btn)
        {
            btn.BackColor = TemaApp.BotonPrincipal;
            btn.ForeColor = TemaApp.BotonTexto;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            Redondear(btn, 30);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var r = MessageBox.Show(
        "¿Seguro que quieres salir?",
        "Confirmar salida",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question
    );

            if (r == DialogResult.Yes)
                Application.Exit(); // cierra toda la app

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            using (var f = new FrmReferencias())
            {
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog(this);
            }
        }


        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://repositorio.uteq.edu.ec/server/api/core/bitstreams/18b0cb88-f1fe-446d-920b-e32e3d88b86e/content";

            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(url)
                {
                    UseShellExecute = true
                });

            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo abrir el enlace. Asegúrese de tener conexión a internet.\n\nDetalle: " + ex.Message,
                                "Error de Navegación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://ridaa.unq.edu.ar/bitstream/handle/20.500.11807/1545/09_RCS-21_dossier8.pdf?sequence=1&isAllowed=";

            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(url)
                {
                    UseShellExecute = true
                });

            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo abrir el enlace. Asegúrese de tener conexión a internet.\n\nDetalle: " + ex.Message,
                                "Error de Navegación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://www.researchgate.net/publication/282556794_Tourists'_Approach_to_Local_Food";

            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(url)
                {
                    UseShellExecute = true
                });

            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo abrir el enlace. Asegúrese de tener conexión a internet.\n\nDetalle: " + ex.Message,
                                "Error de Navegación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://www.sciencedirect.com/science/article/pii/S2590198222002056/pdfft?download=true";

            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(url)
                {
                    UseShellExecute = true
                });

            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo abrir el enlace. Asegúrese de tener conexión a internet.\n\nDetalle: " + ex.Message,
                                "Error de Navegación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://www.mdpi.com/2071-1050/13/12/6693?utm_source";

            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(url)
                {
                    UseShellExecute = true
                });

            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo abrir el enlace. Asegúrese de tener conexión a internet.\n\nDetalle: " + ex.Message,
                                "Error de Navegación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }
    }
}
