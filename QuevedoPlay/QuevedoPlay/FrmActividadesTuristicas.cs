using QuevedoPlay.Estilos;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;

namespace QuevedoPlay
{
    public partial class FrmActividadesTuristicas : Form
    {
        // ===== Layout =====
        private const int TOP_MARGIN = 95;
        private const int SIDE_MARGIN = 45;
        private const int GAP = 35;

        private const int BOTTOM_PADDING = 22;
        private const int CLOSE_PAD_X = 18;
        private const int CLOSE_PAD_Y = 18;

        private static readonly Size FORM_SIZE = new Size(1100, 650);
        private static readonly Size CARD_SIZE = new Size(310, 430);

        private bool _uiReady;

        public FrmActividadesTuristicas()
        {
            InitializeComponent();
            ConfigureForm();
            WireEvents();
        }

        private void ConfigureForm()
        {
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            BackColor = TemaApp.FondoPrincipal;

            MaximizeBox = false;
            MinimumSize = FORM_SIZE;
            ClientSize = FORM_SIZE;

            DoubleBuffered = true;

            EnableDoubleBuffer(panel1);
            EnableDoubleBuffer(panel2);
            EnableDoubleBuffer(panel3);
        }

        private void WireEvents()
        {
            Load += FrmActividadesTuristicas_Load;
            Shown += (s, e) => { CenterToScreen(); ApplyLayout(); };
            Resize += (s, e) => ApplyLayout();
        }

        private void FrmActividadesTuristicas_Load(object sender, EventArgs e)
        {
            if (_uiReady) return;
            _uiReady = true;

            // Tamaño de cards
            panel1.Size = CARD_SIZE;
            panel2.Size = CARD_SIZE;
            panel3.Size = CARD_SIZE;

            // Estilos (solo estilo, no posiciones)
            EstilizarPanel(panel1, label3, label6, ptBoxTalleres, button1);
            EstilizarPanel(panel2, label4, label7, ptBoxCaminatas, button2);
            EstilizarPanel(panel3, label5, label8, ptBoxVisitasGuiadas, button3);

            // Botones inferiores

            btnRegresar.ForeColor = TemaApp.BotonTexto;

            if (button4 != null)
            {

                button4.ForeColor = TemaApp.BotonTexto;
            }

            btnSalirApp.BackColor = Color.Red;
            btnSalirApp.ForeColor = Color.White;



            // Redondeos botones (una vez)
            Redondear(btnRegresar, 22);
            Redondear(button1, 22);
            Redondear(button2, 22);
            Redondear(button3, 22);
            if (button4 != null) Redondear(button4, 22);

            ApplyLayout();
        }

        private void ApplyLayout()
        {
            if (!IsHandleCreated) return;

            SuspendLayout();

            // 1) Posición de cards (centradas)
            int cardsTotalWidth = panel1.Width + panel2.Width + panel3.Width + GAP * 2;
            int startX = Math.Max(SIDE_MARGIN, (ClientSize.Width - cardsTotalWidth) / 2);
            int yCardsTop = TOP_MARGIN;

            panel1.Left = startX;
            panel2.Left = panel1.Right + GAP;
            panel3.Left = panel2.Right + GAP;

            panel1.Top = yCardsTop;
            panel2.Top = yCardsTop;
            panel3.Top = yCardsTop;

            // 2) Barra inferior (botones)
            int barHeight = Math.Max(btnRegresar.Height, button4?.Height ?? 0) + (BOTTOM_PADDING * 2);
            int maxCardsBottom = ClientSize.Height - barHeight - 10;

            if (panel1.Bottom > maxCardsBottom)
            {
                int delta = panel1.Bottom - maxCardsBottom;
                panel1.Top -= delta;
                panel2.Top -= delta;
                panel3.Top -= delta;
            }

            btnRegresar.Left = SIDE_MARGIN;
            btnRegresar.Top = ClientSize.Height - BOTTOM_PADDING - btnRegresar.Height;

            if (button4 != null)
            {
                button4.Left = ClientSize.Width - SIDE_MARGIN - button4.Width;
                button4.Top = ClientSize.Height - BOTTOM_PADDING - button4.Height;
                button4.BringToFront();
            }

            // 3) Layout interno de cada card (esto es lo que te faltaba)
            LayoutCard(panel1, ptBoxTalleres, label3, label6, button1);
            LayoutCard(panel2, ptBoxCaminatas, label4, label7, button2);
            LayoutCard(panel3, ptBoxVisitasGuiadas, label5, label8, button3);

            // 4) Botones inferiores alineados a las cards (se ve más pro)
            int bottomY = ClientSize.Height - BOTTOM_PADDING - btnRegresar.Height;

            // Regresar alineado a la card izquierda
            btnRegresar.Top = bottomY;
            btnRegresar.Left = panel1.Left;

            // Juego interactivo alineado a la card derecha
            if (button4 != null)
            {
                button4.Top = ClientSize.Height - BOTTOM_PADDING - button4.Height;
                button4.Left = panel3.Right - button4.Width;
                button4.BringToFront();
            }

            // 5) Redondeos DESPUÉS de calcular tamaños (clave)
            RedondearPanel(panel1, 28);
            RedondearPanel(panel2, 28);
            RedondearPanel(panel3, 28);

            Redondear(button1, 22);
            Redondear(button2, 22);
            Redondear(button3, 22);

            Redondear(btnRegresar, 22);
            if (button4 != null) Redondear(button4, 22);

            // Botón X rojo
            btnSalirApp.Top = CLOSE_PAD_Y;
            btnSalirApp.Left = ClientSize.Width - CLOSE_PAD_X - btnSalirApp.Width;
            btnSalirApp.BringToFront();
            Redondear(btnSalirApp, 10);


            ResumeLayout(true);
        }

