using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuevedoPlay
{
    public partial class FrmReferencias : Form
    {
        public FrmReferencias()
        {
            InitializeComponent();
        }

        private void FrmReferencias_Load(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;


            // Scroll container
            pnlScroll.Dock = DockStyle.Fill;
            pnlScroll.AutoScroll = true;

            // Contenido real
            panel1.Dock = DockStyle.None;               // importante: NO Fill dentro de AutoScroll
            panel1.Location = new Point(12, 12);
            panel1.Padding = new Padding(12);

            // Truco: el panel debe tener un tamaño real para que el scroll funcione
            panel1.AutoSize = true;
            panel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
