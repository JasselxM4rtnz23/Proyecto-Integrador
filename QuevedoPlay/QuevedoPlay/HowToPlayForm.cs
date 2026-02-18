using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace QuevedoPlay
{
    public partial class HowToPlayForm : Form
    {
        // ====== Tema ======
        private readonly Color fondo = Color.FromArgb(245, 248, 252);
        private readonly Color card = Color.White;
        private readonly Color borde = Color.FromArgb(220, 230, 240);

        private readonly Color titleColor = Color.FromArgb(30, 50, 80);
        private readonly Color textColor = Color.FromArgb(45, 45, 45);
        private readonly Color subTextColor = Color.DimGray;

        private Panel cardPanel;
        private Panel scroll;
        private FlowLayoutPanel content;
        private Button btnClose;
        private Label note;

        public HowToPlayForm()
        {
            Text = "Cómo jugar";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            ClientSize = new Size(680, 540);
            BackColor = fondo;
            DoubleBuffered = true;

            BuildUI();

            Shown += (s, e) => ApplyLayout();
            ResizeEnd += (s, e) => ApplyLayout();
        }

        private void BuildUI()
        {
            // ====== Título (centrado) ======
            var title = new Label
            {
                Text = "🧩 ¿Cómo jugar?",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = titleColor,
                AutoSize = true,
                BackColor = Color.Transparent
            };
            Controls.Add(title);

            var subtitle = new Label
            {
                Text = "Arrastra y suelta las tarjetas en la categoría correcta",
                Font = new Font("Segoe UI", 10.5f, FontStyle.Regular),
                ForeColor = subTextColor,
                AutoSize = true,
                BackColor = Color.Transparent
            };
            Controls.Add(subtitle);

            // ====== Card (se centra en ApplyLayout) ======
            cardPanel = new Panel
            {
                BackColor = card,
                Size = new Size(600, 340),
                Padding = new Padding(16)
            };
            cardPanel.Paint += (s, e) => DrawCardBorder(e.Graphics, cardPanel, 18);
            Controls.Add(cardPanel);

            // ====== Scroll interno ======
            scroll = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.Transparent,
                Padding = new Padding(6, 6, 18, 6) // espacio para scrollbar
            };
            cardPanel.Controls.Add(scroll);

            // ====== Contenido con jerarquía visual ======
            content = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                WrapContents = false,
                FlowDirection = FlowDirection.TopDown,
                BackColor = Color.Transparent,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            scroll.Controls.Add(content);

            BuildPrettyContent();

            // Ajustar max width de labels al cambiar tamaño (evita corte raro)
            scroll.SizeChanged += (s, e) => FixLabelWidths();

            // ====== Botón ======
            btnClose = new Button
            {
                Text = "Entendido ✅",
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(170, 46),
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => Close();
            btnClose.Paint += (s, e) => RoundButtonRegion(btnClose, 16);
            Controls.Add(btnClose);

            // ====== Nota inferior ======
            note = new Label
            {
                Text = "Tip: Esta ventana pausa el juego mientras lees.",
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = Color.DimGray,
                AutoSize = true,
                BackColor = Color.Transparent
            };
            Controls.Add(note);
        }

        // =========================================================
        // Contenido bonito (jerarquía + chips + bullets)
        // =========================================================
        private void BuildPrettyContent()
        {
            content.Controls.Clear();

            content.Controls.Add(H("Objetivo 🎯"));
            content.Controls.Add(P("Clasifica cada tarjeta en su categoría correcta. Si aciertas, sumas puntos y avanzas al siguiente nivel."));

            content.Controls.Add(Spacer(6));
            content.Controls.Add(H("Cómo se juega 🖱️"));
            content.Controls.Add(Bullet("1)", "Mantén clic izquierdo presionado sobre una tarjeta."));
            content.Controls.Add(Bullet("2)", "Arrástrala hacia el contenedor correcto (Naturaleza / Cultura / Gastronomía)."));
            content.Controls.Add(Bullet("3)", "Suéltala dentro del contenedor para validar."));

            content.Controls.Add(Spacer(6));
            content.Controls.Add(H("Reglas rápidas ⚡"));

            var chipsRow = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 2, 0, 8),
                Padding = new Padding(0)
            };

            chipsRow.Controls.Add(Chip("✅ +10", "Correcto", Color.FromArgb(235, 250, 240), Color.FromArgb(22, 120, 60)));
            chipsRow.Controls.Add(Chip("❌ -5", "Incorrecto", Color.FromArgb(255, 235, 235), Color.FromArgb(160, 40, 40)));
            chipsRow.Controls.Add(Chip("🚫 5", "Máx. errores", Color.FromArgb(245, 245, 245), Color.FromArgb(70, 70, 70)));
            chipsRow.Controls.Add(Chip("⏱️ Bonus", "Por tiempo", Color.FromArgb(235, 245, 255), Color.FromArgb(0, 90, 160)));

            content.Controls.Add(chipsRow);

            content.Controls.Add(Spacer(6));
            content.Controls.Add(H("Tips para ganar más 🔥"));
            content.Controls.Add(Bullet("💡", "Lee la tarjeta completa antes de soltarla."));
            content.Controls.Add(Bullet("👀", "Fíjate en el título del contenedor: te ahorra errores tontos."));
            content.Controls.Add(Bullet("🏁", "Mientras más rápido termines, más bonus de tiempo recibes."));

            FixLabelWidths();
        }

        private Control Spacer(int h) => new Panel { Height = h, Width = 10, BackColor = Color.Transparent, Margin = new Padding(0) };

        private Label H(string text) => new Label
        {
            Text = text,
            AutoSize = true,
            Font = new Font("Segoe UI", 12, FontStyle.Bold),
            ForeColor = titleColor,
            Margin = new Padding(0, 4, 0, 6)
        };

        private Label P(string text) => new Label
        {
            Text = text,
            AutoSize = true,
            Font = new Font("Segoe UI", 10.5f, FontStyle.Regular),
            ForeColor = textColor,
            Margin = new Padding(0, 0, 0, 6)
        };

        private Label Bullet(string icon, string text) => new Label
        {
            Text = $"{icon}  {text}",
            AutoSize = true,
            Font = new Font("Segoe UI", 10.5f, FontStyle.Regular),
            ForeColor = textColor,
            Margin = new Padding(10, 2, 0, 6)
        };

        private Panel Chip(string big, string small, Color back, Color fore)
        {
            var p = new Panel
            {
                BackColor = back,
                AutoSize = true,
                Padding = new Padding(10, 7, 10, 7),
                Margin = new Padding(0, 4, 10, 6)
            };

            var lbl = new Label
            {
                AutoSize = true,
                ForeColor = fore,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Text = $"{big}  {small}"
            };

            p.Controls.Add(lbl);
            p.Paint += (s, e) => RoundPanelRegion(p, 14);
            return p;
        }

        private void FixLabelWidths()
        {
            int maxW = Math.Max(200, scroll.ClientSize.Width - 18);

            foreach (Control c in content.Controls)
            {
                if (c is Label lb)
                    lb.MaximumSize = new Size(maxW, 0);

                // si hay fila de chips, no tocar
            }
        }

        // =========================================================
        // Layout centrado (jerarquía visual)
        // =========================================================
        private void ApplyLayout()
        {
            int topPad = 18;

            // Título y subtítulo centrados
            var title = Controls[0] as Label;      // 🧩 ¿Cómo jugar?
            var subtitle = Controls[1] as Label;   // texto pequeño

            if (title != null)
                title.Location = new Point((ClientSize.Width - title.PreferredWidth) / 2, topPad);

            if (subtitle != null)
                subtitle.Location = new Point((ClientSize.Width - subtitle.PreferredWidth) / 2, topPad + 42);

            // Card centrada
            int cardW = Math.Min(620, ClientSize.Width - 60);
            int cardH = Math.Min(360, ClientSize.Height - 190);

            cardPanel.Size = new Size(cardW, cardH);
            cardPanel.Location = new Point((ClientSize.Width - cardPanel.Width) / 2, topPad + 78);

            // Botón centrado abajo
            btnClose.Location = new Point((ClientSize.Width - btnClose.Width) / 2, cardPanel.Bottom + 14);

            // Nota centrada
            note.Location = new Point((ClientSize.Width - note.PreferredWidth) / 2, btnClose.Bottom + 10);

            FixLabelWidths();
        }

        // =========================================================
        // Dibujo / bordes / redondeos
        // =========================================================
        private void DrawCardBorder(Graphics g, Control c, int radius)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (var path = RoundedRectPath(new Rectangle(1, 1, c.Width - 3, c.Height - 3), radius))
            using (var pen = new Pen(borde, 2))
                g.DrawPath(pen, path);
        }

        private GraphicsPath RoundedRectPath(Rectangle r, int radius)
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

        private void RoundButtonRegion(Control c, int radius)
        {
            if (c.Width < 10 || c.Height < 10) return;
            using (var path = RoundedRectPath(new Rectangle(0, 0, c.Width, c.Height), radius))
                c.Region = new Region(path);
        }

        private void RoundPanelRegion(Panel p, int radius)
        {
            if (p.Width < 10 || p.Height < 10) return;
            using (var path = RoundedRectPath(new Rectangle(0, 0, p.Width, p.Height), radius))
                p.Region = new Region(path);
        }
    }
}