        // ===== Estilo (no posiciones) =====
        private void EstilizarPanel(Panel panel, Label titulo, Label texto, PictureBox img, Button btn)
        {
            panel.BackColor = TemaApp.FondoPanel;       // blanco
            panel.BorderStyle = BorderStyle.None;
            panel.Padding = new Padding(22);

            img.SizeMode = PictureBoxSizeMode.Zoom;
            img.BorderStyle = BorderStyle.None;
            img.BackColor = panel.BackColor;

            titulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            titulo.ForeColor = TemaApp.TextoPrincipal;
            titulo.TextAlign = ContentAlignment.MiddleCenter;
            titulo.AutoSize = false;

            texto.Font = new Font("Segoe UI", 10.5F);
            texto.ForeColor = TemaApp.TextoSecundario;
            texto.TextAlign = ContentAlignment.TopCenter;
            texto.AutoSize = false;

            btn.UseVisualStyleBackColor = false;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;

            // hover / down derivados del tema (más oscuro)
            btn.FlatAppearance.MouseOverBackColor = ControlPaint.Dark(TemaApp.BotonPrincipal, 0.08f);
            btn.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(TemaApp.BotonPrincipal, 0.16f);

            btn.BackColor = TemaApp.BotonPrincipal;
            btn.ForeColor = TemaApp.BotonTexto;
            btn.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
        }


        // ===== Layout interno pro (posiciones) =====
        private void LayoutCard(Panel panel, PictureBox img, Label titulo, Label texto, Button btn)
        {
            int padC = 22;                 // padding SOLO para texto
            int wFull = panel.ClientSize.Width;

            int gap1 = 12, gap2 = 8, gap3 = 14;
            int titleH = 34;

            int btnW = 170, btnH = 44, btnBottomPad = 26;

            // Botón abajo centrado
            int btnY = panel.ClientSize.Height - btnBottomPad - btnH;
            btn.SetBounds((wFull - btnW) / 2, btnY, btnW, btnH);
            Redondear(btn, 22);

            // Texto fijo para que NO se coma la imagen (ajusta si quieres)
            int textH = 78;

            // Imagen: que use TODO el ancho (SIN margen)
            int imgH = btnY - gap3 - (titleH + gap1 + gap2 + textH);
            imgH = Math.Max(200, Math.Min(imgH, 260));  // para que no quede mini

            img.Dock = DockStyle.None;
            img.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            img.SetBounds(0, 0, wFull, imgH);

            // Contenido con padding (texto)
            int w = wFull - padC * 2;

            int y = img.Bottom + gap1;
            titulo.SetBounds(padC, y, w, titleH);

            y = titulo.Bottom + gap2;
            texto.SetBounds(padC, y, w, textH);
        }


        private static int Clamp(int v, int min, int max) => (v < min) ? min : (v > max) ? max : v;

        private static void StylePrimary(Button b, Size size, Color back, int radius)
        {
            if (b == null) return;

            b.Size = size;
            b.BackColor = back;
            b.ForeColor = Color.White;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
        }

        private static void StyleSecondary(Button b, Size size)
        {
            if (b == null) return;

            b.Size = size;
            b.BackColor = Color.FromArgb(230, 230, 230);
            b.ForeColor = Color.Black;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
        }

        // ===== Redondeo =====
        private void Redondear(Control control, int radio)
        {
            if (control == null) return;
            if (control.Width <= radio || control.Height <= radio) return;

            using (GraphicsPath path = new GraphicsPath())
            {
                path.StartFigure();
                path.AddArc(0, 0, radio, radio, 180, 90);
                path.AddArc(control.Width - radio, 0, radio, radio, 270, 90);
                path.AddArc(control.Width - radio, control.Height - radio, radio, radio, 0, 90);
                path.AddArc(0, control.Height - radio, radio, radio, 90, 90);
                path.CloseFigure();
                control.Region = new Region(path);
            }
        }

        private void RedondearPanel(Panel panel, int radio)
        {
            if (panel == null) return;
            if (panel.Width <= radio || panel.Height <= radio) return;

            using (GraphicsPath path = new GraphicsPath())
            {
                path.StartFigure();
                path.AddArc(0, 0, radio, radio, 180, 90);
                path.AddArc(panel.Width - radio, 0, radio, radio, 270, 90);
                path.AddArc(panel.Width - radio, panel.Height - radio, radio, radio, 0, 90);
                path.AddArc(0, panel.Height - radio, radio, radio, 90, 90);
                path.CloseFigure();
                panel.Region = new Region(path);
            }
        }

        private static void EnableDoubleBuffer(Control c)
        {
            if (c == null) return;
            typeof(Control).GetProperty("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.SetValue(c, true, null);
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            FrmOpciones opciones = new FrmOpciones();
            opciones.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var f = new FrmActividadDetalle();
            
            f.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var f = new FrmActividadDetalle(ActividadBuilders.BuildCaminatas());
            
            f.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var f = new FrmActividadDetalle(ActividadBuilders.BuildVisitas());
            
            f.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var frm = new ClasificaActividadForm();
            
            frm.Show();
            this.Hide();
        }

        private void btnSalirApp_Click(object sender, EventArgs e)
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
