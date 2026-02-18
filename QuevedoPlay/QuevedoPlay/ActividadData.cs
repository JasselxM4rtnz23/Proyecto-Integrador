using System;
using System.Drawing;

namespace QuevedoPlay
{
    public class ActividadData
    {
        public string Titulo { get; set; }
        public string Descripcion { get; set; }

        public string Card1Titulo { get; set; }
        public string Card2Titulo { get; set; }
        public string Card3Titulo { get; set; }

        public string Sub1 { get; set; }
        public string Sub2 { get; set; }
        public string Sub3 { get; set; }

        public string TipTitulo { get; set; }
        public string TipTexto { get; set; }

        public Image Img1 { get; set; }
        public Image Img2 { get; set; }
        public Image Img3 { get; set; }

        public string ModalTitulo1 { get; set; }
        public string ModalQueEs1 { get; set; }
        public string ModalAprendes1 { get; set; }
        public string ModalTip1 { get; set; }

        public string ModalTitulo2 { get; set; }
        public string ModalQueEs2 { get; set; }
        public string ModalAprendes2 { get; set; }
        public string ModalTip2 { get; set; }

        public string ModalTitulo3 { get; set; }
        public string ModalQueEs3 { get; set; }
        public string ModalAprendes3 { get; set; }
        public string ModalTip3 { get; set; }

        public string[] ModalLugares1 { get; set; } = Array.Empty<string>();
        public string[] ModalLugares2 { get; set; } = Array.Empty<string>();
        public string[] ModalLugares3 { get; set; } = Array.Empty<string>();

    }
}
