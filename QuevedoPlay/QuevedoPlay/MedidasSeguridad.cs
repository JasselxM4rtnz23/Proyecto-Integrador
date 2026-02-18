using QuevedoPlay.Estilos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;


namespace QuevedoPlay
{

    public partial class MedidasSeguridad : Form
    {
        List<Carta> cartas;
        int indiceCentral = 0;

        int velocidad = 35;
        bool moviendoDerecha = false;
        bool moviendoIzquierda = false;

        int posLeft, posCenter, posRight;

        public MedidasSeguridad()
        {
            InitializeComponent();
            CargarCartas();
            ActualizarCarrusel();

            this.DoubleBuffered = true;
            this.KeyPreview = true;

        }

        void CargarCartas()
        {
            cartas = new List<Carta>
            {
                new Carta(
                    "Seguridad Ciudadana",
                    "Usa rutas oficiales y transporte autorizado para evitar robos y situaciones de riesgo.",
                    Properties.Resources.SeguridadCiudadana
                ),
                new Carta(
                    "Seguridad Sanitaria",
                    "Consume solo agua potable y viaja con seguro activo para prevenir emergencias de salud.",
                    Properties.Resources.SeguridadSanitaria
                ),
                new Carta(
                    "Seguridad Informativa",
                    "Descarga mapas offline y guarda contactos oficiales para actuar ante emergencias.",
                    Properties.Resources.SeguridadInformativa
                ),
                new Carta(
                    "Seguridad Económica",
                    "Consulta precios antes de pagar y utiliza medios seguros para evitar fraudes.",
                    Properties.Resources.SeguridadEconómica
                ),
                new Carta(
                    "Seguridad al Movilizarse",
                    "Muévete en transporte público autorizado o turístico legal para reducir riesgos.",
                    Properties.Resources.SeguridadDesplazamiento
                ),
                new Carta(
                    "Seguridad Psicológica",
                    "Mantén contacto con familiares durante el viaje para sentirte acompañado y seguro.",
                    Properties.Resources.SeguridadPsicológica
                )
            };
        }
        void ActualizarCarrusel()
        {
            int total = cartas.Count;

            int indexLeft = (indiceCentral - 1 + total) % total;
            int indexRight = (indiceCentral + 1) % total;

            //Controles para panel de la izquierda
            lblTituloLeft.Text = cartas[indexLeft].Titulo;
            lblDescripcionLeft.Text = cartas[indexLeft].Descripcion;
            picLeft.Image = cartas[indexLeft].Imagen;

            //Controles para panel de central
            lblTituloCenter.Text = cartas[indiceCentral].Titulo;
            lblDescripcionCenter.Text = cartas[indiceCentral].Descripcion;
            picCenter.Image = cartas[indiceCentral].Imagen;

            //Controles para panel de la derecha
            lblTituloRight.Text = cartas[indexRight].Titulo;
            lblDescripcionRight.Text = cartas[indexRight].Descripcion;
            picRight.Image = cartas[indexRight].Imagen;

            DibujarIndicadores();
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            OpcionesTurismoSeguro opTurismo = new OpcionesTurismoSeguro();
            opTurismo.Show();
            this.Close();
        }

        private GraphicsPath CrearPath(Rectangle rect, int radio)
        {
            GraphicsPath path = new GraphicsPath();
            int d = radio * 2;

            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);

            path.CloseFigure();
            return path;
        }

        private void AplicarPanelRedondeado
            (
                Panel panel,
                int radio,
                Color colorBorde,
                int grosor
            )
        {
            panel.BorderStyle = BorderStyle.None;

            panel.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                //Path del recorte (completo)
                Rectangle rectRegion = new Rectangle(
                    0, 0,
                    panel.Width,
                    panel.Height
                );

                //Path del borde (más pequeño)
                Rectangle rectBorde = new Rectangle(
                    grosor,
                    grosor,
                    panel.Width - (grosor * 2),
                    panel.Height - (grosor * 2)
                );

                using (GraphicsPath pathRegion = CrearPath(rectRegion, radio))
                using (GraphicsPath pathBorde = CrearPath(rectBorde, radio - 1))
                using (Pen pen = new Pen(colorBorde, grosor))
                {
                    panel.Region = new Region(pathRegion);
                    e.Graphics.DrawPath(pen, pathBorde);
                }
            };

            panel.Resize += (s, e) => panel.Invalidate();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (timerSlider.Enabled)
                return true;

            if (keyData == Keys.Right)
            {
                moviendoIzquierda = true;
                timerSlider.Start();
                return true;
            }

