using ConsoleApp4.Forms;
using System;
using System.Windows.Forms;

namespace ConsoleApp4
{
    public partial class Transporte : Form
    {
        private Orientacion _formOrientacion;
        public Transporte(Orientacion formOrientacion)
        {
            InitializeComponent();
            _formOrientacion = formOrientacion;
        }
        private void AbrirFormulario(Form formHijo)
        {
            pnlCarro.Controls.Clear(); // Limpia el panel

            formHijo.TopLevel = false;
            formHijo.FormBorderStyle = FormBorderStyle.None;
            formHijo.Dock = DockStyle.Fill;

            pnlCarro.Controls.Add(formHijo);
            pnlCarro.Tag = formHijo;

            formHijo.Show();
        }
        private void bntVolver3_Click(object sender, EventArgs e)
        {
            _formOrientacion.Show();
            Hide();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedItem.ToString())
            {
                case "Terminal Terrestre Principal":
                    AbrirFormulario(new formPrincipal());
                    break;

                case "Mini Terminal":
                    AbrirFormulario(new formMini());
                    break;

                case "Taxis":
                    AbrirFormulario(new formTaxi());
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
