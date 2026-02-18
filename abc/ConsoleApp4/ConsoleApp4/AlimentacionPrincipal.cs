using ConsoleApp4.Forms;
using System;
using System.Windows.Forms;

namespace ConsoleApp4
{
    public partial class Alimentacion : Form
    {
        private Orientacion _formOrientacion;
        public Alimentacion(Orientacion formOrientacion)
        {
            InitializeComponent();
            _formOrientacion = formOrientacion;
        }
        private void AbrirFormulario(Form formHijo)
        {
            pnlComidas.Controls.Clear(); // Limpia el panel

            formHijo.TopLevel = false;
            formHijo.FormBorderStyle = FormBorderStyle.None;
            formHijo.Dock = DockStyle.Fill;

            pnlComidas.Controls.Add(formHijo);
            pnlComidas.Tag = formHijo;

            formHijo.Show();
        }
        private void btnVolver_Click(object sender, EventArgs e)
        {
            _formOrientacion.Show();
            Hide();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedItem.ToString())
            {
                case "Chifa Miraflores":
                    AbrirFormulario(new formMiraflores());
                    break;

                case "The Best Wings":
                    AbrirFormulario(new formBest());
                    break;

                case "Cevichería Sport Fish":
                    AbrirFormulario(new formCeviche());
                    break;

                case "FOGO - Carnes de Autor":
                    AbrirFormulario(new formAsadero());
                    break;

                case "Lokos D' Asar":
                    AbrirFormulario(new formLokos());
                    break;

                case "El Tizne":
                    AbrirFormulario(new fortTinez());
                    break;

                case "La Terraza El Mirador":
                    AbrirFormulario(new formMirador());
                    break;
            }

        }

        private void Alimentacion_Load(object sender, EventArgs e)
        {

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