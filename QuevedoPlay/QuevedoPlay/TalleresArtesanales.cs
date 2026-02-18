using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Media;
using System.Windows.Forms;

namespace QuevedoPlay
{
    public partial class FrmActividadDetalle : Form
    {
        private bool _initialized = false;
        private TableLayoutPanel tlpTip;
        private PictureBox picTipIcon;

        // ====== Colores ======
        private readonly Color Danger = Color.Firebrick;
        private readonly Color Primary = Color.SteelBlue;

        private ActividadData _data;

        // Anti doble click fantasma
        private int _lastModalCloseTick = 0;
        private const int MODAL_GUARD_MS = 250;

        // Cache imágenes
        private readonly Dictionary<string, Image> _imgCache =
            new Dictionary<string, Image>(StringComparer.OrdinalIgnoreCase);

        // ====== Layout containers ======
        private TableLayoutPanel rootLayout;     // 2 filas: contenido + footer fijo
        private Panel pnlRoot;                   // scrolleable
        private Panel pnlFooter;                 // fijo abajo

        private Panel contentHost;               // centrado dentro del scroll
        private TableLayoutPanel contentLayout;  // header + cards + tip

        private TableLayoutPanel tlpHeader;
        private TableLayoutPanel tlpCards;

        private SoundPlayer _clickSound;

        // ====== Constantes UI ======
        private const int ROOT_PAD_X = 24;
        private const int ROOT_PAD_Y = 18;
        private const int FOOTER_H = 90;
        private const int MAX_CONTENT_W = 1100;
        private const int CARD_IMG_H = 240;
        private const int CARD_RADIUS = 22;
        private const int TIP_RADIUS = 18;

        // =========================
        // CONSTRUCTORES
        // =========================
        public FrmActividadDetalle()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();

            DoubleBuffered = true;

            Load += FrmActividadDetalle_Load;
            Shown += FrmActividadDetalle_Shown;
            FormClosed += FrmActividadDetalle_FormClosed;
        }

        public FrmActividadDetalle(ActividadData data) : this()
        {
            _data = data;
        }

        // Si el diseñador te enganchó a esto, no rompe nada:
        private void FrmActividadDetalle_Load_1(object sender, EventArgs e)
        {
            FrmActividadDetalle_Load(sender, e);
        }

        // =========================
        // LOAD / INIT
        // =========================
        private void FrmActividadDetalle_Load(object sender, EventArgs e)
        {
            if (_initialized) return;
            _initialized = true;

            if (_data == null)
                _data = ActividadBuilders.BuildDefaultTalleres();

            EnsureLayoutStructure();
            ApplyTheme();
            ApplyDataToUI(_data);

            EnsureTipIcon(); // ✅ Tip con icono + borde

            // Cards: orden interno correcto + imagen grande
            PrepareCard(panel1, pic1, label3, lblSub1);
            PrepareCard(panel2, pic2, label4, lblSub2);
            PrepareCard(panel3, pic3, label5, lblSub3);

            // Hover + modal PRO (con lugares + imagen)
            WireCard(panel1, 1, _data.ModalTitulo1, _data.ModalQueEs1, _data.ModalAprendes1, _data.ModalTip1);
            WireCard(panel2, 2, _data.ModalTitulo2, _data.ModalQueEs2, _data.ModalAprendes2, _data.ModalTip2);
            WireCard(panel3, 3, _data.ModalTitulo3, _data.ModalQueEs3, _data.ModalAprendes3, _data.ModalTip3);

            SetupCloseButton();

            // Redondeo automático por tamaño
            AttachRound(panel1, CARD_RADIUS);
            AttachRound(panel2, CARD_RADIUS);
            AttachRound(panel3, CARD_RADIUS);
            AttachRound(pnlTip, TIP_RADIUS);
            AttachRound(btnRegresar, CARD_RADIUS);

            // Sonido
            try { _clickSound = new SoundPlayer(Properties.Resources.clicwav); }
            catch { _clickSound = null; }

            // Ajustes responsivos iniciales
            UpdateContentWidth();
        }

