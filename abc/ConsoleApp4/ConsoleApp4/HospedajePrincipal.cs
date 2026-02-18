using ConsoleApp4.Forms;
using System;
using System.Windows.Forms;

namespace ConsoleApp4
{
    public partial class Hospedaje : Form
    {
        private Orientacion _formOrientacion;
        public Hospedaje(Orientacion formOrientacion)
        {
            InitializeComponent();
            _formOrientacion = formOrientacion;
        }
        private void AbrirFormulario(Form formHijo)
        {
            pnlHotel.Controls.Clear(); // Limpia el panel

            formHijo.TopLevel = false;
            formHijo.FormBorderStyle = FormBorderStyle.None;
            formHijo.Dock = DockStyle.Fill;

            pnlHotel.Controls.Add(formHijo);
            pnlHotel.Tag = formHijo;

            formHijo.Show();
        }

        private void bntVolver2_Click(object sender, EventArgs e)
        {
            _formOrientacion.Show();
            Hide();

        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedItem.ToString())
            {
                case "Hotel Palmar del Sol ":
                    AbrirFormulario(new formPalmar());
                    break;

                case "Hotel Olimpico":
                    AbrirFormulario(new formOlimpico());
                    break;

                case "Hotel San Andres Inn":
                    AbrirFormulario(new formSan());
                    break;

                case "Bravo Grand Hotel":
                    AbrirFormulario(new formBravo());
                    break;

                case "Del Rio Hotel":
                    AbrirFormulario(new formRio());
                    break;

                case "Hotel Golden":
                    AbrirFormulario(new formGolden());
                    break;

                case "Hotel Ingles":
                    AbrirFormulario(new formIngles());
                    break;
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
