using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace ConsoleApp4.Forms
{
    public partial class Orientacion : Form
    {
        // Referencia al formulario principal (OpcionesTurismo)
        private Form _formOpcionesTurismo;

        // Constructor que recibe el formulario padre
        public Orientacion(Form formOpcionesTurismo)
        {
            InitializeComponent();
            _formOpcionesTurismo = formOpcionesTurismo;
        }

        // Botón para abrir Alimentacion
        private void button1_Click(object sender, EventArgs e)
        {
            Alimentacion alimento = new Alimentacion(this); // pasamos Orientacion como padre
            alimento.Show();
            this.Hide();
        }

        // Botón para abrir Hospedaje
        private void button2_Click(object sender, EventArgs e)
        {
            Hospedaje hotel = new Hospedaje(this);
            hotel.Show();
            this.Hide();
        }

        // Botón para abrir Transporte
        private void button3_Click(object sender, EventArgs e)
        {
            Transporte bus = new Transporte(this);
            bus.Show();
            this.Hide();
        }

        // Botón para abrir Ahorcado
        private void bnJugar_Click(object sender, EventArgs e)
        {
            Ahorcado a = new Ahorcado(this);
            a.Show();
            this.Hide();
        }

        // Botón para regresar al formulario principal
        private void btnRegresar_Click(object sender, EventArgs e)
        {
            // Si se recibió una referencia al formulario padre, mostrarla y cerrar este formulario
            if (_formOpcionesTurismo != null)
            {
                try
                {
                    if (_formOpcionesTurismo.IsDisposed)
                    {
                        // Si el padre fue disposed, intentar recrearlo por reflexión
                        throw new ObjectDisposedException("parent");
                    }

                    _formOpcionesTurismo.Show();
                    this.Close();
                    return;
                }
                catch
                {
                    // Caerá al intento por reflexión más abajo
                }
            }

            // Buscar entre formularios abiertos uno llamado FrmOpciones
            var abierto = Application.OpenForms.Cast<Form>().FirstOrDefault(f => f.Name == "FrmOpciones" || f.GetType().Name == "FrmOpciones");
            if (abierto != null)
            {
                abierto.Show();
                this.Close();
                return;
            }

            // Intentar crear una instancia por reflexión sin enlazar el proyecto (evita referencias circulares)
            try
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                Type tipo = null;
                foreach (var asm in assemblies)
                {
                    // Buscar por nombre calificado primero
                    tipo = asm.GetType("QuevedoPlay.FrmOpciones");
                    if (tipo != null)
                        break;

                    // Fallback: buscar por nombre de tipo
                    tipo = asm.GetTypes().FirstOrDefault(t => t.Name == "FrmOpciones");
                    if (tipo != null)
                        break;
                }

                if (tipo != null && typeof(Form).IsAssignableFrom(tipo))
                {
                    var instancia = (Form)Activator.CreateInstance(tipo);
                    instancia.Show();
                    this.Close();
                    return;
                }

                MessageBox.Show("No se encontró el formulario 'FrmOpciones' cargado en la aplicación.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al intentar abrir FrmOpciones: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            var r = MessageBox.Show(
                "¿Seguro que quieres salir?",
                "Confirmar salida",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (r == DialogResult.Yes)
                Application.Exit();
        }
    }
}




