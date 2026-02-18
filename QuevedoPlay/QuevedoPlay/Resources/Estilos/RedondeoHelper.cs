using System.Drawing;
using System.Windows.Forms;

namespace QuevedoPlay.Estilos
{
    public static class RedondeoHelper
    {
        public static void Aplicar(Control control, int radio)
        {
            control.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                var path = new System.Drawing.Drawing2D.GraphicsPath();
                path.AddArc(0, 0, radio, radio, 180, 90);
                path.AddArc(control.Width - radio, 0, radio, radio, 270, 90);
                path.AddArc(control.Width - radio, control.Height - radio, radio, radio, 0, 90);
                path.AddArc(0, control.Height - radio, radio, radio, 90, 90);
                path.CloseAllFigures();

                control.Region = new Region(path);
            };
        }
    }
}
