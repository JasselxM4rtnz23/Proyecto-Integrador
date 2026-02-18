using System;
using System.Drawing;

namespace QuevedoPlay
{
    public static class ActividadBuilders
    {
        // =========================
        // TALLERES ARTESANALES
        // =========================
        public static ActividadData BuildDefaultTalleres()
        {
            return new ActividadData
            {
                Titulo = "Talleres Artesanales",
                Descripcion =
                    "Los talleres artesanales en el cantón Quevedo permiten al visitante conocer y valorar " +
                    "el trabajo manual como parte de la identidad cultural local, promoviendo el respeto por " +
                    "los saberes tradicionales y el apoyo a los artesanos de la comunidad.",

                Card1Titulo = "Cerámica",
                Card2Titulo = "Tejido",
                Card3Titulo = "Tallado en madera",

                Sub1 = "Trabajo manual con arcilla.",
                Sub2 = "Técnicas tradicionales con hilo.",
                Sub3 = "Elaboración de piezas en madera.",

                TipTitulo = "✅ Recomendación general",
                TipTexto =
                    "Apoya a los artesanos locales adquiriendo productos originales, respeta sus procesos de " +
                    "trabajo y solicita permiso antes de fotografiar o grabar.",

                Img1 = ImageLoader.Load("Ceramica.png"),
                Img2 = ImageLoader.Load("Tejido.png"),
                Img3 = ImageLoader.Load("TalladoMadera.png"),

                // ---- CERÁMICA ----
                ModalTitulo1 = "Cerámica artesanal",
                ModalQueEs1 =
                    "La cerámica artesanal en Quevedo consiste en la elaboración de objetos utilitarios y " +
                    "decorativos a partir de arcilla, aplicando técnicas manuales que forman parte del " +
                    "conocimiento tradicional transmitido entre generaciones.",

                ModalAprendes1 =
                    "El visitante aprende técnicas básicas de modelado, secado y decoración de piezas de " +
                    "cerámica, además de comprender el valor cultural y económico del trabajo artesanal en " +
                    "la comunidad local.",

                ModalTip1 =
                    "Utiliza ropa cómoda, sigue las indicaciones del artesano y respeta los tiempos del " +
                    "proceso de elaboración.",

                // ✅ DÓNDE QUEDA (Quevedo)
                ModalLugares1 = new[]
                {
                    "Malecón Eloy Alfaro, Quevedo (eventos/ferias)",
                    "Palacio de Cristal, Quevedo (zona cultural/visitas)",
                    "Parque Central de Quevedo (ferias y encuentro)",
                    "Parque Lineal San Camilo, Quevedo (zona pública)"
                },

                // ---- TEJIDO ----
                ModalTitulo2 = "Tejido tradicional",
                ModalQueEs2 =
                    "El tejido artesanal utiliza hilos y fibras para la elaboración de prendas y objetos, " +
                    "empleando técnicas tradicionales que reflejan la creatividad y la identidad cultural " +
                    "de la zona.",

                ModalAprendes2 =
                    "Se aprende el uso básico de materiales, la elaboración de patrones sencillos y la " +
                    "importancia del tejido como actividad cultural y económica.",

                ModalTip2 =
                    "Valora el trabajo manual y adquiere productos auténticos elaborados por artesanos locales.",

                // ✅ DÓNDE QUEDA (Quevedo)
                ModalLugares2 = new[]
                {
                    "Parque Central de Quevedo (ferias/stands artesanales)",
                    "Malecón Eloy Alfaro, Quevedo (eventos/ferias)",
                    "Parque Lineal San Camilo, Quevedo",
                    "Palacio de Cristal, Quevedo (zona cultural)"
                },

                // ---- TALLADO EN MADERA ----
                ModalTitulo3 = "Tallado en madera",
                ModalQueEs3 =
                    "El tallado en madera es una actividad artesanal que consiste en dar forma a la madera " +
                    "mediante herramientas manuales, creando objetos decorativos y funcionales.",

                ModalAprendes3 =
                    "El visitante conoce técnicas básicas de diseño, detalle y acabado, además de reconocer " +
                    "la habilidad y precisión necesarias en este oficio.",

                ModalTip3 =
                    "Evita tocar las piezas sin autorización y respeta el tiempo y esfuerzo del artesano.",

                // ✅ DÓNDE QUEDA (Quevedo)
                ModalLugares3 = new[]
                {
                    "Malecón Eloy Alfaro, Quevedo (eventos/ferias)",
                    "Palacio de Cristal, Quevedo",
                    "Parque Central de Quevedo",
                    "Parque Lineal San Camilo, Quevedo"
                }
            };
        }