        // =========================
        // TIP ICON + BORDE
        // =========================
        private void EnsureTipIcon()
        {
            if (pnlTip == null || lblTipTitulo == null || lblTipTexto == null) return;

            if (tlpTip == null)
            {
                tlpTip = new TableLayoutPanel
                {
                    Dock = DockStyle.Top,
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    ColumnCount = 2,
                    RowCount = 2,
                    Margin = new Padding(0),
                    Padding = new Padding(0)
                };
                tlpTip.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 46));
                tlpTip.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
                tlpTip.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                tlpTip.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            }

            if (picTipIcon == null)
            {
                picTipIcon = new PictureBox
                {
                    Width = 32,
                    Height = 32,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Margin = new Padding(0, 2, 10, 0),
                    Image = GetTipIcon()
                };
            }
            else if (picTipIcon.Image == null)
            {
                picTipIcon.Image = GetTipIcon();
            }

            pnlTip.Controls.Remove(lblTipTitulo);
            pnlTip.Controls.Remove(lblTipTexto);
            pnlTip.Controls.Remove(tlpTip);

            tlpTip.Controls.Clear();
            tlpTip.Controls.Add(picTipIcon, 0, 0);
            tlpTip.SetRowSpan(picTipIcon, 2);
            tlpTip.Controls.Add(lblTipTitulo, 1, 0);
            tlpTip.Controls.Add(lblTipTexto, 1, 1);

            lblTipTitulo.Dock = DockStyle.Fill;
            lblTipTexto.Dock = DockStyle.Fill;
            lblTipTitulo.TextAlign = ContentAlignment.MiddleLeft;
            lblTipTexto.TextAlign = ContentAlignment.MiddleLeft;

            lblTipTitulo.AutoSize = true;
            lblTipTexto.AutoSize = true;

            pnlTip.Controls.Add(tlpTip);

            pnlTip.Paint -= PnlTip_PaintBorder;
            pnlTip.Paint += PnlTip_PaintBorder;
        }

        private Image GetTipIcon()
        {
            var custom = LoadImg("Tip.png");
            if (custom != null) return custom;

            return SystemIcons.Information.ToBitmap();
        }

        private void PnlTip_PaintBorder(object sender, PaintEventArgs e)
        {
            var p = (Panel)sender;
            var rect = new Rectangle(0, 0, p.Width - 1, p.Height - 1);

            using (var path = RoundedRect(rect, 18))
            using (var pen = new Pen(Color.FromArgb(220, 220, 220)))
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawPath(pen, path);
            }
        }

        private GraphicsPath RoundedRect(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            int d = radius * 2;
            if (d > rect.Width) d = rect.Width;
            if (d > rect.Height) d = rect.Height;

            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        // =========================
        // SHOWN / CLOSED
        // =========================
        private void FrmActividadDetalle_Shown(object sender, EventArgs e)
        {
            UpdateContentWidth();
            Redondear(panel1, CARD_RADIUS);
            Redondear(panel2, CARD_RADIUS);
            Redondear(panel3, CARD_RADIUS);
            Redondear(pnlTip, TIP_RADIUS);
            Redondear(btnRegresar, CARD_RADIUS);
        }

        private void FrmActividadDetalle_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (var kv in _imgCache)
            {
                try { kv.Value?.Dispose(); } catch { }
            }
            _imgCache.Clear();

            try { _clickSound?.Dispose(); } catch { }
        }

        // =========================
        // LAYOUT DEFINITIVO
        // =========================
        private void EnsureLayoutStructure()
        {
            SuspendLayout();

            if (rootLayout == null)
            {
                rootLayout = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 1,
                    RowCount = 2,
                    BackColor = Color.FromArgb(245, 247, 250)
                };
                rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
                rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, FOOTER_H));

                Controls.Add(rootLayout);
            }

            if (pnlRoot == null)
            {
                pnlRoot = new Panel
                {
                    Dock = DockStyle.Fill,
                    AutoScroll = true,
                    BackColor = Color.FromArgb(245, 247, 250),
                    Padding = new Padding(ROOT_PAD_X, ROOT_PAD_Y, ROOT_PAD_X, ROOT_PAD_Y)
                };
                rootLayout.Controls.Add(pnlRoot, 0, 0);

                pnlRoot.Resize += (s, e) => UpdateContentWidth();
            }

            if (pnlFooter == null)
            {
                pnlFooter = new Panel
                {
                    Dock = DockStyle.Fill,
                    Padding = new Padding(ROOT_PAD_X, 14, ROOT_PAD_X, 14),
                    BackColor = Color.FromArgb(245, 247, 250)
                };
                rootLayout.Controls.Add(pnlFooter, 0, 1);

                pnlFooter.Paint += (s, e) =>
                {
                    using (var pen = new Pen(Color.FromArgb(220, 220, 220)))
                        e.Graphics.DrawLine(pen, 0, 0, pnlFooter.Width, 0);
                };

                if (btnRegresar != null)
                {
                    btnRegresar.Parent = pnlFooter;
                    btnRegresar.Dock = DockStyle.Left;
                    btnRegresar.Width = 190;
                    btnRegresar.Height = 46;
                }
            }

            if (contentHost == null)
            {
                contentHost = new Panel
                {
                    Parent = pnlRoot,
                    Dock = DockStyle.Top,
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    BackColor = Color.Transparent
                };
            }

            if (contentLayout == null)
            {
                contentLayout = new TableLayoutPanel
                {
                    Parent = contentHost,
                    Dock = DockStyle.Top,
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    ColumnCount = 1,
                    RowCount = 3,
                    Margin = new Padding(0),
                    Padding = new Padding(0)
                };
                contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            }

            if (tlpHeader == null)
            {
                tlpHeader = new TableLayoutPanel
                {
                    Dock = DockStyle.Top,
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    ColumnCount = 1,
                    RowCount = 2,
                    Margin = new Padding(0, 0, 0, 16)
                };
                tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
                contentLayout.Controls.Add(tlpHeader, 0, 0);
            }

            if (tlpCards == null)
            {
                tlpCards = new TableLayoutPanel
                {
                    Dock = DockStyle.Top,
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    Margin = new Padding(0, 0, 0, 18),
                    Padding = new Padding(0),
                    GrowStyle = TableLayoutPanelGrowStyle.FixedSize
                };
                contentLayout.Controls.Add(tlpCards, 0, 1);
            }

            if (pnlTip != null)
            {
                pnlTip.Parent = contentLayout;
                pnlTip.Dock = DockStyle.Top;
                pnlTip.Margin = new Padding(0, 0, 0, 10);
                pnlTip.AutoSize = true;
                pnlTip.AutoSizeMode = AutoSizeMode.GrowAndShrink;

                if (!contentLayout.Controls.Contains(pnlTip))
                    contentLayout.Controls.Add(pnlTip, 0, 2);
            }

            tlpHeader.SuspendLayout();
            tlpHeader.Controls.Clear();

            if (label1 != null)
            {
                label1.Parent = tlpHeader;
                label1.Anchor = AnchorStyles.None;
                label1.Margin = new Padding(0, 6, 0, 6);
                tlpHeader.Controls.Add(label1, 0, 0);
            }

            if (lblDescripcion != null)
            {
                lblDescripcion.Parent = tlpHeader;
                lblDescripcion.Anchor = AnchorStyles.None;
                lblDescripcion.Margin = new Padding(0, 0, 0, 0);
                tlpHeader.Controls.Add(lblDescripcion, 0, 1);
            }

            tlpHeader.ResumeLayout();

            UpdateCardsLayout();

            if (button1 != null)
            {
                button1.Parent = this;
                button1.BringToFront();
            }

            ResumeLayout();
        }

        // =========================
        // RESPONSIVE + CENTRADO
        // =========================
        private void UpdateContentWidth()
        {
            if (pnlRoot == null || contentHost == null) return;

            int available = pnlRoot.ClientSize.Width - pnlRoot.Padding.Horizontal;
            if (available < 520) available = Math.Max(320, pnlRoot.ClientSize.Width - 8);

            int w = Math.Min(MAX_CONTENT_W, available);
            contentHost.Width = w;
            contentHost.Left = Math.Max(0, (pnlRoot.ClientSize.Width - w) / 2);

            int wrapW = Math.Min(900, Math.Max(400, w - 80));

            if (lblDescripcion != null)
            {
                lblDescripcion.MaximumSize = new Size(wrapW, 0);
                lblDescripcion.AutoSize = true;
            }

            if (lblTipTexto != null)
            {
                lblTipTexto.MaximumSize = new Size(wrapW, 0);
                lblTipTexto.AutoSize = true;
            }

            UpdateCardsLayout();
        }

        private void UpdateCardsLayout()
        {
            if (tlpCards == null) return;

            bool singleColumn = contentHost != null && contentHost.Width < 980;

            tlpCards.SuspendLayout();
            tlpCards.Controls.Clear();
            tlpCards.ColumnStyles.Clear();
            tlpCards.RowStyles.Clear();

            if (singleColumn)
            {
                tlpCards.ColumnCount = 1;
                tlpCards.RowCount = 3;
                tlpCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
                tlpCards.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                tlpCards.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                tlpCards.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                if (panel1 != null) { panel1.Parent = tlpCards; panel1.Dock = DockStyle.Fill; tlpCards.Controls.Add(panel1, 0, 0); }
                if (panel2 != null) { panel2.Parent = tlpCards; panel2.Dock = DockStyle.Fill; tlpCards.Controls.Add(panel2, 0, 1); }
                if (panel3 != null) { panel3.Parent = tlpCards; panel3.Dock = DockStyle.Fill; tlpCards.Controls.Add(panel3, 0, 2); }
            }
            else
            {
                tlpCards.ColumnCount = 3;
                tlpCards.RowCount = 1;
                tlpCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333f));
                tlpCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333f));
                tlpCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333f));
                tlpCards.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                if (panel1 != null) { panel1.Parent = tlpCards; panel1.Dock = DockStyle.Fill; tlpCards.Controls.Add(panel1, 0, 0); }
                if (panel2 != null) { panel2.Parent = tlpCards; panel2.Dock = DockStyle.Fill; tlpCards.Controls.Add(panel2, 1, 0); }
                if (panel3 != null) { panel3.Parent = tlpCards; panel3.Dock = DockStyle.Fill; tlpCards.Controls.Add(panel3, 2, 0); }
            }

            tlpCards.ResumeLayout();
        }

        // =========================
        // THEME
        // =========================
        private void ApplyTheme()
        {
            BackColor = Color.FromArgb(245, 247, 250);

            if (label1 != null)
            {
                label1.ForeColor = Color.Black;
                label1.Font = new Font("Segoe UI", 28, FontStyle.Bold);
                label1.AutoSize = true;
                label1.TextAlign = ContentAlignment.MiddleCenter;
            }

            if (lblDescripcion != null)
            {
                lblDescripcion.Font = new Font("Segoe UI", 12, FontStyle.Regular);
                lblDescripcion.ForeColor = Color.FromArgb(80, 80, 80);
                lblDescripcion.TextAlign = ContentAlignment.MiddleCenter;
                lblDescripcion.AutoSize = true;
                lblDescripcion.Padding = new Padding(10, 0, 10, 0);
            }

            if (pnlTip != null)
            {
                pnlTip.BackColor = Color.FromArgb(245, 246, 248);
                pnlTip.Padding = new Padding(18);
                pnlTip.AutoSize = true;
                pnlTip.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            }

            if (lblTipTitulo != null)
            {
                lblTipTitulo.Font = new Font("Segoe UI", 12, FontStyle.Bold);
                lblTipTitulo.ForeColor = Color.Black;
                lblTipTitulo.Dock = DockStyle.Top;
                lblTipTitulo.AutoSize = true;
            }

            if (lblTipTexto != null)
            {
                lblTipTexto.Font = new Font("Segoe UI", 11, FontStyle.Regular);
                lblTipTexto.ForeColor = Color.DimGray;
                lblTipTexto.Dock = DockStyle.Top;
                lblTipTexto.AutoSize = true;
                lblTipTexto.Padding = new Padding(0, 10, 0, 0);
            }

            if (btnRegresar != null)
            {
                btnRegresar.BackColor = Danger;
                btnRegresar.ForeColor = Color.White;
                btnRegresar.FlatStyle = FlatStyle.Flat;
                btnRegresar.FlatAppearance.BorderSize = 0;
                btnRegresar.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
                btnRegresar.Cursor = Cursors.Hand;
            }
        }

        private void SetupCloseButton()
        {
            if (button1 == null) return;

            button1.Parent = this;
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button1.Width = 48;
            button1.Height = 40;
            button1.Left = ClientSize.Width - button1.Width - 18;
            button1.Top = 16;

            button1.BackColor = Color.Red;
            button1.ForeColor = Color.White;
            button1.FlatStyle = FlatStyle.Flat;
            button1.FlatAppearance.BorderSize = 0;
            button1.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            button1.Text = "X";
            button1.Cursor = Cursors.Hand;

            Resize += (s, ev) =>
            {
                button1.Left = ClientSize.Width - button1.Width - 18;
                button1.BringToFront();
            };

            button1.BringToFront();
        }

        // =========================
        // DATA -> UI
        // =========================
        private void ApplyDataToUI(ActividadData d)
        {
            if (d == null) return;

            if (label1 != null) label1.Text = d.Titulo ?? "";
            if (lblDescripcion != null) lblDescripcion.Text = d.Descripcion ?? "";

            if (label3 != null) label3.Text = d.Card1Titulo ?? "";
            if (label4 != null) label4.Text = d.Card2Titulo ?? "";
            if (label5 != null) label5.Text = d.Card3Titulo ?? "";

            if (lblSub1 != null) lblSub1.Text = d.Sub1 ?? "";
            if (lblSub2 != null) lblSub2.Text = d.Sub2 ?? "";
            if (lblSub3 != null) lblSub3.Text = d.Sub3 ?? "";

            if (lblTipTitulo != null)
                lblTipTitulo.Text = string.IsNullOrWhiteSpace(d.TipTitulo) ? "✅ Recomendación general" : d.TipTitulo;

            if (lblTipTexto != null) lblTipTexto.Text = d.TipTexto ?? "";

            if (pic1 != null) pic1.Image = d.Img1;
            if (pic2 != null) pic2.Image = d.Img2;
            if (pic3 != null) pic3.Image = d.Img3;
        }

        // =========================
        // CARD UI
        // =========================
        private void PrepareCard(Panel card, PictureBox pic, Label title, Label sub)
        {
            if (card == null || pic == null || title == null || sub == null) return;

            card.BackColor = Color.White;
            card.Padding = new Padding(18);
            card.Margin = new Padding(12);
            card.Dock = DockStyle.Fill;
            card.Cursor = Cursors.Hand;

            if (pic.Parent != card) pic.Parent = card;
            if (title.Parent != card) title.Parent = card;
            if (sub.Parent != card) sub.Parent = card;

            pic.Dock = DockStyle.Top;
            pic.Height = CARD_IMG_H;
            pic.SizeMode = PictureBoxSizeMode.Zoom;

            title.Dock = DockStyle.Top;
            title.AutoSize = false;
            title.Height = 42;
            title.TextAlign = ContentAlignment.MiddleCenter;
            title.Font = new Font("Segoe UI", 15, FontStyle.Bold);
            title.Padding = new Padding(0, 10, 0, 0);

            sub.Dock = DockStyle.Top;
            sub.AutoSize = false;
            sub.Height = 34;
            sub.TextAlign = ContentAlignment.MiddleCenter;
            sub.Font = new Font("Segoe UI", 11, FontStyle.Regular);
            sub.ForeColor = Color.DimGray;
            sub.Padding = new Padding(8, 6, 8, 0);

            card.Controls.Remove(sub);
            card.Controls.Remove(title);
            card.Controls.Remove(pic);

            card.Controls.Add(sub);
            card.Controls.Add(title);
            card.Controls.Add(pic);
        }

        // ✅ Card + Modal PRO: lugares + imagen + Maps
        private void WireCard(Panel card, int index, string titulo, string queEs, string queAprendes, string tip)
        {
            if (card == null) return;

            Color normal = Color.White;
            Color hover = Color.FromArgb(248, 249, 251);

            void OnEnter() => card.BackColor = hover;
            void OnLeave() => card.BackColor = normal;

            card.MouseEnter += (s, e) => OnEnter();
            card.MouseLeave += (s, e) => OnLeave();

            foreach (Control c in card.Controls)
            {
                c.MouseEnter += (s, e) => OnEnter();
                c.MouseLeave += (s, e) => OnLeave();
            }

            void ClickHandler()
            {
                int now = Environment.TickCount;
                if (now - _lastModalCloseTick < MODAL_GUARD_MS) return;

                Image hero = FindHeroImage(index);
                var lugares = GetModalLugares(index);

                ShowInfoModalPro(
                    titulo ?? "Información",
                    hero,
                    queEs ?? "",
                    queAprendes ?? "",
                    tip ?? "",
                    lugares
                );
            }

            card.Click += (s, e) => ClickHandler();
            foreach (Control c in card.Controls) c.Click += (s, e) => ClickHandler();
        }

        private Image FindHeroImage(int index)
        {
            if (index == 1 && pic1 != null && pic1.Image != null) return pic1.Image;
            if (index == 2 && pic2 != null && pic2.Image != null) return pic2.Image;
            if (index == 3 && pic3 != null && pic3.Image != null) return pic3.Image;
            return SystemIcons.Information.ToBitmap();
        }

        // Soporta:
        // - ModalLugares1/2/3 (string[] o List<string>)
        // - ModalDonde1/2/3 (string con saltos de línea)
        private List<string> GetModalLugares(int index)
        {
            var result = new List<string>();

            if (_data == null) return result;

            string propArray = $"ModalLugares{index}";
            string propText = $"ModalDonde{index}";

            try
            {
                var t = _data.GetType();

                var pArr = t.GetProperty(propArray);
                if (pArr != null)
                {
                    object val = pArr.GetValue(_data);

                    if (val is string[] arr)
                    {
                        foreach (var s in arr)
                            if (!string.IsNullOrWhiteSpace(s)) result.Add(s.Trim());
                        return result;
                    }

                    if (val is List<string> list)
                    {
                        foreach (var s in list)
                            if (!string.IsNullOrWhiteSpace(s)) result.Add(s.Trim());
                        return result;
                    }
                }

                var pTxt = t.GetProperty(propText);
                if (pTxt != null)
                {
                    var txt = pTxt.GetValue(_data) as string;
                    if (!string.IsNullOrWhiteSpace(txt))
                    {
                        var lines = txt.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var line in lines)
                        {
                            var clean = line.Replace("•", "").Trim();
                            if (!string.IsNullOrWhiteSpace(clean)) result.Add(clean);
                        }
                    }
                }
            }
            catch { }

            return result;
        }

        private void OpenMapsSearch(string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return;

            string url = "https://www.google.com/maps/search/?api=1&query=" + Uri.EscapeDataString(query);
            try
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch { }
        }

        // =========================
        // MODAL PRO (imagen + lugares + maps)
        // =========================
        private void ShowInfoModalPro(string titulo, Image hero, string queEs, string queAprendes, string tip, List<string> lugares)
        {
            using (Form dlg = new Form())
            {
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.MaximizeBox = false;
                dlg.MinimizeBox = false;
                dlg.ShowInTaskbar = false;
                dlg.ShowIcon = false;
                dlg.ControlBox = false;
                dlg.FormBorderStyle = FormBorderStyle.FixedSingle;
                dlg.BackColor = Color.White;
                dlg.ClientSize = new Size(920, 560);

                // ROOT
                var root = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 1,
                    RowCount = 3
                };
                root.RowStyles.Add(new RowStyle(SizeType.Absolute, 72));   // header
                root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));   // body
                root.RowStyles.Add(new RowStyle(SizeType.Absolute, 72));   // footer
                dlg.Controls.Add(root);

                // HEADER
                var header = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
                header.Paint += (s, e) =>
                {
                    using (var pen = new Pen(Color.FromArgb(225, 225, 225)))
                        e.Graphics.DrawLine(pen, 0, header.Height - 1, header.Width, header.Height - 1);
                };

                var lblTitle = new Label
                {
                    Text = titulo,
                    Dock = DockStyle.Fill,
                    Font = new Font("Segoe UI", 18, FontStyle.Bold),
                    ForeColor = Color.Black,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding = new Padding(18, 0, 0, 0)
                };

                var btnClose = new Button
                {
                    Text = "X",
                    Width = 46,
                    Height = 36,
                    BackColor = Color.FromArgb(245, 245, 245),
                    ForeColor = Color.FromArgb(70, 70, 70),
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 11, FontStyle.Bold),
                    Cursor = Cursors.Hand,
                    Anchor = AnchorStyles.Top | AnchorStyles.Right
                };
                btnClose.FlatAppearance.BorderSize = 0;

                header.Controls.Add(lblTitle);
                header.Controls.Add(btnClose);
                root.Controls.Add(header, 0, 0);

                header.Resize += (s, e) =>
                {
                    btnClose.Left = header.Width - btnClose.Width - 16;
                    btnClose.Top = (header.Height - btnClose.Height) / 2;
                };

                btnClose.Click += (s, e) => dlg.Close();

                // BODY SCROLL
                var bodyScroll = new Panel
                {
                    Dock = DockStyle.Fill,
                    AutoScroll = true,
                    Padding = new Padding(18, 16, 18, 16),
                    BackColor = Color.White
                };
                root.Controls.Add(bodyScroll, 0, 1);

                var body = new TableLayoutPanel
                {
                    Dock = DockStyle.Top,
                    AutoSize = true,
                    ColumnCount = 2
                };
                body.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 310));
                body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
                bodyScroll.Controls.Add(body);

                // LEFT (imagen + lugares)
                var left = new Panel
                {
                    Dock = DockStyle.Top,
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    Padding = new Padding(0, 0, 14, 0)
                };

                var pic = new PictureBox
                {
                    Width = 290,
                    Height = 260,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Image = hero ?? SystemIcons.Information.ToBitmap()
                };
                left.Controls.Add(pic);

                var grp = new GroupBox
                {
                    Text = "📍 Dónde queda en Quevedo",
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    Padding = new Padding(10),
                    Top = pic.Bottom + 12,
                    Width = 300
                };

                var flpL = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    FlowDirection = FlowDirection.TopDown,
                    WrapContents = false,
                    AutoScroll = false,
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink
                };
                grp.Controls.Add(flpL);

                if (lugares == null || lugares.Count == 0)
                {
                    var none = new Label
                    {
                        Text = "Agrega ModalLugares" + " (o ModalDonde) en tu data.\nAquí se mostrarán lugares reales.",
                        AutoSize = true,
                        ForeColor = Color.DimGray,
                        Font = new Font("Segoe UI", 10, FontStyle.Regular),
                        MaximumSize = new Size(270, 0)
                    };
                    flpL.Controls.Add(none);
                }
                else
                {
                    foreach (var lugar in lugares)
                    {
                        var link = new LinkLabel
                        {
                            Text = "• " + lugar,
                            AutoSize = true,
                            LinkColor = Primary,
                            Font = new Font("Segoe UI", 10, FontStyle.Regular),
                            MaximumSize = new Size(270, 0)
                        };
                        string q = lugar;
                        link.LinkClicked += (s, e) => OpenMapsSearch(q);
                        flpL.Controls.Add(link);
                    }
                }

                left.Controls.Add(grp);

                // Ajuste vertical
                left.Resize += (s, e) =>
                {
                    grp.Top = pic.Bottom + 12;
                    grp.Width = left.Width;
                };

                body.Controls.Add(left, 0, 0);

                // RIGHT (bloques)
                var right = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    FlowDirection = FlowDirection.TopDown,
                    WrapContents = false,
                    AutoScroll = false,
                    AutoSize = true
                };
                body.Controls.Add(right, 1, 0);

                Panel Block(string subtitulo, string texto)
                {
                    var p = new Panel
                    {
                        Width = 540,
                        BackColor = Color.FromArgb(245, 246, 248),
                        Padding = new Padding(14),
                        Margin = new Padding(0, 0, 0, 14)
                    };

                    var t = new Label
                    {
                        Text = subtitulo,
                        Font = new Font("Segoe UI", 11, FontStyle.Bold),
                        ForeColor = Color.Black,
                        AutoSize = true
                    };

                    var b = new Label
                    {
                        Text = texto,
                        Font = new Font("Segoe UI", 10),
                        ForeColor = Color.DimGray,
                        AutoSize = true,
                        MaximumSize = new Size(510, 0),
                        Padding = new Padding(0, 8, 0, 0)
                    };

                    p.Controls.Add(t);
                    p.Controls.Add(b);
                    b.Top = t.Bottom;
                    b.Left = 0;
                    p.Height = b.Bottom + 10;

                    return p;
                }

                right.Controls.Add(Block("¿Qué es esta actividad?", queEs));
                right.Controls.Add(Block("¿Qué aprende el visitante?", queAprendes));
                right.Controls.Add(Block("Recomendaciones para el visitante", tip));

                // FOOTER
                var footer = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
                footer.Paint += (s, e) =>
                {
                    using (var pen = new Pen(Color.FromArgb(225, 225, 225)))
                        e.Graphics.DrawLine(pen, 0, 0, footer.Width, 0);
                };
                root.Controls.Add(footer, 0, 2);

                var btnOk = new Button
                {
                    Text = "Entendido",
                    Width = 170,
                    Height = 44,
                    BackColor = Primary,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };
                btnOk.FlatAppearance.BorderSize = 0;
                footer.Controls.Add(btnOk);

                footer.Resize += (s, e) =>
                {
                    btnOk.Left = 18;
                    btnOk.Top = (footer.Height - btnOk.Height) / 2;
                };

                btnOk.Click += (s, e) => dlg.Close();
                dlg.AcceptButton = btnOk;

                dlg.ShowDialog(this);
                _lastModalCloseTick = Environment.TickCount;
            }
        }

        // =========================
        // REDONDEO
        // =========================
        private void AttachRound(Control c, int radius)
        {
            if (c == null) return;
            c.SizeChanged += (s, e) => Redondear(c, radius);
        }

        private void Redondear(Control control, int radio)
        {
            if (control == null) return;
            if (control.Width <= radio || control.Height <= radio) return;

            using (GraphicsPath path = new GraphicsPath())
            {
                int r = radio;

                path.StartFigure();
                path.AddArc(0, 0, r, r, 180, 90);
                path.AddArc(control.Width - r, 0, r, r, 270, 90);
                path.AddArc(control.Width - r, control.Height - r, r, r, 0, 90);
                path.AddArc(0, control.Height - r, r, r, 90, 90);
                path.CloseFigure();

                control.Region = new Region(path);
            }
        }

        // =========================
        // IMG LOADER
        // =========================
        private Image LoadImg(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return null;

            if (_imgCache.TryGetValue(fileName, out var cached))
                return cached;

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string path = Path.Combine(baseDir, "Resources", "Imagenes", fileName);

            if (!File.Exists(path)) return null;

            try
            {
                using (var temp = Image.FromFile(path))
                {
                    var bmp = new Bitmap(temp);
                    _imgCache[fileName] = bmp;
                    return bmp;
                }
            }
            catch
            {
                return null;
            }
        }

        // =========================
        // BUILDERS (tus nombres)
        // =========================
        public ActividadData BuildCaminatas()
        {
            return new ActividadData
            {
                Titulo = "Caminatas Guiadas",
                Descripcion =
                    "Las caminatas guiadas en Quevedo permiten recorrer áreas naturales y zonas rurales cercanas al cantón,\n" +
                    "fomentando el contacto con la naturaleza y la educación ambiental.",

                Card1Titulo = "Ruta Natural",
                Card2Titulo = "Avistamiento",
                Card3Titulo = "Fotografía",

                Sub1 = "Recorrido por áreas naturales.",
                Sub2 = "Observación de fauna local.",
                Sub3 = "Registro visual del entorno.",

                TipTitulo = "✅ Recomendación general",
                TipTexto = "Lleva agua, usa calzado adecuado y sigue siempre las indicaciones del guía durante el recorrido.",

                Img1 = LoadImg("RutaNatural.png"),
                Img2 = LoadImg("Avistamiento.png"),
                Img3 = LoadImg("Fotografia.png"),

                ModalTitulo1 = "Ruta Natural",
                ModalQueEs1 = "Senderos y recorridos para conocer paisajes y ecosistemas.",
                ModalAprendes1 = "Aprendes: orientación básica, cuidado del entorno y puntos de interés.",
                ModalTip1 = "Tip: no dejes basura y respeta la flora.",

                ModalTitulo2 = "Avistamiento",
                ModalQueEs2 = "Observación de aves y animales en su hábitat.",
                ModalAprendes2 = "Aprendes: paciencia, identificación básica y respeto por la fauna.",
                ModalTip2 = "Tip: evita ruidos fuertes y no alimentes animales.",

                ModalTitulo3 = "Fotografía",
                ModalQueEs3 = "Captura imágenes del recorrido: paisajes, detalles y cultura.",
                ModalAprendes3 = "Aprendes: encuadre, luz y contar historias con fotos.",
                ModalTip3 = "Tip: pide permiso antes de fotografiar personas."
            };
        }

        public ActividadData BuildVisitas()
        {
            return new ActividadData
            {
                Titulo = "Visitas Guiadas",
                Descripcion =
                    "Las visitas guiadas permiten conocer espacios históricos y culturales de Quevedo,\n" +
                    "facilitando la comprensión de su desarrollo social e identidad local.",

                Card1Titulo = "Museo",
                Card2Titulo = "Centro Histórico",
                Card3Titulo = "Patrimonio",

                Sub1 = "Historia y memoria local.",
                Sub2 = "Espacios representativos.",
                Sub3 = "Tradiciones culturales.",

                TipTitulo = "✅ Recomendación general",
                TipTexto = "Respeta las normas de cada lugar y mantente atento a las indicaciones del guía.",

                Img1 = LoadImg("Museo.png"),
                Img2 = LoadImg("CentroHistorico.png"),
                Img3 = LoadImg("Patrimonio.png"),

                ModalTitulo1 = "Museo",
                ModalQueEs1 = "Espacio donde se conservan piezas y relatos de la historia local.",
                ModalAprendes1 = "Aprendes: contexto histórico y valor cultural.",
                ModalTip1 = "Tip: no tocar piezas y no usar flash si está prohibido.",

                ModalTitulo2 = "Centro Histórico",
                ModalQueEs2 = "Recorrido por calles y lugares que representan la memoria de la ciudad.",
                ModalAprendes2 = "Aprendes: arquitectura, historias y cambios del lugar.",
                ModalTip2 = "Tip: mantente con el grupo para no perderte.",

                ModalTitulo3 = "Patrimonio",
                ModalQueEs3 = "Tradiciones, edificios y símbolos que identifican a una comunidad.",
                ModalAprendes3 = "Aprendes: respeto por costumbres y significado cultural.",
                ModalTip3 = "Tip: pregunta antes de grabar o entrevistar."
            };
        }

        // =========================
        // NAV / BOTONES (tus nombres)
        // =========================
        private void btnRegresar_Click(object sender, EventArgs e)
        {
            try { _clickSound?.Play(); } catch { }

            FrmActividadesTuristicas opciones = new FrmActividadesTuristicas();
            opciones.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
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

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}
