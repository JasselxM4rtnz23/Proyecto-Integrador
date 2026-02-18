using System.Drawing;

namespace QuevedoPlay
{
    public class Carta
    {
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public Image Imagen { get; set; }

        public Carta(string titulo, string descripcion, Image imagen)
        {
            Titulo = titulo;
            Descripcion = descripcion;
            Imagen = imagen;
        }
    }
}