        // =========================
        // CAMINATAS GUIADAS
        // =========================
        public static ActividadData BuildCaminatas()
        {
            return new ActividadData
            {
                Titulo = "Caminatas Guiadas",
                Descripcion =
                    "Las caminatas guiadas en Quevedo permiten recorrer áreas naturales y zonas rurales " +
                    "cercanas al cantón, fomentando el contacto con la naturaleza y la educación ambiental.",

                Card1Titulo = "Ruta Natural",
                Card2Titulo = "Avistamiento",
                Card3Titulo = "Fotografía",

                Sub1 = "Recorrido por áreas naturales.",
                Sub2 = "Observación de fauna local.",
                Sub3 = "Registro visual del entorno.",

                TipTitulo = "✅ Recomendación general",
                TipTexto =
                    "Lleva agua, usa calzado adecuado y sigue siempre las indicaciones del guía durante el recorrido.",

                Img1 = ImageLoader.Load("RutaNatural.png"),
                Img2 = ImageLoader.Load("Avistamiento.png"),
                Img3 = ImageLoader.Load("Fotografia.png"),

                // ---- RUTA NATURAL ----
                ModalTitulo1 = "Ruta natural",
                ModalQueEs1 =
                    "Las rutas naturales permiten recorrer senderos y espacios verdes cercanos a Quevedo, " +
                    "donde se aprecia la biodiversidad característica de la región.",

                ModalAprendes1 =
                    "Se aprende sobre la flora local, el entorno agrícola y la importancia de la conservación " +
                    "del medio ambiente.",

                ModalTip1 =
                    "Respeta la naturaleza y evita dejar basura durante el recorrido.",

                // ✅ DÓNDE QUEDA (Quevedo / parroquias)
                ModalLugares1 = new[]
                {
                    "Ruta del Río (parroquia San Cristóbal, Quevedo)",
                    "Parque Lineal San Camilo, Quevedo",
                    "Malecón Eloy Alfaro, Quevedo (paseo ribereño)",
                    "Ribera del río Quevedo (zonas de recorrido)"
                },

                // ---- AVISTAMIENTO ----
                ModalTitulo2 = "Avistamiento de fauna",
                ModalQueEs2 =
                    "El avistamiento consiste en la observación de aves y otras especies propias del entorno " +
                    "natural de Quevedo, sin alterar su hábitat.",

                ModalAprendes2 =
                    "El visitante aprende a identificar especies locales y a desarrollar respeto por la vida silvestre.",

                ModalTip2 =
                    "Evita ruidos fuertes y no intentes alimentar a los animales.",

                // ✅ DÓNDE QUEDA (Quevedo)
                ModalLugares2 = new[]
                {
                    "Ruta del Río (parroquia San Cristóbal, Quevedo)",
                    "Parque Lineal San Camilo, Quevedo",
                    "Riberas del río Quevedo (zonas tranquilas)",
                    "Malecón Eloy Alfaro, Quevedo (zona ribereña)"
                },

                // ---- FOTOGRAFÍA ----
                ModalTitulo3 = "Fotografía turística",
                ModalQueEs3 =
                    "La fotografía turística permite capturar paisajes, tradiciones y momentos representativos " +
                    "durante las caminatas guiadas.",

                ModalAprendes3 =
                    "Se aprenden técnicas básicas de encuadre y el uso responsable de la imagen como medio de registro cultural.",

                ModalTip3 =
                    "Solicita permiso antes de fotografiar a personas o espacios privados.",

                // ✅ DÓNDE QUEDA (Quevedo)
                ModalLugares3 = new[]
                {
                    "Malecón Eloy Alfaro, Quevedo",
                    "Palacio de Cristal, Quevedo",
                    "Parque Central de Quevedo",
                    "Puente de Quevedo (vistas/atardeceres)"
                }
            };
        }

