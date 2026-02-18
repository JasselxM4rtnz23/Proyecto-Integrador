using QuevedoPlay.Estilos;
using System;
using System.Windows.Forms;


namespace QuevedoPlay
{
    public partial class OpcionesTurismoSeguro : Form
    {
        public OpcionesTurismoSeguro()
        {
            InitializeComponent();
        }

        private void OpcionesTurismoSeguro_Load(object sender, EventArgs e)
        {
            RedondeoHelper.Aplicar(button1, 10);
            RedondeoHelper.Aplicar(button2, 10);
            RedondeoHelper.Aplicar(button3, 10);
            RedondeoHelper.Aplicar(btnRegresar, 10);
            RedondeoHelper.Aplicar(btnSalir, 10);

        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            FrmOpciones form2 = new FrmOpciones();
            form2.Show();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TurismoSeguro mapa = new TurismoSeguro();
            mapa.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MedidasSeguridad frmmedidas = new MedidasSeguridad();
            frmmedidas.Show();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CrucigramaSeg formCru = new CrucigramaSeg();
            formCru.Show();
            this.Close();
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
