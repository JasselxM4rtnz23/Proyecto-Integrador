using ConsoleApp4.Forms;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace ConsoleApp4
{
    public partial class Ahorcado : Form
    {
        char[] PalabrasAdivinadas;
        int Oportunidades;
        char[] PalabraSeleccionada;
        char[] Alfabeto;
        string[] Palabras;
        string[] Pistas;
        Timer Temporizador;
        int tiempoRestante;
        int nivelActual = 1;
        string[] Consejos;


        private Orientacion _formOrientacion;

        public Ahorcado(Orientacion formOrientacion)
        {
            InitializeComponent();
            _formOrientacion = formOrientacion;

            panelPista.Paint += panelPista_Paint;
        }

        private void IniciarJuego()
        {
            flFichasDeJuego.Controls.Clear();
            flFichasDeJuego.Enabled = true;
            flPalabra.Controls.Clear();

            picAhorcado.Image = null;
            lblMensaje.Visible = false;
            lblReiniciar.Visible = false;

            Oportunidades = 0;
            btnIniciarJuego.Image = Properties.Resources.Jugando;

            Palabras = new string[]
            {
                "quevedo",
                "orientacion",
                "turismo",
                "hospedaje",
                "alimentacion",
                "transporte"
            };

            Pistas = new string[]
            {
                "Ciudad importante de Ecuador",
                "Área que guía a las personas",
                "Actividad de viajar por placer",
                "Lugar donde te quedas a dormir",
                "Acción necesaria para vivir",
                "Medio utilizado para movilizarse"
            };

            Consejos = new string[]
            {
                "Quevedo es una ciudad comercial importante, investiga su cultura.",
                "La orientación ayuda a tomar mejores decisiones en la vida.",
                "El turismo impulsa la economía local.",
                "Elegir buen hospedaje mejora tu experiencia de viaje.",
                "Una alimentación saludable mejora tu rendimiento.",
                "El transporte facilita el desarrollo de las ciudades."
            };


            Alfabeto = "ABCDEFGHIJKLMNÑOPQRSTUVWXYZ".ToCharArray();

            Random random = new Random();
            int indice = random.Next(Palabras.Length);

            PalabraSeleccionada = Palabras[indice].ToUpper().ToCharArray();
            PalabrasAdivinadas = (char[])PalabraSeleccionada.Clone();

            lblPista.Text = "💡 PISTA"
                + Environment.NewLine + Environment.NewLine
                + Pistas[indice];

            lblPista.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblPista.ForeColor = Color.DarkBlue;

            lblPista.AutoSize = false;
            lblPista.Dock = DockStyle.Fill;
            lblPista.TextAlign = ContentAlignment.MiddleCenter;

            panelPista.Visible = true;

            foreach (char letra in Alfabeto)
            {
                Button btnLetra = new Button();
                btnLetra.Text = letra.ToString();
                btnLetra.Width = 60;
                btnLetra.Height = 40;
                btnLetra.Click += Compara;
                btnLetra.ForeColor = Color.White;
                btnLetra.Font = new Font("Segoe UI", 14, FontStyle.Bold);
                btnLetra.BackColor = Color.Black;
                btnLetra.FlatStyle = FlatStyle.Flat;

                flFichasDeJuego.Controls.Add(btnLetra);
            }

            for (int i = 0; i < PalabraSeleccionada.Length; i++)
            {
                Button letra = new Button();
                letra.Tag = PalabraSeleccionada[i].ToString();
                letra.Width = 50;
                letra.Height = 80;
                letra.ForeColor = Color.Blue;
                letra.Text = "_";
                letra.Font = new Font("Segoe UI", 28, FontStyle.Bold);
                letra.BackColor = Color.White;
                letra.FlatStyle = FlatStyle.Flat;
                letra.Name = "Adivinado" + i;

                flPalabra.Controls.Add(letra);
            }

            if (nivelActual == 1)
            {
                tiempoRestante = 60;
            }

            else
            {
                tiempoRestante = 30;
            }

            lblTiempo.Text = "Tiempo: " + tiempoRestante;
            lblTiempo.Visible = true;

            Temporizador.Stop();
            Temporizador.Start();
        }

        private void Compara(object sender, EventArgs e)
        {
            bool encontrado = false;
            Button btn = (Button)sender;

            btn.BackColor = Color.White;
            btn.ForeColor = Color.Black;
            btn.Enabled = false;

            for (int i = 0; i < PalabrasAdivinadas.Length; i++)
            {
                if (PalabrasAdivinadas[i] == Char.Parse(btn.Text))
                {
                    Button tbx = this.Controls.Find("Adivinado" + i, true).FirstOrDefault() as Button;

                    if (tbx != null)
                        tbx.Text = PalabrasAdivinadas[i].ToString();

                    PalabrasAdivinadas[i] = '-';
                    encontrado = true;
                }
            }

            bool Ganaste = PalabrasAdivinadas.All(c => c == '-');

            if (Ganaste)
            {
                Temporizador.Stop();

                if (nivelActual == 1)
                {
                    MessageBox.Show("🎉 ¡Pasas al Nivel 2!\nAhora tienes 30 segundos.");
                    nivelActual = 2;
                    IniciarJuego();
                }
                else
                {
                    MessageBox.Show("🏆 ¡GANASTE EL JUEGO COMPLETO!\n\nConsejo:\n" + Consejos[Array.IndexOf(Palabras, new string(PalabraSeleccionada).ToLower())]);
                    flFichasDeJuego.Enabled = false;
                    btnIniciarJuego.Enabled = true;
                }

                return;
            }



            if (!encontrado)
            {
                Oportunidades++;

                picAhorcado.Image =
                    (Bitmap)Properties.Resources.ResourceManager
                    .GetObject("ahorcado" + Oportunidades);

                if (Oportunidades == 7)
                {
                    lblMensaje.Text = "❌ ¡PERDISTE!";
                    lblMensaje.Visible = true;

                    MessageBox.Show("❌ ¡PERDISTE!\n\nConsejo:\n" +
                    Consejos[Array.IndexOf(Palabras, new string(PalabraSeleccionada).ToLower())]);


                    for (int i = 0; i < PalabraSeleccionada.Length; i++)
                    {
                        Button btnLetra = this.Controls.Find("Adivinado" + i, true).FirstOrDefault() as Button;

                        if (btnLetra != null)
                            btnLetra.Text = btnLetra.Tag.ToString();
                    }

                    flFichasDeJuego.Enabled = false;
                    Temporizador.Stop();
                }
            }
        }

        private void panelPista_Paint(object sender, PaintEventArgs e)
        {
            LinearGradientBrush brush =
                new LinearGradientBrush(
                    panelPista.ClientRectangle,
                    Color.LightYellow,
                    Color.Orange,
                    90F);

            e.Graphics.FillRectangle(brush, panelPista.ClientRectangle);
        }

        private void Ahorcado_Load(object sender, EventArgs e)
        {
            Temporizador = new Timer();
            Temporizador.Interval = 1000;
            Temporizador.Tick += Temporizador_Tick;

            IniciarJuego();
        }

        private void Temporizador_Tick(object sender, EventArgs e)
        {
            tiempoRestante--;
            lblTiempo.Text = "Tiempo: " + tiempoRestante;

            if (tiempoRestante <= 0)
            {
                Temporizador.Stop();

                lblMensaje.Text = "⏰ SE ACABÓ EL TIEMPO!!";
                lblMensaje.Visible = true;
                flFichasDeJuego.Enabled = false;

                MessageBox.Show("⏰ Tiempo terminado\n\nConsejo:\n" +
                Consejos[Array.IndexOf(Palabras, new string(PalabraSeleccionada).ToLower())]);
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

        private void bntVolver2_Click(object sender, EventArgs e)
        {
            _formOrientacion.Show();
            Hide();
        }

        private void btnIniciarJuego_Click(object sender, EventArgs e)
        {
            nivelActual = 1;
            Temporizador.Stop();
            IniciarJuego();
        }

    }
}
