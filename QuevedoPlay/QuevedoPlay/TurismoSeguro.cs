using QuevedoPlay.Estilos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace QuevedoPlay
{
    public partial class TurismoSeguro : Form
    {
        List<Provincia> provincias = new List<Provincia>();
        Provincia provinciaHover = null;

        public TurismoSeguro()
        {
            InitializeComponent();
            CrearProvincias();
            pictureBox1.MouseMove += pictureBox1_MouseMove;
            pictureBox1.Paint += pictureBox1_Paint;
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            OpcionesTurismoSeguro Opciones = new OpcionesTurismoSeguro();
            Opciones.Show();
            this.Close();
        }

        void CrearProvincias()
        {
            //Región Venus del Rio Quevedo
            Provincia p1 = new Provincia();

            GraphicsPath path1 = new GraphicsPath();
            path1.AddPolygon(new Point[]
            {
                new Point(251,161),
                new Point(255,161),
                new Point(257,165),
                new Point(259,166),
                new Point(262,167),
                new Point(268,167),
                new Point(271,164),
                new Point(276,164),
                new Point(276,156),
                new Point(275,154),
                new Point(275,144),
                new Point(276,137),
                new Point(276,131),
                new Point(278,129),
                new Point(277,114),
                new Point(276,112),
                new Point(276,107),
                new Point(275,106),
                new Point(275,97),
                new Point(273,94),
                new Point(272,89),
                new Point(271,85),
                new Point(264,79),
                new Point(260,74),
                new Point(257,67),
                new Point(256,64),
                new Point(242,54),
                new Point(236,54),
                new Point(227,62),
                new Point(224,62),
                new Point(223,60),
                new Point(218,58),
                new Point(207,47),
                new Point(205,46),
                new Point(206,41),
                new Point(205,41),
                new Point(204,35),
                new Point(203,33),
                new Point(203,26),
                new Point(201,25),
                new Point(201,22),
                new Point(197,20),
                new Point(196,17),
                new Point(194,17),
                new Point(192,17),
                new Point(189,12),
                new Point(189,10),
                new Point(185,7),
                new Point(181,7),
                new Point(176,11),
                new Point(176,14),
                new Point(176,23),
                new Point(173,27),
                new Point(168,27),
                new Point(166,29),
                new Point(167,32),
                new Point(164,34),
                new Point(162,33),
                new Point(159,33),
                new Point(156,35),
                new Point(153,37),
                new Point(152,47),
                new Point(151,48),
                new Point(151,56),
                new Point(156,61),
                new Point(157,63),
                new Point(156,65),
                new Point(155,66),
                new Point(155,71),
                new Point(152,73),
                new Point(148,76),
                new Point(146,75),
                new Point(145,78),
                new Point(141,77),
                new Point(139,81),
                new Point(136,81),
                new Point(133,80),
                new Point(128,80),
                new Point(124,84),
                new Point(122,85),
                new Point(122,87),
                new Point(126,90),
                new Point(130,91),
                new Point(131,93),
                new Point(135,94),
                new Point(180,94),
                new Point(182,99),
                new Point(180,101),
                new Point(178,102),
                new Point(174,102),
                new Point(173,104),
                new Point(170,105),
                new Point(164,106),
                new Point(161,109),
                new Point(156,110),
                new Point(153,109),
                new Point(151,106),
                new Point(148,106),
                new Point(147,109),
                new Point(144,109),
                new Point(141,112),
                new Point(140,115),
                new Point(138,116),
                new Point(135,113),
                new Point(132,113),
                new Point(131,111),
                new Point(129,111),
                new Point(130,115),
                new Point(126,119),
                new Point(124,119),
                new Point(123,122),
                new Point(123,130),
                new Point(112,139),
                new Point(108,144),
                new Point(106,147),
                new Point(104,149),
                new Point(104,157),
                new Point(106,159),
                new Point(109,159),
                new Point(113,158),
                new Point(117,161),
                new Point(120,164),
                new Point(122,167),
                new Point(122,171),
                new Point(124,174),
                new Point(125,177),
                new Point(128,179),
                new Point(132,181),
                new Point(135,182),
                new Point(138,184),
                new Point(152,183),
                new Point(157,180),
                new Point(163,175),
                new Point(165,173),
                new Point(165,172),
                new Point(168,171),
                new Point(170,171),
                new Point(194,161),
                new Point(201,161),
                new Point(205,162),
                new Point(211,164),
                new Point(219,166),
                new Point(225,169),
                new Point(231,173),
                new Point(233,174),
                new Point(233,178),
                new Point(234,180),
                new Point(239,180),
                new Point(240,178),
                new Point(244,177),
                new Point(246,176),
                new Point(246,169),
                new Point(247,165),


            });


            p1.Nombre = "Venus del Río Quevedo";
            p1.Nivel = "Riesgo Alto";
            p1.Descripcion = "•Extorsión (\"vacunas\"). \n\n•Secuestro. \n\n•Balaceras entre bandas.";
            p1.Recomendaciones = "•Playa Grande Grito de Libertad.\n\n•Las zonas cercanas al Anillo Vial.";
            p1.picNivR = Properties.Resources.rojo;

            p1.Area = path1;
            provincias.Add(p1);

            //Región Nicolas Infante Días
            Provincia p2 = new Provincia();

            GraphicsPath path2 = new GraphicsPath();
            path2.AddPolygon(new Point[]
            {
                new Point(258, 66),
                new Point(267, 66),
                new Point(273, 67),
                new Point(275, 71),
                new Point(282, 76),
                new Point(285, 79),
                new Point(289, 78),
                new Point(293, 77),
                new Point(296, 70),
                new Point(299, 67),
                new Point(304, 61),
                new Point(309, 58),
                new Point(313, 60),
                new Point(315, 63),
                new Point(317, 63),
                new Point(316, 71),
                new Point(316, 78),
                new Point(318, 80),
                new Point(321, 82),
                new Point(320, 87),
                new Point(320, 89),
                new Point(320, 91),
                new Point(332, 104),
                new Point(333, 108),
                new Point(334, 112),
                new Point(330, 120),
                new Point(326, 122),
                new Point(323, 126),
                new Point(320, 127),
                new Point(318, 123),
                new Point(317, 119),
                new Point(316, 112),
                new Point(312, 108),
                new Point(307, 105),
                new Point(306, 109),
                new Point(308, 112),
                new Point(307, 114),
                new Point(300, 124),
                new Point(298, 128),
                new Point(299, 132),
                new Point(302, 135),
                new Point(306, 138),
                new Point(307, 141),
                new Point(307, 150),
                new Point(289, 164),
                new Point(286, 165),
                new Point(283, 167),
                new Point(278, 167),
                new Point(276, 165),
                new Point(275, 156),
                new Point(275, 154),
                new Point(275, 143),
                new Point(276, 133),
                new Point(277, 132),
                new Point(278, 115),
                new Point(276, 112),
                new Point(274, 104),
                new Point(275, 96),
                new Point(271, 93),
                new Point(271, 87),
                new Point(259, 74),
                new Point(258, 70),
                new Point(257, 69)

            });

            p2.Nombre = "Nicolas Infante Días";
            p2.Nivel = "Riesgo Alto";
            p2.Descripcion = "•Microtráfico." +
                "\n\n•Robo de vehículos. \n\n•Extorsión. \n\n•Balaceras entre bandas."; ;
            p2.Recomendaciones = "•Playa Grande (sector norte).\n\n•Zonas ribereñas donde el acceso policial es limitado.";
            p2.picNivR = Properties.Resources.rojo;



            p2.Area = path2;
            provincias.Add(p2);

            //Región La Esperanza
            Provincia p3 = new Provincia();

            GraphicsPath path3 = new GraphicsPath();
            path3.AddPolygon(new Point[]
            {
                new Point(319, 58),
                new Point(334, 55),
                new Point(341, 56),
                new Point(348, 59),
                new Point(355, 64),
                new Point(364, 73),
                new Point(369, 76),
                new Point(379, 73),
                new Point(384, 72),
                new Point(386, 74),
                new Point(391, 72),
                new Point(394, 75),
                new Point(396, 79),
                new Point(391, 86),
                new Point(391, 88),
                new Point(396, 88),
                new Point(397, 91),
                new Point(406, 92),
                new Point(413, 90),
                new Point(417, 89),
                new Point(435, 89),
                new Point(441, 92),
                new Point(445, 96),
                new Point(448, 101),
                new Point(452, 108),
                new Point(455, 116),
                new Point(457, 129),
                new Point(451, 135),
                new Point(445, 136),
                new Point(443, 140),
                new Point(440, 140),
                new Point(440, 144),
                new Point(437, 144),
                new Point(438, 149),
                new Point(435, 152),
                new Point(428, 158),
                new Point(428, 160),
                new Point(421, 160),
                new Point(421, 166),
                new Point(409, 175),
                new Point(405, 175),
                new Point(400, 171),
                new Point(394, 167),
                new Point(390, 165),
                new Point(387, 161),
                new Point(384, 157),
                new Point(376, 158),
                new Point(371, 155),
                new Point(370, 153),
                new Point(370, 145),
                new Point(365, 138),
                new Point(362, 132),
                new Point(360, 129),
                new Point(356, 130),
                new Point(350, 134),
                new Point(348, 135),
                new Point(344, 130),
                new Point(342, 126),
                new Point(336, 127),
                new Point(334, 122),
                new Point(333, 119),
                new Point(336, 113),
                new Point(337, 110),
                new Point(336, 106),
                new Point(334, 102),
                new Point(329, 98),
                new Point(323, 92),
                new Point(322, 90),
                new Point(322, 83),
                new Point(321, 80),
                new Point(319, 80),
                new Point(318, 77),
                new Point(318, 72),
                new Point(318, 67),
                new Point(320, 63),
                new Point(319, 58),

            });

            p3.Nombre = "La Esperanza";
            p3.Nivel = "Riesgo Bajo";
            p3.Descripcion = "•Robos en carreteras.\n\n" +
                "•Abigeato (robo de ganado\ny animales).";
            p3.Recomendaciones = "•Vía principal (Quevedo-El Empalme), donde se reportan asaltos a buses y camiones.";
            p3.picNivR = Properties.Resources.verde;



            p3.Area = path3;
            provincias.Add(p3);

            //Región 24 de mayo
            Provincia p4 = new Provincia();

            GraphicsPath path4 = new GraphicsPath();
            path4.AddPolygon(new Point[]
            {
                new Point(275,165),
                new Point(271,165),
                new Point(269,167),
                new Point(260,167),
                new Point(254,160),
                new Point(250,161),
                new Point(249,174),
                new Point(245,176),
                new Point(240,178),
                new Point(236,177),
                new Point(237,173),
                new Point(234,170),
                new Point(218,162),
                new Point(212,161),
                new Point(206,159),
                new Point(200,159),
                new Point(190,162),
                new Point(181,166),
                new Point(168,170),
                new Point(164,176),
                new Point(156,182),
                new Point(153,183),
                new Point(140,183),
                new Point(133,181),
                new Point(127,177),
                new Point(123,171),
                new Point(123,166),
                new Point(120,160),
                new Point(113,157),
                new Point(110,158),
                new Point(109,159),
                new Point(102,162),
                new Point(97,166),
                new Point(97,172),
                new Point(96,176),
                new Point(94,180),
                new Point(92,183),
                new Point(93,200),
                new Point(95,203),
                new Point(95,213),
                new Point(95,220),
                new Point(92,223),
                new Point(92,227),
                new Point(90,230),
                new Point(88,232),
                new Point(85,232),
                new Point(82,238),
                new Point(76,239),
                new Point(76,251),
                new Point(71,256),
                new Point(72,258),
                new Point(77,259),
                new Point(82,258),
                new Point(85,256),
                new Point(87,257),
                new Point(91,256),
                new Point(94,250),
                new Point(95,250),
                new Point(98,252),
                new Point(100,253),
                new Point(105,255),
                new Point(108,257),
                new Point(115,251),
                new Point(120,253),
                new Point(122,249),
                new Point(127,249),
                new Point(129,251),
                new Point(133,249),
                new Point(135,249),
                new Point(137,245),
                new Point(140,243),
                new Point(149,242),
                new Point(158,232),
                new Point(186,231),
                new Point(196,225),
                new Point(208,221),
                new Point(214,217),
                new Point(226,201),
                new Point(231,199),
                new Point(262,199),
                new Point(262,195),
                new Point(272,181),
                new Point(275,165)

            });

            p4.Nombre = "24 de Mayo";
            p4.Nivel = "Riesgo Medio";
            p4.Descripcion = "•Robo a domicilios.\n\n" +
                "•Asalto individual en calles secundarias poco iluminadas.";
            p4.Recomendaciones = "•Zonas colindantes con la Venus del Río; el riesgo sube por la cercanía a sectores rojos.";
            p4.picNivR = Properties.Resources.amarillo;


            p4.Area = path4;
            provincias.Add(p4);

            //Región San Cristobal
            Provincia p5 = new Provincia();

            GraphicsPath path5 = new GraphicsPath();
            path5.AddPolygon(new Point[]
            {
                new Point(407, 175),
                new Point(399, 170),
                new Point(392, 165),
                new Point(385, 160),
                new Point(382, 155),
                new Point(375, 156),
                new Point(370, 153),
                new Point(369, 148),
                new Point(367, 142),
                new Point(363, 137),
                new Point(358, 129),
                new Point(352, 132),
                new Point(348, 135),
                new Point(344, 131),
                new Point(341, 126),
                new Point(335, 126),
                new Point(334, 123),
                new Point(332, 120),
                new Point(326, 122),
                new Point(323, 127),
                new Point(320, 125),
                new Point(318, 119),
                new Point(316, 112),
                new Point(311, 108),
                new Point(308, 108),
                new Point(306, 110),
                new Point(308, 113),
                new Point(306, 116),
                new Point(302, 120),
                new Point(300, 125),
                new Point(297, 129),
                new Point(296, 132),
                new Point(298, 136),
                new Point(304, 140),
                new Point(306, 145),
                new Point(305, 150),
                new Point(302, 155),
                new Point(294, 161),
                new Point(286, 166),
                new Point(281, 168),
                new Point(273, 168),
                new Point(273, 173),
                new Point(273, 178),
                new Point(275, 181),
                new Point(285, 185),
                new Point(310, 193),
                new Point(315, 197),
                new Point(325, 198),
                new Point(321, 204),
                new Point(321, 207),
                new Point(324, 211),
                new Point(328, 216),
                new Point(331, 220),
                new Point(332, 227),
                new Point(326, 234),
                new Point(330, 236),
                new Point(336, 235),
                new Point(342, 233),
                new Point(345, 236),
                new Point(352, 235),
                new Point(359, 229),
                new Point(372, 229),
                new Point(377, 227),
                new Point(382, 228),
                new Point(384, 229),
                new Point(386, 228),
                new Point(387, 219),
                new Point(390, 219),
                new Point(391, 217),
                new Point(396, 216),
                new Point(397, 215),
                new Point(401, 215),
                new Point(403, 213),
                new Point(404, 206),
                new Point(405, 204),
                new Point(405, 197),
                new Point(404, 193),
                new Point(403, 191),
                new Point(403, 187),
                new Point(408, 187),
                new Point(409, 178),
                new Point(407, 175)


            });

            p5.Nombre = "San Cristobal";
            p5.Nivel = "Riesgo Medio";
            p5.Descripcion = "•Robo a personas en paradas de buses." +
                "\n\n•Asaltos en locales comerciales.";
            p5.Recomendaciones = "•Sector de la Terminal Terrestre.\n\n•Cercanías a la zona de los juegos mecánicos en festividades.";
            p5.picNivR = Properties.Resources.amarillo;


            p5.Area = path5;
            provincias.Add(p5);

            //Región Guayacan
            Provincia p6 = new Provincia();

            GraphicsPath path6 = new GraphicsPath();
            path6.AddPolygon(new Point[]
            {
                new Point(266, 198),
                new Point(265, 205),
                new Point(258, 211),
                new Point(255, 214),
                new Point(251, 219),
                new Point(247, 224),
                new Point(244, 229),
                new Point(236, 241),
                new Point(231, 247),
                new Point(225, 256),
                new Point(225, 258),
                new Point(225, 260),
                new Point(224, 267),
                new Point(220, 269),
                new Point(220, 271),
                new Point(217, 274),
                new Point(214, 277),
                new Point(204, 278),
                new Point(199, 279),
                new Point(195, 279),
                new Point(194, 280),
                new Point(190, 280),
                new Point(188, 282),
                new Point(185, 282),
                new Point(182, 284),
                new Point(174, 284),
                new Point(173, 285),
                new Point(165, 285),
                new Point(164, 287),
                new Point(158, 287),
                new Point(156, 289),
                new Point(150, 289),
                new Point(149, 290),
                new Point(143, 290),
                new Point(143, 290),
                new Point(128, 291),
                new Point(128, 291),
                new Point(125, 290),
                new Point(121, 291),
                new Point(118, 289),
                new Point(115, 289),
                new Point(112, 288),
                new Point(110, 288),
                new Point(108, 286),
                new Point(100, 285),
                new Point(99, 283),
                new Point(94, 283),
                new Point(91, 281),
                new Point(83, 281),
                new Point(80, 279),
                new Point(69, 278),
                new Point(63, 275),
                new Point(51, 274),
                new Point(47, 271),
                new Point(37, 261),
                new Point(32, 257),
                new Point(30, 253),
                new Point(30, 249),
                new Point(40, 249),
                new Point(49, 253),
                new Point(53, 258),
                new Point(57, 259),
                new Point(62, 260),
                new Point(70, 257),
                new Point(74, 256),
                new Point(79, 257),
                new Point(83, 257),
                new Point(86, 255),
                new Point(88, 254),
                new Point(89, 257),
                new Point(96, 250),
                new Point(106, 253),
                new Point(108, 255),
                new Point(112, 255),
                new Point(116, 250),
                new Point(122, 250),
                new Point(126, 247),
                new Point(130, 247),
                new Point(131, 249),
                new Point(137, 249),
                new Point(142, 242),
                new Point(152, 242),
                new Point(162, 231),
                new Point(190, 230),
                new Point(198, 226),
                new Point(205, 223),
                new Point(213, 221),
                new Point(218, 217),
                new Point(224, 210),
                new Point(230, 202),
                new Point(235, 198)

            });

            p6.Nombre = "Guayacán";
            p6.Nivel = "Riesgo Bajo";
            p6.Descripcion = "•Robo de motocicletas.\n\n" +
                "•Asaltos a estudiantes universitarios.";
            p6.Recomendaciones = "•Entrada a la Universidad (UTEQ).\n\n•Las etapas más alejadas de la vía principal.";
            p6.picNivR = Properties.Resources.verde;


            p6.Area = path6;
            provincias.Add(p6);

            //Región Siete de Octubre
            Provincia p7 = new Provincia();

            GraphicsPath path7 = new GraphicsPath();
            path7.AddPolygon(new Point[]
            {
                new Point(265,205),
                new Point(269,211),
                new Point(272,217),
                new Point(272,237),
                new Point(269,240),
                new Point(266,246),
                new Point(263,256),
                new Point(265,257),
                new Point(271,261),
                new Point(275,266),
                new Point(276,270),
                new Point(268,280),
                new Point(263,286),
                new Point(252,287),
                new Point(249,284),
                new Point(249,273),
                new Point(243,270),
                new Point(235,264),
                new Point(228,258),
                new Point(228,255),
                new Point(249,223),
                new Point(260,208),

            });

            p7.Nombre = "Siete de Octubre";
            p7.Nivel = "Riesgo Medio";
            p7.Descripcion = "•Hurto (\"arranches\").\n\n•Robo de celulares.\n\n" +
                "•Estafas callejeras.";
            p7.Recomendaciones = "•Calle Siete de Octubre (arteria principal).\n\n•La zona del mercado.";
            p7.picNivR = Properties.Resources.amarillo;


            p7.Area = path7;
            provincias.Add(p7);

            //Región San Camilo
            Provincia p8 = new Provincia();

            GraphicsPath path8 = new GraphicsPath();
            path8.AddPolygon(new Point[]
            {
               new Point(276, 180),
                new Point(283, 182),
                new Point(289, 185),
                new Point(295, 187),
                new Point(303, 190),
                new Point(309, 192),
                new Point(314, 194),
                new Point(317, 197),
                new Point(322, 199),
                new Point(326, 199),
                new Point(324, 204),
                new Point(322, 207),
                new Point(325, 212),
                new Point(329, 216),
                new Point(332, 220),
                new Point(334, 225),
                new Point(334, 228),
                new Point(328, 235),
                new Point(327, 240),
                new Point(326, 243),
                new Point(324, 250),
                new Point(322, 253),
                new Point(319, 253),
                new Point(318, 251),
                new Point(318, 243),
                new Point(312, 237),
                new Point(312, 218),
                new Point(306, 218),
                new Point(296, 224),
                new Point(293, 226),
                new Point(284, 226),
                new Point(279, 222),
                new Point(277, 218),
                new Point(276, 212),
                new Point(273, 209),
                new Point(267, 210),
                new Point(265, 206),
                new Point(264, 196),
                new Point(267, 191),
                new Point(272, 185),
                new Point(276, 180)
            });

            p8.Nombre = "San Camilo";
            p8.Nivel = "Riesgo Medio";
            p8.Descripcion = "•\"Saca-pintas\" (robo a la salida de bancos)." +
                "\n\n•Asalto a transeúntes. \n\n•Robo de accesorios de autos.";
            p8.Recomendaciones = "•Av. Guayaquil (por la aglomeración).\n\n•El área del Parque Lineal (especialmente de noche).";
            p8.picNivR = Properties.Resources.amarillo;


            p8.Area = path8;
            provincias.Add(p8);

            //Región Viva Alfaro
            Provincia p9 = new Provincia();

            GraphicsPath path9 = new GraphicsPath();
            path9.AddPolygon(new Point[]
            {
                new Point(268, 208),
                new Point(274, 208),
                new Point(277, 210),
                new Point(277, 216),
                new Point(283, 223),
                new Point(293, 223),
                new Point(306, 215),
                new Point(312, 215),
                new Point(312, 234),
                new Point(317, 240),
                new Point(317, 251),
                new Point(309, 261),
                new Point(309, 271),
                new Point(314, 277),
                new Point(314, 287),
                new Point(310, 294),
                new Point(302, 294),
                new Point(298, 292),
                new Point(286, 292),
                new Point(285, 294),
                new Point(284, 302),
                new Point(273, 296),
                new Point(272, 293),
                new Point(271, 285),
                new Point(268, 280),
                new Point(276, 271),
                new Point(275, 267),
                new Point(271, 262),
                new Point(263, 257),
                new Point(266, 247),
                new Point(269, 241),
                new Point(272, 238),
                new Point(272, 219),
                new Point(269, 213)

            });

            p9.Nombre = "Viva Alfaro";
            p9.Nivel = "Riesgo Alto";
            p9.Descripcion = "•Robo a mano armada.\n\n•Conflictos territoriales.";
            p9.Recomendaciones = "•Barrio El Desquite.\n\n•Las laderas del cerro donde se esconden tras cometer delitos.";
            p9.picNivR = Properties.Resources.rojo;


            p9.Area = path9;
            provincias.Add(p9);

            //Región San Carlos
            Provincia p10 = new Provincia();

            GraphicsPath path10 = new GraphicsPath();
            path10.AddPolygon(new Point[]
            {
                new Point(328,235),
                new Point(343,232),
                new Point(345,236),
                new Point(352,235),
                new Point(357,230),
                new Point(362,229),
                new Point(375,229),
                new Point(379,226),
                new Point(384,229),
                new Point(391,238),
                new Point(398,246),
                new Point(402,248),
                new Point(401,257),
                new Point(396,266),
                new Point(395,272),
                new Point(395,283),
                new Point(399,286),
                new Point(408,304),
                new Point(408,318),
                new Point(405,318),
                new Point(405,321),
                new Point(419,328),
                new Point(420,334),
                new Point(418,336),
                new Point(418,339),
                new Point(420,340),
                new Point(421,351),
                new Point(428,360),
                new Point(424,364),
                new Point(407,352),
                new Point(392,351),
                new Point(384,349),
                new Point(378,348),
                new Point(376,352),
                new Point(366,357),
                new Point(364,360),
                new Point(364,376),
                new Point(361,380),
                new Point(354,384),
                new Point(355,389),
                new Point(357,398),
                new Point(360,401),
                new Point(360,413),
                new Point(361,429),
                new Point(359,437),
                new Point(352,441),
                new Point(353,450),
                new Point(355,454),
                new Point(339,468),
                new Point(331,468),
                new Point(331,466),
                new Point(327,467),
                new Point(326,458),
                new Point(318,453),
                new Point(318,441),
                new Point(304,421),
                new Point(297,421),
                new Point(287,415),
                new Point(286,407),
                new Point(284,406),
                new Point(278,396),
                new Point(277,391),
                new Point(281,381),
                new Point(281,378),
                new Point(273,378),
                new Point(270,374),
                new Point(270,367),
                new Point(266,364),
                new Point(263,355),
                new Point(262,345),
                new Point(267,340),
                new Point(276,338),
                new Point(286,337),
                new Point(288,344),
                new Point(292,343),
                new Point(292,339),
                new Point(290,338),
                new Point(290,323),
                new Point(299,314),
                new Point(298,310),
                new Point(285,300),
                new Point(286,294),
                new Point(288,291),
                new Point(300,291),
                new Point(308,294),
                new Point(312,293),
                new Point(316,284),
                new Point(316,276),
                new Point(312,269),
                new Point(313,261),
                new Point(320,251),
                new Point(324,250),
                new Point(328,236),

            });

            p10.Nombre = "San Carlos";
            p10.Nivel = "Riesgo Bajo";
            p10.Descripcion = "•Asaltos en paradas de bus interprovincial.\n\n•Robos tipo \"estruche\" en fincas.";
            p10.Recomendaciones = "•El casco central de la parroquia en días de feria.\n\n•La vía hacia Babahoyo.";
            p10.picNivR = Properties.Resources.verde;


            p10.Area = path10;
            provincias.Add(p10);

        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Provincia nueva = null;

            foreach (var p in provincias)
            {
                if (p.Area.IsVisible(e.Location))
                {
                    nueva = p;
                    break;
                }
            }

            if (nueva != provinciaHover)
            {
                provinciaHover = nueva;
                pictureBox1.Invalidate();
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (provinciaHover != null)
            {
                using (Brush b = new SolidBrush(
                    Color.FromArgb(50, Color.Black)))
                {
                    e.Graphics.FillPath(b, provinciaHover.Area);
                }
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            foreach (var p in provincias)
            {
                if (p.Area.IsVisible(e.Location))
                {
                    MostrarProvincia(p);
                    break;
                }


            }
        }
        void MostrarProvincia(Provincia p)
        {
            panInfo.Visible = true;

            lblNombre.Text = p.Nombre;
            lblNivel.Text = p.Nivel;
            lblDetalles.Text = p.Descripcion;
            lblRecomendaciones.Text = p.Recomendaciones;
            picNivR.Image = p.picNivR;


            if (p.Nivel == "Riesgo Alto")
            {
                lblNivel.BackColor = Color.FromArgb(150, 255, 0, 0);
            }
            else if (p.Nivel == "Riesgo Medio")
            {
                lblNivel.BackColor = Color.Yellow;
            }
            else if (p.Nivel == "Riesgo Bajo")
            {
                lblNivel.BackColor = Color.FromArgb(150, 0, 255, 0);
            }

        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void TurismoSeguro_Load(object sender, EventArgs e)
        {
            RedondeoHelper.Aplicar(btnRegresar, 20);
            RedondeoHelper.Aplicar(btnSalir, 20);
            RedondeoHelper.Aplicar(lblNivel, 20);
            RedondeoHelper.Aplicar(label2, 20);
            RedondeoHelper.Aplicar(label3, 20);
            RedondeoHelper.Aplicar(pictureBox1, 20);
        }


    }
}
