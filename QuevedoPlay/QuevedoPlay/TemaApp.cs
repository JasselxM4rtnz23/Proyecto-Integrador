using System.Drawing;

namespace QuevedoPlay.Estilos
{
    public static class TemaApp
    {
        // 🎨 COLORES BASE
        public static Color FondoPrincipal = Color.FromArgb(245, 247, 250);
        public static Color FondoPanel = Color.White;

        // 🎯 IDENTIDAD
        public static Color Primario = Color.FromArgb(34, 139, 230);   // Azul
        public static Color Secundario = Color.FromArgb(46, 204, 113); // Verde
        public static Color Acento = Color.FromArgb(255, 159, 67);     // Naranja

        // ✍️ TEXTO
        public static Color TextoPrincipal = Color.FromArgb(33, 33, 33);
        public static Color TextoSecundario = Color.FromArgb(120, 120, 120);

        // 🔘 BOTONES
        public static Color BotonPrincipal = Primario;
        public static Color BotonTexto = Color.White;
    }
}