        // =========================
        // VISITAS GUIADAS
        // =========================
        public static ActividadData BuildVisitas()
        {
            return new ActividadData
            {
                Titulo = "Visitas Guiadas",
                Descripcion =
                    "Las visitas guiadas permiten conocer espacios históricos y culturales de Quevedo, " +
                    "facilitando la comprensión de su desarrollo social e identidad local.",

                Card1Titulo = "Museo",
                Card2Titulo = "Centro Histórico",
                Card3Titulo = "Patrimonio",

                Sub1 = "Historia y memoria local.",
                Sub2 = "Espacios representativos.",
                Sub3 = "Tradiciones culturales.",

                TipTitulo = "✅ Recomendación general",
                TipTexto =
                    "Respeta las normas de cada lugar y mantente atento a las indicaciones del guía.",

                Img1 = ImageLoader.Load("Museo.png"),
                Img2 = ImageLoader.Load("CentroHistorico.png"),
                Img3 = ImageLoader.Load("Patrimonio.png"),

                // ---- MUSEO ----
                ModalTitulo1 = "Museo / espacio cultural",
                ModalQueEs1 =
                    "Espacios culturales donde se presentan exposiciones, memoria local y actividades artísticas " +
                    "relacionadas con la identidad de Quevedo.",

                ModalAprendes1 =
                    "El visitante aprende sobre la historia local, expresiones culturales y el valor del patrimonio " +
                    "de la ciudad.",

                ModalTip1 =
                    "Pregunta por horarios y reglas (fotografía/flash) antes de ingresar.",

                // ✅ DÓNDE QUEDA (Quevedo)
                ModalLugares1 = new[]
                {
                    "Palacio de Cristal, Quevedo",
                    "Parque Central de Quevedo (punto de partida para recorridos)",
                    "Malecón Eloy Alfaro, Quevedo (zona cultural/visitas)"
                },

                // ---- CENTRO HISTÓRICO ----
                ModalTitulo2 = "Centro histórico",
                ModalQueEs2 =
                    "Recorrido por espacios céntricos y puntos representativos que muestran el crecimiento urbano " +
                    "y social de Quevedo.",

                ModalAprendes2 =
                    "Se aprende sobre costumbres, lugares emblemáticos y referencias del desarrollo del cantón.",

                ModalTip2 =
                    "Mantente con el grupo y respeta los espacios públicos.",

                // ✅ DÓNDE QUEDA (Quevedo)
                ModalLugares2 = new[]
                {
                    "Parque Central de Quevedo",
                    "Iglesia San José de Quevedo",
                    "Malecón Eloy Alfaro, Quevedo"
                },

                // ---- PATRIMONIO ----
                ModalTitulo3 = "Patrimonio cultural",
                ModalQueEs3 =
                    "El patrimonio cultural está conformado por tradiciones, costumbres y manifestaciones que " +
                    "identifican a la población de Quevedo.",

                ModalAprendes3 =
                    "El visitante comprende la importancia de preservar la identidad cultural y valorar las tradiciones locales.",

                ModalTip3 =
                    "Respeta las expresiones culturales y solicita permiso antes de registrar actividades tradicionales.",

                // ✅ DÓNDE QUEDA (Quevedo)
                ModalLugares3 = new[]
                {
                    "Iglesia San José de Quevedo",
                    "Parque Central de Quevedo",
                    "Malecón Eloy Alfaro, Quevedo",
                    "Ruta del Río (parroquia San Cristóbal, Quevedo)"
                }
            };
        }
    }
}
