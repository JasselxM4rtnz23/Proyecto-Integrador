using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuevedoPlay
{
    public partial class ClasificaActividadForm : Form
    {
        // =========================
        // ESTADO
        // =========================
        private readonly Random rnd = new Random();
        private bool tutorialMostrado = false;

        private int nivelActual = 1;
        private const int maxErrores = 5;
        private int errores = 0;
        private int puntaje = 0;
        private int tarjetasCorrectas = 0;
        private int totalTarjetas = 0;

        private int tiempoRestante = 60;
        private const int bonusPorSegundo = 2;
        private bool juegoTerminado = false;

        private int puntajeMaximo = 0;
        private readonly string rutaArchivoRecord = "record.txt";
        private Panel pnlClient;


        // =========================
        // DATA
        // =========================
        private const int CardsPerCategoryNivel1 = 4;
        private const int CardsPerCategoryNivel2 = 6;

        private readonly Dictionary<string, List<string>> bancoTarjetas = new Dictionary<string, List<string>>
        {
            ["Naturaleza"] = new List<string>
            {
                "Sendero ecológico","Ruta de senderismo","Mirador panorámico","Avistamiento de aves",
                "Paseo por el río","Tour de paisajes naturales","Ruta en bicicleta","Parque natural",
                "Caminata por bosque","Visita a cascada","Recorrido por humedal","Observación de flora",
                "Ruta de trekking","Paseo al aire libre","Tour de fotografía de naturaleza",
                "Recorrido por finca ecológica","Sendero interpretativo","Paseo por ribera del río"
            },
            ["Cultura"] = new List<string>
            {
                "Recorrido por el centro histórico","Visita al museo local","Feria artesanal","Exposición de arte",
                "Festival de la ciudad","Danza tradicional","Tour de leyendas e historias","Visita a iglesia patrimonial",
                "Murales y arte urbano","Casa de la cultura","Taller de cerámica","Taller de tejido artesanal",
                "Demostración de tallado en madera","Concierto cultural","Teatro comunitario","Ruta patrimonial",
                "Visita guiada a monumentos","Feria cultural"
            },
            ["Gastronomía"] = new List<string>
            {
                "Degustación de platos típicos","Ruta de comida local","Mercado gastronómico","Feria gastronómica",
                "Jugos tropicales","Dulces tradicionales","Helados artesanales","Ceviche tradicional",
                "Encebollado","Bolón de verde","Empanadas caseras","Tigrillo",
                "Seco tradicional","Tostado y snacks locales","Degustación de cacao/chocolate",
                "Café de la zona","Parrillada local","Plato típico de la región"
            }
        };

        private static readonly string[] CATEGORIAS = { "Naturaleza", "Cultura", "Gastronomía" };

        // =========================
        // TEMA (más vivo)
        // =========================
        private readonly Color Bg = Color.FromArgb(243, 246, 255);
        private readonly Color CardBg = Color.White;
        private readonly Color CardHover = Color.FromArgb(240, 247, 255);
        private readonly Color Border = Color.FromArgb(210, 223, 247);

        private readonly Color Header1 = Color.FromArgb(15, 94, 247);   // azul intenso
        private readonly Color Header2 = Color.FromArgb(19, 194, 194);  // teal

        private readonly Color Danger = Color.FromArgb(239, 68, 68);
        private readonly Color Success = Color.FromArgb(34, 197, 94);
        private readonly Color Warn = Color.FromArgb(245, 158, 11);

        private readonly Color Accent = Color.FromArgb(59, 130, 246);   // azul vivo
        private readonly Color GrayBtn = Color.FromArgb(107, 114, 128);

        private Font fTitle = new Font("Segoe UI", 16, FontStyle.Bold);
        private Font fStats = new Font("Segoe UI", 10.5f, FontStyle.Bold);
        private Font fBody = new Font("Segoe UI", 10, FontStyle.Regular);

        // =========================
        // UI
        // =========================
        private Panel pnlHeader, pnlFooter, pnlOverlay;
        private TableLayoutPanel root;

        private Label lblTitulo, lblPuntaje, lblErrores, lblTiempo, lblNivel;
        private Timer timerNivel;

        private Button btnRegresar, btnComoJugar;

        private ProgressBarEx progress;

        private RoundedPanel cardPoolContainer;
        private FlowLayoutPanel flpPool;

        private TableLayoutPanel tlpDropZones;
        private DropZone zoneNaturaleza, zoneCultura, zoneGastro;

        private RoundedPanel endDialog;
        private Label endTitle, endSubtitle, endScore;
        private Button endPlayAgain, endExit;

        public ClasificaActividadForm()
        {
            InitializeComponent();

            // Si el diseñador te metió cosas raras, aquí lo neutralizamos.
            Controls.Clear();

            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Bg;
            DoubleBuffered = true;

            BuildUI();
            Shown += (s, e) => StartGame();

            Resize += (s, e) =>
            {
                ApplyResponsiveLayout();
                LayoutPoolCards();
                CenterEndDialog();
            };
        }



        // =========================================================
        // UI BUILD
        // =========================================================
        private void BuildUI()
        {
            SuspendLayout();

            Text = "Clasifica la Actividad";
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            ClientSize = new Size(1100, 700);
            BackColor = Bg;

            // =========================
            // CONTENEDOR RAÍZ (3 FILAS)
            // Header (84) / Cliente (*) / Footer (76)
            // =========================
            var frame = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = Bg,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };
            frame.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            frame.RowStyles.Add(new RowStyle(SizeType.Absolute, 84));
            frame.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            frame.RowStyles.Add(new RowStyle(SizeType.Absolute, 76));
            Controls.Add(frame);

            // =========================
            // HEADER
            // =========================
            pnlHeader = new Panel { Dock = DockStyle.Fill };
            pnlHeader.Paint += Header_Paint;
            frame.Controls.Add(pnlHeader, 0, 0);

            var headerGrid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(18,14, 18, 12),
                BackColor = Color.Transparent
            };
            headerGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 52));
            headerGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48));
            pnlHeader.Controls.Add(headerGrid);

            lblTitulo = new Label
            {
                Text = "Clasifica la Actividad",
                Font = fTitle,
                ForeColor = Color.White,

                AutoSize = false,
                Dock = DockStyle.Fill,

                TextAlign = ContentAlignment.TopCenter,   // ✅ arriba + centrado

                Margin = new Padding(0),
                Padding = new Padding(0, 2, 0, 0)         // ✅ ajusta 0..6 según te guste
            };




            var stats = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 2, 0, 0)
            };

            lblNivel = Stat("Nivel: 1");
            lblTiempo = Stat("Tiempo: 0s");
            lblErrores = Stat("Errores: 0/5");
            lblPuntaje = Stat("Puntaje: 0");

            stats.Controls.Add(lblNivel);
            stats.Controls.Add(lblTiempo);
            stats.Controls.Add(lblErrores);
            stats.Controls.Add(lblPuntaje);

            headerGrid.Controls.Add(lblTitulo, 0, 0);
            headerGrid.Controls.Add(stats, 1, 0);

            // =========================
            // CLIENTE (JUEGO EN MEDIO)
            // =========================
            pnlClient = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Bg,
                // ✅ margen real arriba/abajo para que NO se pegue al header/footer
                Padding = new Padding(0, 10, 0, 10)
            };
            frame.Controls.Add(pnlClient, 0, 1);

            // =========================
            // FOOTER
            // =========================
            pnlFooter = new Panel { Dock = DockStyle.Fill, BackColor = Bg };
            pnlFooter.Paint += (s, e) =>
            {
                using (var p = new Pen(Border, 1))
                    e.Graphics.DrawLine(p, 0, 0, pnlFooter.Width, 0);
            };
            frame.Controls.Add(pnlFooter, 0, 2);

            var footerGrid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(18, 14, 18, 14),
                BackColor = Bg
            };
            footerGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            footerGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            pnlFooter.Controls.Add(footerGrid);

            btnRegresar = MakeButton("Regresar", Danger, Color.White);
            btnRegresar.Width = 160;
            btnRegresar.Click += (s, e) =>
            {
                timerNivel?.Stop();
                var r = MessageBox.Show(
                    "¿Deseas salir del juego?\nSe perderá el progreso del nivel actual.",
                    "Confirmar salida",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );
                if (r == DialogResult.Yes)
                {
                    var f = new FrmActividadesTuristicas();
                    f.Show();
                    Hide();
                }
                else
                {
                    if (!juegoTerminado) timerNivel?.Start();
                }
            };

            btnComoJugar = MakeButton("¿Cómo jugar?", Accent, Color.White);
            btnComoJugar.Width = 180;
            btnComoJugar.Click += (s, e) =>
            {
                timerNivel?.Stop();
                using (var f = new HowToPlayForm()) f.ShowDialog(this);
                if (!juegoTerminado) timerNivel?.Start();
            };

            var leftFooter = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight, WrapContents = false, BackColor = Color.Transparent };
            var rightFooter = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft, WrapContents = false, BackColor = Color.Transparent };

            leftFooter.Controls.Add(btnRegresar);
            rightFooter.Controls.Add(btnComoJugar);

            footerGrid.Controls.Add(leftFooter, 0, 0);
            footerGrid.Controls.Add(rightFooter, 1, 0);

            // =========================
            // ROOT DEL JUEGO (DENTRO DEL pnlClient)
            // =========================
            root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Bg,
                Padding = new Padding(18, 12, 18, 12), // ✅ margen interno arriba/abajo
                Margin = new Padding(0),
                ColumnCount = 2,
                RowCount = 1
            };
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34));
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 66));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            pnlClient.Controls.Add(root);

            // Left: pool
            var leftCol = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent, Padding = new Padding(0, 0, 10, 0) };
            root.Controls.Add(leftCol, 0, 0);

            progress = new ProgressBarEx
            {
                Dock = DockStyle.Top,
                Height = 18,
                Margin = new Padding(0, 0, 0, 12),
                TrackColor = Color.FromArgb(226, 234, 252),
                Fill1 = Color.FromArgb(99, 102, 241),
                Fill2 = Color.FromArgb(34, 197, 94)
            };

            cardPoolContainer = new RoundedPanel
            {
                Dock = DockStyle.Fill,
                Radius = 18,
                FillColor = Color.FromArgb(255, 255, 255),
                BorderColor = Border,
                BorderWidth = 1,
                Padding = new Padding(14),
                Shadow = true
            };

            var poolHeader = new Label
            {
                Text = "Tarjetas",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(17, 24, 39),
                Dock = DockStyle.Top,
                Height = 28
            };

            var poolHint = new Label
            {
                Text = "Arrastra cada tarjeta a su categoría correcta",
                Font = fBody,
                ForeColor = Color.FromArgb(75, 85, 99),
                Dock = DockStyle.Top,
                Height = 22
            };

            flpPool = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                BackColor = Color.Transparent,
                Padding = new Padding(4, 10, 4, 4)
            };
            flpPool.SizeChanged += (s, e) => LayoutPoolCards();

            cardPoolContainer.Controls.Add(flpPool);
            cardPoolContainer.Controls.Add(poolHint);
            cardPoolContainer.Controls.Add(poolHeader);

            leftCol.Controls.Add(cardPoolContainer);
            leftCol.Controls.Add(progress);

            // Right: drop zones
            var rightCol = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent, Padding = new Padding(10, 0, 0, 0) };
            root.Controls.Add(rightCol, 1, 0);

            tlpDropZones = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = Color.Transparent,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };
            tlpDropZones.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            tlpDropZones.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33f));
            tlpDropZones.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33f));
            tlpDropZones.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33f));

            zoneNaturaleza = new DropZone("Naturaleza", "🌿 Naturaleza", Color.FromArgb(34, 197, 94), Border);
            zoneCultura = new DropZone("Cultura", "🏛️ Cultura", Color.FromArgb(59, 130, 246), Border);
            zoneGastro = new DropZone("Gastronomía", "🍽️ Gastronomía", Color.FromArgb(245, 158, 11), Border);

            zoneNaturaleza.Dock = DockStyle.Fill;
            zoneCultura.Dock = DockStyle.Fill;
            zoneGastro.Dock = DockStyle.Fill;

            zoneNaturaleza.Margin = new Padding(0, 0, 0, 14);
            zoneCultura.Margin = new Padding(0, 0, 0, 14);
            zoneGastro.Margin = new Padding(0);

            tlpDropZones.Controls.Add(zoneNaturaleza, 0, 0);
            tlpDropZones.Controls.Add(zoneCultura, 0, 1);
            tlpDropZones.Controls.Add(zoneGastro, 0, 2);

            rightCol.Controls.Add(tlpDropZones);

            zoneNaturaleza.DropRequested += OnDropRequested;
            zoneCultura.DropRequested += OnDropRequested;
            zoneGastro.DropRequested += OnDropRequested;

            // =========================
            // OVERLAY (ENCIMA DE TODO)
            // =========================
            pnlOverlay = new Panel
            {
                BackColor = Color.FromArgb(160, 0, 0, 0),
                Visible = false,
                Dock = DockStyle.Fill
            };
            frame.Controls.Add(pnlOverlay, 0, 0);
            frame.SetRowSpan(pnlOverlay, 3);
            pnlOverlay.BringToFront();

            BuildEndDialog();

            // Timer
            timerNivel = new Timer { Interval = 1000 };
            timerNivel.Tick += TimerNivel_Tick;

            ApplyResponsiveLayout();
            FixDockOrder();
            ResumeLayout(true);
        }

        private void FixDockOrder()
        {
            // ✅ con frame de 3 filas ya no hay solapes; solo aseguras overlay arriba
            pnlOverlay?.BringToFront();
        }

        private void Header_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (var br = new LinearGradientBrush(pnlHeader.ClientRectangle, Header1, Header2, LinearGradientMode.Horizontal))
                e.Graphics.FillRectangle(br, pnlHeader.ClientRectangle);

            // línea inferior
            using (var p = new Pen(Color.FromArgb(40, 255, 255, 255), 1))
                e.Graphics.DrawLine(p, 0, pnlHeader.Height - 1, pnlHeader.Width, pnlHeader.Height - 1);
        }

        private Label Stat(string t)
        {
            return new Label
            {
                Text = t,
                Font = fStats,
                ForeColor = Color.White,
                AutoSize = true,
                Margin = new Padding(18, 0, 0, 0)
            };
        }

        private Button MakeButton(string text, Color back, Color fore)
        {
            var b = new Button
            {
                Text = text,
                BackColor = back,
                ForeColor = fore,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Height = 44,
                Cursor = Cursors.Hand
            };
            b.FlatAppearance.BorderSize = 0;
            b.Paint += (s, e) => ApplyRoundedRegion(b, 16);

            b.MouseEnter += (s, e) => b.BackColor = Shift(back, 10);
            b.MouseLeave += (s, e) => b.BackColor = back;
            return b;
        }

        private static Color Shift(Color c, int delta)
        {
            int r = Math.Max(0, Math.Min(255, c.R + delta));
            int g = Math.Max(0, Math.Min(255, c.G + delta));
            int b = Math.Max(0, Math.Min(255, c.B + delta));
            return Color.FromArgb(c.A, r, g, b);
        }

        // =========================================================
        // RESPONSIVE LAYOUT
        // =========================================================
        private void ApplyResponsiveLayout()
        {
            if (root == null) return;

            // ✅ un poquito más compacto (sube todo un poco)
            if (pnlClient != null) pnlClient.Padding = new Padding(0, 8, 0, 10);
            root.Padding = new Padding(16, 8, 16, 10);

            // ✅ header: que no quede “caído”
            if (lblTitulo != null)
            {
                lblTitulo.Font = new Font("Segoe UI", 14.5f, FontStyle.Bold);
                lblTitulo.Margin = new Padding(0, 0, 0, 0);
            }
            foreach (var lab in new[] { lblPuntaje, lblErrores, lblTiempo, lblNivel })
            {
                if (lab == null) continue;
                lab.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
                lab.Margin = new Padding(14, 0, 0, 0);
            }

            // ✅ barra: más arriba + margen correcto
            if (progress != null)
            {
                progress.Height = 14;
                progress.Margin = new Padding(0, 0, 0, 10);
                progress.ForeColor = Color.FromArgb(31, 41, 55);
            }

            bool narrow = ClientSize.Width < 980;

            if (!narrow && root.ColumnCount != 2)
            {
                root.SuspendLayout();
                var panels = root.Controls.OfType<Panel>().ToList();
                Panel left = panels.Count > 0 ? panels[0] : null;
                Panel right = panels.Count > 1 ? panels[1] : null;

                root.Controls.Clear();
                root.ColumnStyles.Clear();
                root.RowStyles.Clear();

                root.ColumnCount = 2;
                root.RowCount = 1;
                root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34));
                root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 66));
                root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

                if (left != null) { left.Padding = new Padding(0, 0, 10, 0); root.Controls.Add(left, 0, 0); }
                if (right != null) { right.Padding = new Padding(10, 0, 0, 0); root.Controls.Add(right, 1, 0); }

                root.ResumeLayout(true);
            }
            else if (narrow && root.ColumnCount != 1)
            {
                root.SuspendLayout();
                var panels = root.Controls.OfType<Panel>().ToList();
                Panel left = panels.Count > 0 ? panels[0] : null;
                Panel right = panels.Count > 1 ? panels[1] : null;

                root.Controls.Clear();
                root.ColumnStyles.Clear();
                root.RowStyles.Clear();

                root.ColumnCount = 1;
                root.RowCount = 2;
                root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
                root.RowStyles.Add(new RowStyle(SizeType.Percent, 46));
                root.RowStyles.Add(new RowStyle(SizeType.Percent, 54));

                if (left != null) { left.Padding = new Padding(0, 0, 0, 12); root.Controls.Add(left, 0, 0); }
                if (right != null) { right.Padding = new Padding(0); root.Controls.Add(right, 0, 1); }

                root.ResumeLayout(true);
            }

            LayoutPoolCards(); // ✅ aplica compacto siempre
        }

        private void LayoutPoolCards()
        {
            if (flpPool == null) return;

            int usableW = flpPool.ClientSize.Width - flpPool.Padding.Left - flpPool.Padding.Right;
            if (flpPool.VerticalScroll.Visible) usableW -= SystemInformation.VerticalScrollBarWidth;
            if (usableW < 200) return;

            // ✅ NO full width: más minimal
            int cardW = Math.Min(330, usableW - 24);
            cardW = Math.Max(220, cardW);

            int side = Math.Max(0, (usableW - cardW) / 2);
            int vGap = 10;

            foreach (var card in flpPool.Controls.OfType<CardChip>())
            {
                card.Margin = new Padding(side, vGap / 2, side, vGap / 2);
                card.FitToWidth(cardW);
            }
        }

        // =========================================================
        // GAME
        // =========================================================
        private void StartGame()
        {
            juegoTerminado = false;
            errores = 0;
            puntaje = 0;
            tarjetasCorrectas = 0;
            totalTarjetas = 0;

            LoadRecord();

            var cards = GenerateCardsForLevel(nivelActual);
            totalTarjetas = cards.Count;

            int baseTime = (nivelActual == 1) ? 45 : 40;
            tiempoRestante = baseTime + cards.Count;

            progress.Maximum = Math.Max(1, totalTarjetas);
            progress.Value = 0;

            // clear
            flpPool.Controls.Clear();
            zoneNaturaleza.ClearCards();
            zoneCultura.ClearCards();
            zoneGastro.ClearCards();

            // add
            foreach (var c in cards)
                flpPool.Controls.Add(CreateCard(c.Texto, c.Categoria));

            LayoutPoolCards();
            UpdateStats();

            timerNivel.Stop();
            timerNivel.Start();

            if (!tutorialMostrado)
            {
                tutorialMostrado = true;
                timerNivel.Stop();
                using (var f = new HowToPlayForm()) f.ShowDialog(this);
                if (!juegoTerminado) timerNivel.Start();
            }
        }

        private List<(string Texto, string Categoria)> GenerateCardsForLevel(int nivel)
        {
            int porCategoria = (nivel == 1) ? CardsPerCategoryNivel1 : CardsPerCategoryNivel2;
            var list = new List<(string Texto, string Categoria)>();

            foreach (var cat in CATEGORIAS)
            {
                var origen = bancoTarjetas.ContainsKey(cat) ? bancoTarjetas[cat] : new List<string>();
                if (origen.Count == 0) continue;

                int take = Math.Min(porCategoria, origen.Count);
                list.AddRange(origen.OrderBy(_ => rnd.Next()).Take(take).Select(x => (x, cat)));
            }

            // seguridad
            if (list.Count == 0)
            {
                // fallback mínimo para no romper
                list.Add(("Tarjeta demo", "Naturaleza"));
            }

            return list.OrderBy(_ => rnd.Next()).ToList();
        }

        private CardChip CreateCard(string texto, string categoria)
        {
            var chip = new CardChip(texto, categoria, CardBg, CardHover, Border);
            chip.BeginDrag += (s, e) =>
            {
                if (juegoTerminado) return;
                chip.DoDragDrop(chip, DragDropEffects.Move);
            };
            return chip;
        }

        private void TimerNivel_Tick(object sender, EventArgs e)
        {
            if (juegoTerminado) return;

            tiempoRestante--;
            if (tiempoRestante < 0) tiempoRestante = 0;

            UpdateStats();

            if (tiempoRestante == 0)
            {
                EndGame(EndReason.TimeOut);
            }
        }

        private async void OnDropRequested(object sender, DropRequestEventArgs e)
        {
            if (juegoTerminado) return;
            if (e == null || e.Card == null) return;

            var card = e.Card;
            string destino = e.TargetCategory;
            string correcta = card.Categoria;

            if (!IsValidCategory(destino) || !IsValidCategory(correcta))
                return;

            bool ok = (destino == correcta);

            if (ok)
            {
                puntaje += 10;
                tarjetasCorrectas++;

                progress.Value = Math.Min(progress.Maximum, tarjetasCorrectas);

                // mover
                SafeRemoveFromAnyParent(card);
                e.TargetZone.AddCard(card);
                card.LockAsCorrect(Success);

                UpdateStats();

                await e.TargetZone.FlashAsync(true);

                CheckWinCondition();
            }
            else
            {
                errores++;
                puntaje = Math.Max(0, puntaje - 5);
                UpdateStats();

                await e.TargetZone.FlashAsync(false);
                await card.FlashWrongAsync(Color.FromArgb(255, 230, 230), CardBg);

                if (errores >= maxErrores)
                    EndGame(EndReason.TooManyErrors);
            }
        }

        private void CheckWinCondition()
        {
            if (tarjetasCorrectas < totalTarjetas) return;

            timerNivel.Stop();

            int bonus = tiempoRestante * bonusPorSegundo;
            puntaje += bonus;
            SaveRecordIfNeeded();
            UpdateStats();

            if (nivelActual == 1)
            {
                nivelActual = 2;
                MessageBox.Show("🔥 Nivel 1 completado. ¡Vamos al Nivel 2!", "Siguiente nivel",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                StartGame();
                return;
            }

            EndGame(EndReason.WonAll);
        }

        private void EndGame(EndReason reason)
        {
            if (juegoTerminado) return;
            juegoTerminado = true;

            timerNivel.Stop();
            SaveRecordIfNeeded();

            string title, subtitle, symbol;
            Color iconColor;

            switch (reason)
            {
                case EndReason.WonAll:
                    title = "¡Increíble! 🎉";
                    subtitle = "Completaste todos los niveles.";
                    iconColor = Success;
                    symbol = "✓";
                    break;

                case EndReason.TimeOut:
                    title = "Se acabó el tiempo ⏰";
                    subtitle = $"Nivel {nivelActual} terminado por tiempo.";
                    iconColor = Warn;
                    symbol = "!";
                    break;

                case EndReason.TooManyErrors:
                    title = "Demasiados errores ❌";
                    subtitle = $"Nivel {nivelActual} terminado. Intenta otra vez.";
                    iconColor = Danger;
                    symbol = "✕";
                    break;

                default:
                    title = "Fin del juego";
                    subtitle = "Intenta otra vez.";
                    iconColor = GrayBtn;
                    symbol = "!";
                    break;
            }

            endTitle.Text = title;
            endSubtitle.Text = subtitle;
            endScore.Text = $"Puntaje: {puntaje}\nRécord: {puntajeMaximo}";

            // ✅ Estado del icono (color + símbolo)
            endDialog.Tag = new object[] { iconColor, symbol };

            // ✅ Overlay SIEMPRE encima de TODO el Form
            if (pnlOverlay.Parent != this)
            {
                try { pnlOverlay.Parent?.Controls.Remove(pnlOverlay); } catch { }
                Controls.Add(pnlOverlay);
            }
            pnlOverlay.Dock = DockStyle.Fill;
            pnlOverlay.Bounds = this.ClientRectangle;
            pnlOverlay.Visible = true;
            pnlOverlay.BringToFront();

            // ✅ Bloquea interacción debajo (por si algo se “cuelan” clicks)
            pnlHeader.Enabled = false;
            pnlClient.Enabled = false;
            pnlFooter.Enabled = false;

            CenterEndDialog();
            endDialog.Focus();
        }

        private void UpdateStats()
        {
            if (lblPuntaje == null) return;
            lblPuntaje.Text = $"Puntaje: {puntaje}";
            lblErrores.Text = $"Errores: {errores}/{maxErrores}";
            lblTiempo.Text = $"Tiempo: {tiempoRestante}s";
            lblNivel.Text = $"Nivel: {nivelActual}";
        }

        private bool IsValidCategory(string c)
        {
            return !string.IsNullOrWhiteSpace(c) && CATEGORIAS.Contains(c);
        }

        private static void SafeRemoveFromAnyParent(Control ctrl)
        {
            if (ctrl?.Parent == null) return;
            try { ctrl.Parent.Controls.Remove(ctrl); } catch { }
        }

        // =========================================================
        // RECORD
        // =========================================================
        private void LoadRecord()
        {
            try
            {
                if (File.Exists(rutaArchivoRecord))
                    int.TryParse(File.ReadAllText(rutaArchivoRecord), out puntajeMaximo);
                else
                    puntajeMaximo = 0;
            }
            catch { puntajeMaximo = 0; }
        }

        private void SaveRecordIfNeeded()
        {
            try
            {
                if (puntaje > puntajeMaximo)
                {
                    puntajeMaximo = puntaje;
                    File.WriteAllText(rutaArchivoRecord, puntajeMaximo.ToString());
                }
            }
            catch { }
        }

        // =========================================================
        // END DIALOG
        // =========================================================
        private void BuildEndDialog()
        {
            endDialog = new RoundedPanel
            {
                Size = new Size(520, 320),
                Radius = 22,
                FillColor = Color.White,
                BorderColor = Border,
                BorderWidth = 1,
                Shadow = true,
                Padding = new Padding(18),
                Anchor = AnchorStyles.None
            };
            pnlOverlay.Controls.Add(endDialog);

            var grid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 6
            };
            grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 66));  // icon
            grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));  // title
            grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));  // subtitle
            grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 70));  // score
            grid.RowStyles.Add(new RowStyle(SizeType.Percent, 10));  // spacer
            grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 65));  // buttons
            endDialog.Controls.Add(grid);

            var icon = new Panel { Dock = DockStyle.Fill };
            icon.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                Color col = Accent;
                string sym = "✓";

                if (endDialog.Tag is object[] arr && arr.Length >= 2)
                {
                    if (arr[0] is Color c) col = c;
                    if (arr[1] is string ss && !string.IsNullOrWhiteSpace(ss)) sym = ss;
                }

                using (var br = new SolidBrush(col))
                {
                    var r = new Rectangle((icon.Width - 44) / 2, (icon.Height - 44) / 2, 44, 44);
                    e.Graphics.FillEllipse(br, r);
                }

                using (var br = new SolidBrush(Color.White))
                using (var f = new Font("Segoe UI", 16, FontStyle.Bold))
                {
                    var sz = e.Graphics.MeasureString(sym, f);
                    e.Graphics.DrawString(sym, f, br,
                        (icon.Width - sz.Width) / 2,
                        (icon.Height - sz.Height) / 2);
                }
            };

            endTitle = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };
            endSubtitle = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleCenter
            };
            endScore = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(17, 24, 39),
                TextAlign = ContentAlignment.MiddleCenter
            };

            var btnRow = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(6, 6, 6, 0)
            };
            btnRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            btnRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            endPlayAgain = MakeButton("Volver a jugar", Accent, Color.White);
            endExit = MakeButton("Salir", Color.Firebrick, Color.White);

            endPlayAgain.Dock = DockStyle.Fill;
            endExit.Dock = DockStyle.Fill;

            endPlayAgain.Click += (s, e) =>
            {
                pnlOverlay.Visible = false;

                // ✅ re-habilita todo
                pnlHeader.Enabled = true;
                pnlClient.Enabled = true;
                pnlFooter.Enabled = true;

                nivelActual = 1;
                StartGame();
            };

            endExit.Click += (s, e) =>
            {
                SaveRecordIfNeeded();
                var f = new FrmActividadesTuristicas();
                f.Show();
                Dispose();
            };

            btnRow.Controls.Add(endPlayAgain, 0, 0);
            btnRow.Controls.Add(endExit, 1, 0);

            grid.Controls.Add(icon, 0, 0);
            grid.Controls.Add(endTitle, 0, 1);
            grid.Controls.Add(endSubtitle, 0, 2);
            grid.Controls.Add(endScore, 0, 3);
            grid.Controls.Add(new Panel(), 0, 4);
            grid.Controls.Add(btnRow, 0, 5);
        }
        private void CenterEndDialog()
        {
            if (endDialog == null || pnlOverlay == null) return;
            endDialog.Location = new Point(
                (pnlOverlay.ClientSize.Width - endDialog.Width) / 2,
                (pnlOverlay.ClientSize.Height - endDialog.Height) / 2
            );
        }

        // =========================================================
        // HELPERS: Rounded Region
        // =========================================================
        private static void ApplyRoundedRegion(Control c, int radius)
        {
            if (c.Width < 10 || c.Height < 10) return;
            using (var gp = RoundedRect(new Rectangle(0, 0, c.Width, c.Height), radius))
                c.Region = new Region(gp);
        }

        private static GraphicsPath RoundedRect(Rectangle r, int radius)
        {
            int d = radius * 2;
            var path = new GraphicsPath();
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        // =========================================================
        // NESTED TYPES
        // =========================================================
        private enum EndReason { WonAll, TimeOut, TooManyErrors }

        private class DropRequestEventArgs : EventArgs
        {
            public CardChip Card { get; }
            public string TargetCategory { get; }
            public DropZone TargetZone { get; }

            public DropRequestEventArgs(CardChip card, string targetCategory, DropZone targetZone)
            {
                Card = card;
                TargetCategory = targetCategory;
                TargetZone = targetZone;
            }
        }

        private class DropZone : RoundedPanel
        {
            public event EventHandler<DropRequestEventArgs> DropRequested;

            public string Categoria { get; }
            private readonly FlowLayoutPanel flp;
            private readonly Color accent;
            private readonly Label title;
            private readonly Label hint;

            public DropZone(string categoria, string titleText, Color accentColor, Color border)
            {
                Categoria = categoria;
                accent = accentColor;

                Radius = 18;
                FillColor = Color.White;
                BorderColor = border;
                BorderWidth = 1;
                Shadow = true;

                // ✅ más compacto
                Padding = new Padding(16);

                AllowDrop = true;

                title = new Label
                {
                    Text = titleText,
                    Dock = DockStyle.Top,
                    Height = 24,
                    Font = new Font("Segoe UI", 11.5f, FontStyle.Bold),
                    ForeColor = Color.FromArgb(17, 24, 39)
                };

                hint = new Label
                {
                    Text = "Arrastra aquí las tarjetas correctas",
                    Dock = DockStyle.Top,
                    Height = 16,
                    Font = new Font("Segoe UI", 9f, FontStyle.Regular),
                    ForeColor = Color.FromArgb(107, 114, 128)
                };

                var bar = new Panel { Dock = DockStyle.Left, Width = 6, BackColor = accent };
                Controls.Add(bar);

                flp = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    AutoScroll = true,
                    WrapContents = false,
                    FlowDirection = FlowDirection.TopDown,
                    BackColor = Color.Transparent,

                    // ✅ padding interno para que no toque bordes
                    Padding = new Padding(14, 10, 14, 10)
                };

                Controls.Add(flp);
                Controls.Add(hint);
                Controls.Add(title);

                DragEnter += Zone_DragEnter;
                DragDrop += Zone_DragDrop;

                flp.AllowDrop = true;
                flp.DragEnter += Zone_DragEnter;
                flp.DragDrop += Zone_DragDrop;

                title.AllowDrop = true;
                hint.AllowDrop = true;
                title.DragEnter += Zone_DragEnter;
                hint.DragEnter += Zone_DragEnter;
                title.DragDrop += Zone_DragDrop;
                hint.DragDrop += Zone_DragDrop;
            }

            private void Zone_DragEnter(object sender, DragEventArgs e)
            {
                if (e.Data.GetDataPresent(typeof(CardChip)))
                    e.Effect = DragDropEffects.Move;
            }

            private void Zone_DragDrop(object sender, DragEventArgs e)
            {
                var card = e.Data.GetData(typeof(CardChip)) as CardChip;
                if (card == null) return;
                DropRequested?.Invoke(this, new DropRequestEventArgs(card, Categoria, this));
            }

            public void AddCard(CardChip card)
            {
                if (card == null) return;

                int w = flp.ClientSize.Width - flp.Padding.Left - flp.Padding.Right;

                if (flp.VerticalScroll.Visible)
                    w -= SystemInformation.VerticalScrollBarWidth;

                // ✅ margen extra por sombra/borde (para que no “se salga” visualmente)
                w -= 14;

                w = Math.Max(200, w);

                card.Margin = new Padding(0, 0, 0, 10);
                card.FitToWidth(w);
                flp.Controls.Add(card);
            }

            public void ClearCards() => flp.Controls.Clear();

            public async Task FlashAsync(bool ok)
            {
                var old = FillColor;
                FillColor = ok ? Color.FromArgb(236, 253, 245) : Color.FromArgb(254, 242, 242);
                Invalidate();
                await Task.Delay(220);
                FillColor = old;
                Invalidate();
            }
        }

        private class CardChip : RoundedPanel
        {
            public string Categoria { get; }
            public event EventHandler BeginDrag;

            private readonly Color baseColor;
            private readonly Color hoverColor;
            private readonly Label lbl;

            public CardChip(string text, string categoria, Color baseColor, Color hoverColor, Color border)
            {
                Categoria = categoria;
                this.baseColor = baseColor;
                this.hoverColor = hoverColor;

                Radius = 16;
                FillColor = baseColor;
                BorderColor = border;
                BorderWidth = 1;
                Shadow = true;

                Height = 60;
                Width = 260;
                Padding = new Padding(12, 10, 12, 10);
                Cursor = Cursors.Hand;

                lbl = new Label
                {
                    Text = text,
                    Font = new Font("Segoe UI", 10.2f, FontStyle.Bold),
                    ForeColor = Color.FromArgb(17, 24, 39),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    AutoSize = false,
                    AutoEllipsis = false
                };
                Controls.Add(lbl);

                MouseEnter += (_, __) => { if (Enabled) { FillColor = hoverColor; Invalidate(); } };
                MouseLeave += (_, __) => { if (Enabled) { FillColor = baseColor; Invalidate(); } };

                MouseDown += (_, __) => { if (Enabled) BeginDrag?.Invoke(this, EventArgs.Empty); };
                lbl.MouseDown += (_, __) => { if (Enabled) BeginDrag?.Invoke(this, EventArgs.Empty); };
            }

            // ✅ Ajusta altura según texto (no se corta)
            public void FitToWidth(int width)
            {
                Width = width;

                int textW = Math.Max(120, width - Padding.Left - Padding.Right);
                var sz = TextRenderer.MeasureText(
                    lbl.Text,
                    lbl.Font,
                    new Size(textW, 9999),
                    TextFormatFlags.WordBreak | TextFormatFlags.HorizontalCenter
                );

                // ✅ menos “aire” para que sea minimal
                int desiredH = sz.Height + Padding.Top + Padding.Bottom + 8;
                Height = Math.Max(52, desiredH);

                lbl.MaximumSize = new Size(textW, 0);
                Invalidate();
            }


            public void LockAsCorrect(Color okColor)
            {
                Enabled = false;
                Cursor = Cursors.Default;
                FillColor = Color.FromArgb(236, 253, 245);
                BorderColor = okColor;
                BorderWidth = 2;
                Invalidate();
            }

            public async Task FlashWrongAsync(Color wrong, Color back)
            {
                FillColor = wrong;
                Invalidate();
                await Task.Delay(220);
                FillColor = back;
                Invalidate();
            }
        }

        private class RoundedPanel : Panel
        {
            public int Radius { get; set; } = 16;
            public Color FillColor { get; set; } = Color.White;
            public Color BorderColor { get; set; } = Color.Gainsboro;
            public int BorderWidth { get; set; } = 1;
            public bool Shadow { get; set; } = false;

            public RoundedPanel()
            {
                SetStyle(ControlStyles.AllPaintingInWmPaint |
                         ControlStyles.OptimizedDoubleBuffer |
                         ControlStyles.ResizeRedraw |
                         ControlStyles.UserPaint, true);
                BackColor = Color.Transparent;
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                var rect = new Rectangle(0, 0, Width - 1, Height - 1);
                var shadowRect = new Rectangle(2, 4, Width - 5, Height - 5);

                if (Shadow)
                {
                    using (var gpS = RoundedRect(shadowRect, Radius))
                    using (var brS = new SolidBrush(Color.FromArgb(25, 0, 0, 0)))
                        e.Graphics.FillPath(brS, gpS);
                }

                using (var gp = RoundedRect(rect, Radius))
                using (var br = new SolidBrush(FillColor))
                    e.Graphics.FillPath(br, gp);

                if (BorderWidth > 0)
                {
                    using (var gp = RoundedRect(rect, Radius))
                    using (var p = new Pen(BorderColor, BorderWidth))
                        e.Graphics.DrawPath(p, gp);
                }

                base.OnPaint(e);
            }

            private static GraphicsPath RoundedRect(Rectangle r, int radius)
            {
                int d = radius * 2;
                var path = new GraphicsPath();
                path.AddArc(r.X, r.Y, d, d, 180, 90);
                path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
                path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
                path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
                path.CloseFigure();
                return path;
            }
        }

        private class ProgressBarEx : Control
        {
            public int Maximum { get; set; } = 1;
            private int _value = 0;
            public int Value
            {
                get => _value;
                set { _value = Math.Max(0, Math.Min(Maximum, value)); Invalidate(); }
            }

            public Color TrackColor { get; set; } = Color.Gainsboro;
            public Color Fill1 { get; set; } = Color.RoyalBlue;
            public Color Fill2 { get; set; } = Color.MediumSeaGreen;

            public ProgressBarEx()
            {
                SetStyle(ControlStyles.AllPaintingInWmPaint |
                         ControlStyles.OptimizedDoubleBuffer |
                         ControlStyles.ResizeRedraw |
                         ControlStyles.UserPaint, true);
                ForeColor = Color.FromArgb(31, 41, 55);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                var rect = new Rectangle(0, 0, Width - 1, Height - 1);

                using (var gp = RoundedRect(rect, 10))
                using (var br = new SolidBrush(TrackColor))
                    e.Graphics.FillPath(br, gp);

                float pct = (Maximum <= 0) ? 0 : (float)Value / Maximum;
                pct = Math.Max(0, Math.Min(1, pct));
                int w = (int)(rect.Width * pct);

                if (w > 0)
                {
                    var fillRect = new Rectangle(0, 0, w, rect.Height);
                    using (var br = new LinearGradientBrush(fillRect, Fill1, Fill2, LinearGradientMode.Horizontal))
                    using (var gpFill = RoundedRect(new Rectangle(0, 0, w, rect.Height), 10))
                        e.Graphics.FillPath(br, gpFill);
                }

                // ✅ texto %
                int percent = (int)Math.Round(pct * 100f);
                string txt = percent + "%";
                using (var f = new Font("Segoe UI", 9f, FontStyle.Bold))
                using (var brTxt = new SolidBrush(ForeColor))
                {
                    var sz = e.Graphics.MeasureString(txt, f);
                    e.Graphics.DrawString(txt, f, brTxt,
                        (Width - sz.Width) / 2f,
                        (Height - sz.Height) / 2f);
                }

                base.OnPaint(e);
            }

            private static GraphicsPath RoundedRect(Rectangle r, int radius)
            {
                int d = radius * 2;
                var path = new GraphicsPath();
                path.AddArc(r.X, r.Y, d, d, 180, 90);
                path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
                path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
                path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
                path.CloseFigure();
                return path;
            }
        }
    }
}