            if (keyData == Keys.Left)
            {
                moviendoDerecha = true;
                timerSlider.Start();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }


        private void Punto_Click(object sender, EventArgs e)
        {
            if (timerSlider.Enabled) return;

            Panel punto = sender as Panel;
            int indiceSeleccionado = (int)punto.Tag;

            // Mover el carrusel a esa carta
            indiceCentral = indiceSeleccionado;
            ActualizarCarrusel();
        }

        void DibujarIndicadores()
        {
            flowIndicadores.Controls.Clear();

            for (int i = 0; i < cartas.Count; i++)
            {
                Panel punto = new Panel();
                punto.Width = 12;
                punto.Height = 12;
                punto.Margin = new Padding(6);
                punto.Tag = i;
                punto.Cursor = Cursors.Hand;

                punto.Click += Punto_Click;

                punto.Paint += (s, e) =>
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                    Color color = ((int)punto.Tag == indiceCentral)
                        ? Color.DodgerBlue
                        : Color.LightGray;

                    using (SolidBrush brush = new SolidBrush(color))
                    {
                        e.Graphics.FillEllipse(brush, 0, 0, 11, 11);
                    }
                };

                flowIndicadores.Controls.Add(punto);
            }
        }

        private void MedidasSeguridad_Load(object sender, EventArgs e)
        {
            RedondeoHelper.Aplicar(btnRegresar, 20);
            RedondeoHelper.Aplicar(btnSalir, 20);


            posLeft = panelLeft.Left;
            posCenter = panelCenter.Left;
            posRight = panelRight.Left;

            AplicarPanelRedondeado(panelCenter, 25, Color.FromArgb(120, 160, 200), 2);
            RedondeoHelper.Aplicar(panelLeft, 35);
            RedondeoHelper.Aplicar(panelRight, 35);
            RedondeoHelper.Aplicar(lblDescripcionRight, 35);
            RedondeoHelper.Aplicar(lblDescripcionCenter, 35);
            RedondeoHelper.Aplicar(lblDescripcionLeft, 35);

            DibujarIndicadores();

            ActivarHoverPictureBox(btnAnterior);
            ActivarHoverPictureBox(btnSiguiente);
            HacerPictureBoxCircular(btnAnterior);
            HacerPictureBoxCircular(btnSiguiente);
        }

        private void timerSlider_Tick(object sender, EventArgs e)
        {
            if (moviendoIzquierda)
            {
                panelLeft.Left -= velocidad;
                panelCenter.Left -= velocidad;
                panelRight.Left -= velocidad;

                if (panelCenter.Left <= posLeft)
                {
                    timerSlider.Stop();
                    moviendoIzquierda = false;

                    this.SuspendLayout();

                    indiceCentral = (indiceCentral + 1) % cartas.Count;

                    panelLeft.Left = posLeft;
                    panelCenter.Left = posCenter;
                    panelRight.Left = posRight;

                    ActualizarCarrusel();

                    this.ResumeLayout();
                }
            }

            if (moviendoDerecha)
            {
                panelLeft.Left += velocidad;
                panelCenter.Left += velocidad;
                panelRight.Left += velocidad;

                if (panelCenter.Left >= posRight)
                {
                    timerSlider.Stop();
                    moviendoDerecha = false;

                    this.SuspendLayout();

                    indiceCentral = (indiceCentral - 1 + cartas.Count) % cartas.Count;

                    panelLeft.Left = posLeft;
                    panelCenter.Left = posCenter;
                    panelRight.Left = posRight;

                    ActualizarCarrusel();

                    this.ResumeLayout();
                }
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

        private void btnAnterior_Click_1(object sender, EventArgs e)
        {
            if (!timerSlider.Enabled)
            {
                moviendoDerecha = true;
                timerSlider.Start();
            }
        }

        private void btnSiguiente_Click_1(object sender, EventArgs e)
        {
            if (!timerSlider.Enabled)
            {
                moviendoIzquierda = true;
                timerSlider.Start();
            }
        }
        void ActivarHoverPictureBox(PictureBox pic)
        {
            pic.MouseEnter += (s, e) =>
            {
                pic.BackColor = Color.FromArgb(220, 220, 220);

            };

            pic.MouseLeave += (s, e) =>
            {
                pic.BackColor = Color.Transparent;
            };
        }

        void HacerPictureBoxCircular(PictureBox pic)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, pic.Width, pic.Height);
            pic.Region = new Region(path);
        }
    }
}
