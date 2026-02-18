using QuevedoPlay.Estilos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace QuevedoPlay
{
    public partial class CrucigramaSeg : Form
    {
        bool procesandoCarga = false;
        bool juegoIniciado = false;
        int totalIntentos = 0;
        bool nivelCompletado = false;
        int nivelActual = 1;
        bool cargandoProgreso = false;

        int segundosAcumulados = 0;
        DateTime momentoInicioTurno;

        private Timer timerReloj;

        List<Palabra> palabras = new List<Palabra>();
        HashSet<string> palabrasYaContadas = new HashSet<string>();
        Dictionary<(int fila, int columna), TextBox> matrizCrucigrama = new Dictionary<(int, int), TextBox>();
        Dictionary<(int fila, int columna), char> respuestasCorrectas = new Dictionary<(int, int), char>();
        Dictionary<(int fila, int columna), char> respuestasUsuario = new Dictionary<(int, int), char>();
        Dictionary<(int fila, int columna), int> estadoValidacion = new Dictionary<(int, int), int>();
        Dictionary<string, string> descripcionesPalabras = new Dictionary<string, string>();

        public CrucigramaSeg()
        {
            InitializeComponent();
            ConfigurarTimer();
        }

        private void ConfigurarTimer()
        {
            timerReloj = new Timer();
            timerReloj.Interval = 1000;
            timerReloj.Tick += (s, e) => ActualizarEstadisticas();
        }

        class Palabra
        {
            public string Nombre;
            public List<(int fila, int col)> Celdas;
        }

        private (int col, int fila) ObtenerCoordsDesdeNombre(string nombre)
        {
            string[] partesNombre = nombre.Split(new[] { "txt" }, StringSplitOptions.None);
            string coordenadas = partesNombre.Length > 1 ? partesNombre[1] : partesNombre[0];

            string[] partes = coordenadas.Split('_');
            return (int.Parse(partes[0]), int.Parse(partes[1]));
        }

        private void CrucigramaSeg_Load(object sender, EventArgs e)
        {
            label4.Visible = true;
            RedondeoHelper.Aplicar(btnRegresar, 20);
            RedondeoHelper.Aplicar(btnSalir, 20);
            RedondeoHelper.Aplicar(label3, 15);
            RedondeoHelper.Aplicar(label5, 15);
            RedondeoHelper.Aplicar(panel2, 15);
            RedondeoHelper.Aplicar(label4, 15);
            RedondeoHelper.Aplicar(label6, 15);

            ConfigurarTextBoxesCrucigrama(this);
            CambiarDeNivel(1, panelNivel1, panelPistas1);

        }

        private void ValidarPalabras()
        {
            foreach (var txt in matrizCrucigrama.Values)
                txt.BackColor = Color.White;

            foreach (var palabra in palabras)
            {
                bool completa = true;
                bool correcta = true;

                foreach (var (fila, col) in palabra.Celdas)
                {
                    if (!matrizCrucigrama.TryGetValue((fila, col), out TextBox txt) ||
                        string.IsNullOrWhiteSpace(txt.Text))
                    {
                        completa = false;
                        break;
                    }

                    if (respuestasCorrectas[(fila, col)] != txt.Text.ToUpper()[0])
                        correcta = false;
                }

                if (!completa) continue;

                Color colorFinal = correcta ? Color.LightGreen : Color.LightCoral;

                foreach (var (fila, col) in palabra.Celdas)
                {
                    if (matrizCrucigrama.TryGetValue((fila, col), out TextBox t))
                        t.BackColor = colorFinal;
                }

                if (!correcta && !palabrasYaContadas.Contains(palabra.Nombre))
                {
                    totalIntentos++;
                    palabrasYaContadas.Add(palabra.Nombre);
                }

                if (correcta)
                    palabrasYaContadas.Remove(palabra.Nombre);
            }
        }


        private void GuardarProgreso()
        {
            if (procesandoCarga) return;
            string ruta = $"datos/{Sesion.UsuarioActual}_crucigrama_N{nivelActual}.txt";
            Directory.CreateDirectory("datos");

            int segundosSesion = (juegoIniciado && timerReloj.Enabled) ? (int)(DateTime.Now - momentoInicioTurno).TotalSeconds : 0;
            int totalAGuardar = segundosAcumulados + segundosSesion;

            List<string> lineas = new List<string>();
            lineas.Add($"#INTENTOS={totalIntentos}");
            lineas.Add($"#TIEMPO={totalAGuardar}");

            foreach (var kv in respuestasUsuario)
            {
                int estado = 0;
                if (matrizCrucigrama.TryGetValue(kv.Key, out TextBox txt))
                    estado = (txt.BackColor == Color.LightGreen) ? 1 : 0;

                lineas.Add($"{kv.Key.fila},{kv.Key.columna}={kv.Value},{estado}");
            }
            File.WriteAllLines(ruta, lineas);
        }

        private void CargarProgreso()
        {
            string ruta = $"datos/{Sesion.UsuarioActual}_crucigrama_N{nivelActual}.txt";
            if (!File.Exists(ruta)) return;
            cargandoProgreso = true;
            procesandoCarga = true;
            foreach (string linea in File.ReadAllLines(ruta))
            {
                if (linea.StartsWith("#INTENTOS="))
                {
                    totalIntentos = int.Parse(linea.Split('=')[1]);
                    continue;
                }
                if (linea.StartsWith("#TIEMPO="))
                {
                    segundosAcumulados = int.Parse(linea.Split('=')[1]);
                    continue;
                }
                if (linea.StartsWith("#") || !linea.Contains("=")) continue;

                var partes = linea.Split('=');
                var pos = partes[0].Split(',');
                var datos = partes[1].Split(',');

                if (matrizCrucigrama.TryGetValue((int.Parse(pos[0]), int.Parse(pos[1])), out TextBox txt))
                {
                    txt.Text = datos[0];
                    respuestasUsuario[(int.Parse(pos[0]), int.Parse(pos[1]))] = datos[0][0];
                }
            }
            procesandoCarga = false;
            cargandoProgreso = false;

            momentoInicioTurno = DateTime.Now;
            ValidarPalabras();
            ActualizarEstadisticas();
        }

        private void ActualizarEstadisticas()
        {

            int palabrasCorrectasCount = 0;
            foreach (var palabra in palabras)
            {
                bool palabraOk = true;
                foreach (var (fila, col) in palabra.Celdas)
                {
                    if (!matrizCrucigrama.TryGetValue((fila, col), out TextBox txt) ||
                      string.IsNullOrWhiteSpace(txt.Text) ||
                      !respuestasCorrectas.TryGetValue((fila, col), out char correcta) ||
                      txt.Text.ToUpper()[0] != correcta)
                    {
                        palabraOk = false;
                        break;
                    }


                }
                if (palabraOk) palabrasCorrectasCount++;

            }

            int segundosSesion = (juegoIniciado && timerReloj.Enabled) ? (int)(DateTime.Now - momentoInicioTurno).TotalSeconds : 0;
            int tiempoTotalSegundos = segundosAcumulados + segundosSesion;
            TimeSpan t = TimeSpan.FromSeconds(tiempoTotalSegundos);
            string reloj = string.Format("{0:00}:{1:00}", (int)t.TotalMinutes, t.Seconds);

            if (palabrasCorrectasCount == palabras.Count && !nivelCompletado && !cargandoProgreso && juegoIniciado)

            {
                nivelCompletado = true;
                timerReloj.Stop();
                segundosAcumulados = tiempoTotalSegundos;
                juegoIniciado = false;
                MessageBox.Show($"¡Felicidades! Nivel {nivelActual} completado.");

            }

            double totalAcciones = palabrasCorrectasCount + totalIntentos;
            double precisionVal = totalAcciones > 0 ? ((double)palabrasCorrectasCount / totalAcciones) * 100 : 100;

            if (this.IsHandleCreated && !this.IsDisposed)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    if (lblAciertos != null) lblAciertos.Text = $"{palabrasCorrectasCount}/{palabras.Count}";
                    if (lblTiempo != null) lblTiempo.Text = reloj;
                    if (lblIntentos != null) lblIntentos.Text = totalIntentos.ToString();
                    if (lblPrecision != null) lblPrecision.Text = Math.Round(precisionVal) + "%";

                    if (lblAciertosNivel2 != null) lblAciertosNivel2.Text = palabrasCorrectasCount.ToString();
                    if (lblTiempoNivel2 != null) lblTiempoNivel2.Text = reloj;
                    if (lblErroresNivel2 != null) lblErroresNivel2.Text = totalIntentos.ToString();
                    if (lblPrecisionNivel2 != null) lblPrecisionNivel2.Text = Math.Round(precisionVal) + "%";

                    if (lblAciertosNivel1 != null) lblAciertosNivel1.Text = palabrasCorrectasCount.ToString();
                    if (lblTiempoNivel1 != null) lblTiempoNivel1.Text = reloj;
                    if (lblErroresNivel1 != null) lblErroresNivel1.Text = totalIntentos.ToString();
                    if (lblPrecisionNivel1 != null) lblPrecisionNivel1.Text = Math.Round(precisionVal) + "%";
                });

            }

        }

        private void Txt_TextChanged(object sender, EventArgs e)
        {
            if (procesandoCarga) return;

            if (!juegoIniciado)
            {
                juegoIniciado = true;
                momentoInicioTurno = DateTime.Now;
                timerReloj.Start();
            }

            TextBox txt = sender as TextBox;
            if (txt == null) return;

            var coords = ObtenerCoordsDesdeNombre(txt.Name);
            int columna = coords.col;
            int fila = coords.fila;

            procesandoCarga = true;
            txt.Text = txt.Text.ToUpper();
            txt.SelectionStart = txt.Text.Length;
            procesandoCarga = false;

            if (!string.IsNullOrEmpty(txt.Text))
                respuestasUsuario[(fila, columna)] = txt.Text[0];
            else
                respuestasUsuario.Remove((fila, columna));

            ValidarPalabras();
            GuardarProgreso();
            ActualizarEstadisticas();
        }

        private void SoloLetras_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar)) return;
            if (!char.IsLetter(e.KeyChar)) { e.Handled = true; return; }
            TextBox txt = sender as TextBox;
            txt.Text = e.KeyChar.ToString().ToUpper();
            e.Handled = true;
        }

        private void CargarRespuestas()
        {
            respuestasCorrectas.Clear();
            char[] saca = "SACAPINTAS".ToCharArray();
            for (int i = 0; i < saca.Length; i++) respuestasCorrectas[(2, 8 + i)] = saca[i];
            char[] abi = "ABIGEATO".ToCharArray();
            for (int i = 0; i < abi.Length; i++) respuestasCorrectas[(4, 16 + i)] = abi[i];
            char[] info = "INFORMATIVA".ToCharArray();
            for (int i = 0; i < info.Length; i++) respuestasCorrectas[(12, 2 + i)] = info[i];
            char[] ciu = "CIUDADANA".ToCharArray();
            for (int i = 0; i < ciu.Length; i++) respuestasCorrectas[(16, 4 + i)] = ciu[i];
            char[] eco = "ECONOMICA".ToCharArray();
            for (int i = 0; i < eco.Length; i++) respuestasCorrectas[(12, 17 + i)] = eco[i];
            char[] psi = "PSICOLOGICA".ToCharArray();
            for (int i = 0; i < psi.Length; i++) respuestasCorrectas[(2 + i, 12)] = psi[i];
            char[] asa = "ASALTOS".ToCharArray();
            for (int i = 0; i < asa.Length; i++) respuestasCorrectas[(2 + i, 16)] = asa[i];
            char[] ext = "EXTORSION".ToCharArray();
            for (int i = 0; i < ext.Length; i++) respuestasCorrectas[(4 + i, 20)] = ext[i];
            char[] mov = "MOVILIZARSE".ToCharArray();
            for (int i = 0; i < mov.Length; i++) respuestasCorrectas[(11 + i, 5)] = mov[i];
            char[] san = "SANITARIA".ToCharArray();
            for (int i = 0; i < san.Length; i++) respuestasCorrectas[(11 + i, 25)] = san[i];
        }

        private void CargarPalabras()
        {
            palabras.Clear();
            descripcionesPalabras.Clear();

            palabras.Add(new Palabra { Nombre = "SACAPINTAS", Celdas = new List<(int, int)> { (2, 8), (2, 9), (2, 10), (2, 11), (2, 12), (2, 13), (2, 14), (2, 15), (2, 16), (2, 17) } });
            palabras.Add(new Palabra { Nombre = "ABIGEATO", Celdas = new List<(int, int)> { (4, 16), (4, 17), (4, 18), (4, 19), (4, 20), (4, 21), (4, 22), (4, 23) } });
            palabras.Add(new Palabra { Nombre = "INFORMATIVA", Celdas = new List<(int, int)> { (12, 2), (12, 3), (12, 4), (12, 5), (12, 6), (12, 7), (12, 8), (12, 9), (12, 10), (12, 11), (12, 12) } });
            palabras.Add(new Palabra { Nombre = "CIUDADANA", Celdas = new List<(int, int)> { (16, 4), (16, 5), (16, 6), (16, 7), (16, 8), (16, 9), (16, 10), (16, 11), (16, 12) } });
            palabras.Add(new Palabra { Nombre = "ECONOMICA", Celdas = new List<(int, int)> { (12, 17), (12, 18), (12, 19), (12, 20), (12, 21), (12, 22), (12, 23), (12, 24), (12, 25) } });
            palabras.Add(new Palabra { Nombre = "PSICOLOGICA", Celdas = new List<(int, int)> { (2, 12), (3, 12), (4, 12), (5, 12), (6, 12), (7, 12), (8, 12), (9, 12), (10, 12), (11, 12), (12, 12) } });
            palabras.Add(new Palabra { Nombre = "ASALTOS", Celdas = new List<(int, int)> { (2, 16), (3, 16), (4, 16), (5, 16), (6, 16), (7, 16), (8, 16) } });
            palabras.Add(new Palabra { Nombre = "EXTORSION", Celdas = new List<(int, int)> { (4, 20), (5, 20), (6, 20), (7, 20), (8, 20), (9, 20), (10, 20), (11, 20), (12, 20) } });
            palabras.Add(new Palabra { Nombre = "MOVILIZARSE", Celdas = new List<(int, int)> { (11, 5), (12, 5), (13, 5), (14, 5), (15, 5), (16, 5), (17, 5), (18, 5), (19, 5), (20, 5), (21, 5) } });
            palabras.Add(new Palabra { Nombre = "SANITARIA", Celdas = new List<(int, int)> { (11, 25), (12, 25), (13, 25), (14, 25), (15, 25), (16, 25), (17, 25), (18, 25), (19, 25) } });

            descripcionesPalabras["SACAPINTAS"] =
"Modalidad de robo en la que delincuentes vigilan a personas que retiran dinero de entidades financieras " +
"como bancos o cajas, para luego seguirlas y asaltarlas. Generalmente ocurre cuando la víctima sale con efectivo.";


            descripcionesPalabras["ABIGEATO"] =
            "Delito que consiste en el robo de ganado. " +
            "Es frecuente en zonas rurales y afecta la economía de los productores.";

            descripcionesPalabras["INFORMATIVA"] =
            "Que tiene como finalidad transmitir información clara y objetiva.";

            descripcionesPalabras["CIUDADANA"] =
            "Relativo a los ciudadanos y sus derechos y deberes dentro de una sociedad.";

            descripcionesPalabras["ECONOMICA"] =
            "Relacionado con la administración de recursos, producción y consumo de bienes y servicios.";

            descripcionesPalabras["PSICOLOGICA"] =
            "Relacionado con la mente, emociones y comportamiento humano.";

            descripcionesPalabras["ASALTOS"] =
            "Ataques realizados generalmente con violencia o amenaza para robar bienes personales.";

            descripcionesPalabras["EXTORSION"] =
            "Delito que consiste en obligar a una persona, mediante amenazas, a entregar dinero o realizar acciones en contra de su voluntad.";

            descripcionesPalabras["MOVILIZARSE"] =
            "Acción de desplazarse de un lugar a otro por diferentes medios.";

            descripcionesPalabras["SANITARIA"] =
            "Relacionado con la salud pública, higiene y prevención de enfermedades.";

        }

        private void ConfigurarTextBoxesCrucigrama(Control contenedor)
        {
            foreach (Control c in contenedor.Controls)
            {
                if (c is TextBox txt && txt.Name.Contains("txt"))
                {
                    txt.MaxLength = 1;
                    txt.KeyPress += SoloLetras_KeyPress;
                    txt.TextChanged += Txt_TextChanged;
                    txt.KeyDown += MoverConFlechas_KeyDown;
                }
                if (c.HasChildren) ConfigurarTextBoxesCrucigrama(c);
            }
        }

        private void CrearMatrizCrucigrama(Control contenedor)
        {
            foreach (Control c in contenedor.Controls)
            {
                if (c is TextBox txt && txt.Name.Contains("_"))
                {
                    var coords = ObtenerCoordsDesdeNombre(txt.Name);
                    matrizCrucigrama[(coords.fila, coords.col)] = txt;
                }
                if (c.HasChildren) CrearMatrizCrucigrama(c);
            }
        }

        private void MoverConFlechas_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox txtActual = sender as TextBox;
            var coords = ObtenerCoordsDesdeNombre(txtActual.Name);
            int col = coords.col;
            int fila = coords.fila;
            int nF = fila, nC = col;

            if (e.KeyCode == Keys.Left) nC--;
            else if (e.KeyCode == Keys.Right) nC++;
            else if (e.KeyCode == Keys.Up) nF--;
            else if (e.KeyCode == Keys.Down) nF++;
            else return;

            if (matrizCrucigrama.TryGetValue((nF, nC), out TextBox txtDestino))
            {
                txtDestino.Focus();
                e.Handled = true;
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            GuardarProgreso();
            GuardarRecordFinal();
            var r = MessageBox.Show(
                "¿Seguro que quieres salir?",
                "Confirmar salida",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (r == DialogResult.Yes)
                Application.Exit();
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            GuardarProgreso();
            GuardarRecordFinal();
            new OpcionesTurismoSeguro().Show();
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ReiniciarJuego();
        }

        private void GuardarRecordFinal()
        {
            string ruta = "datos/Récord_Crucigrama.txt";
            string linea = $"{DateTime.Now} | Usuario: {Sesion.UsuarioActual} | Aciertos: {lblAciertos.Text} | Intentos: {totalIntentos} | Tiempo: {lblTiempo.Text}\n";
            File.AppendAllText(ruta, linea);
        }

        private void label3_Click(object sender, EventArgs e) { label4.Visible = true; label6.Visible = false; panel2.Visible = false; }
        private void label5_Click(object sender, EventArgs e) { panel2.Visible = false; label4.Visible = false; label6.Visible = true; }
        private void pictureBox2_Click(object sender, EventArgs e) { panel2.Visible = true; label4.Visible = false; label6.Visible = false; }

        private void picNivel3_Click(object sender, EventArgs e)
        {
            CambiarDeNivel(3, panel1, panelPistas);
            panelNivel1.Visible = false;
            panelPistas1.Visible = false;
        }

        private void picNivel2_Click(object sender, EventArgs e)
        {
            CambiarDeNivel(2, panelNivel2, panelPistas2);
        }

        private void CambiarDeNivel(int nivel, Panel pPrincipal, Panel pPistas)
        {
            nivelCompletado = false;

            nivelActual = nivel;

            panel1.Visible = panelPistas.Visible = false;
            panelNivel2.Visible = panelPistas2.Visible = false;
            panelNivel1.Visible = panelPistas1.Visible = false;
            pPrincipal.Visible = pPistas.Visible = true;

            matrizCrucigrama.Clear();
            respuestasCorrectas.Clear();
            palabras.Clear();
            respuestasUsuario.Clear();
            estadoValidacion.Clear();
            juegoIniciado = false;
            timerReloj.Stop();
            segundosAcumulados = 0;
            totalIntentos = 0;

            CrearMatrizCrucigrama(pPrincipal);

            if (nivel == 3) { CargarRespuestas(); CargarPalabras(); }
            else if (nivel == 2) { CargarRespuestasNivel2(); CargarPalabrasNivel2(); }
            else if (nivel == 1) { CargarRespuestasNivel1(); CargarPalabrasNivel1(); }

            CargarProgreso();
            ActualizarEstadisticas();
        }

        private void Estadisticas2_Click(object sender, EventArgs e)
        {
            panelEstadisticas2.Visible = true;
            pistasVerticales2.Visible = false;
            pistasHorizontales2.Visible = false;
        }

        private void verticales2_Click(object sender, EventArgs e)
        {
            pistasVerticales2.Visible = true;
            panelEstadisticas2.Visible = false;
            pistasHorizontales2.Visible = false;
        }

        private void horizontales2_Click(object sender, EventArgs e)
        {
            pistasHorizontales2.Visible = true;
            pistasVerticales2.Visible = false;
            panelEstadisticas2.Visible = false;
        }

        private void CargarRespuestasNivel2()
        {
            respuestasCorrectas.Clear();

            // HORIZONTALES 
            AgregarPalabraMatriz("TRANSPORTE", 3, 4, true);
            AgregarPalabraMatriz("BANCOS", 8, 1, true);
            AgregarPalabraMatriz("CONTACTO", 10, 9, true);
            AgregarPalabraMatriz("DELITOS", 13, 1, true);

            // VERTICALES 
            AgregarPalabraMatriz("RIESGO", 3, 5, false);
            AgregarPalabraMatriz("ESTAFAS", 2, 8, false);
            AgregarPalabraMatriz("EMERGENCIA", 1, 13, false);
            AgregarPalabraMatriz("SEGURO", 8, 6, false);
        }

        private void AgregarPalabraMatriz(string palabra, int fila, int col, bool horizontal)
        {
            for (int i = 0; i < palabra.Length; i++)
            {
                if (horizontal) respuestasCorrectas[(fila, col + i)] = palabra[i];
                else respuestasCorrectas[(fila + i, col)] = palabra[i];
            }
        }

        private void CargarPalabrasNivel2()
        {
            palabras.Clear();
            descripcionesPalabras.Clear();

            // HORIZONTALES
            palabras.Add(new Palabra { Nombre = "TRANSPORTE", Celdas = GenerarCeldas(3, 4, 10, true) });
            palabras.Add(new Palabra { Nombre = "BANCOS", Celdas = GenerarCeldas(8, 1, 6, true) });
            palabras.Add(new Palabra { Nombre = "CONTACTO", Celdas = GenerarCeldas(10, 9, 8, true) });
            palabras.Add(new Palabra { Nombre = "DELITOS", Celdas = GenerarCeldas(13, 1, 7, true) });

            // VERTICALES
            palabras.Add(new Palabra { Nombre = "RIESGO", Celdas = GenerarCeldas(3, 5, 6, false) });
            palabras.Add(new Palabra { Nombre = "ESTAFAS", Celdas = GenerarCeldas(2, 8, 7, false) });
            palabras.Add(new Palabra { Nombre = "EMERGENCIA", Celdas = GenerarCeldas(1, 13, 10, false) });
            palabras.Add(new Palabra { Nombre = "SEGURO", Celdas = GenerarCeldas(8, 6, 6, false) });

            descripcionesPalabras["TRANSPORTE"] =
"Medio utilizado para trasladarse de un lugar a otro, como buses, taxis o vehículos particulares. " +
"Es importante usar transporte seguro y autorizado.";

            descripcionesPalabras["BANCOS"] =
            "Instituciones financieras donde las personas guardan su dinero, realizan pagos y solicitan préstamos. " +
            "También ofrecen servicios de seguridad financiera.";

            descripcionesPalabras["CONTACTO"] =
            "Comunicación directa con una persona o institución. " +
            "En situaciones de emergencia, tener contactos confiables es fundamental.";

            descripcionesPalabras["DELITOS"] =
            "Acciones que infringen la ley y pueden causar daño a personas o bienes. " +
            "Ejemplos incluyen robos, estafas y agresiones.";

            descripcionesPalabras["RIESGO"] =
            "Probabilidad de que ocurra un daño o situación peligrosa. " +
            "Identificar riesgos permite tomar medidas preventivas.";

            descripcionesPalabras["ESTAFAS"] =
            "Engaños realizados con la intención de obtener dinero o beneficios de manera fraudulenta. " +
            "Frecuentemente ocurren por llamadas, mensajes o internet.";

            descripcionesPalabras["EMERGENCIA"] =
            "Situación inesperada que requiere atención inmediata para evitar daños mayores, " +
            "como accidentes, incendios o problemas de salud.";

            descripcionesPalabras["SEGURO"] =
            "Condición en la que no existe peligro o amenaza. " +
            "Adoptar conductas seguras reduce la posibilidad de sufrir daños.";

        }

        private void CargarRespuestasNivel1()
        {
            respuestasCorrectas.Clear();

            // HORIZONTALES 
            AgregarPalabraMatriz("ROBO", 1, 6, true);
            AgregarPalabraMatriz("SALUD", 5, 6, true);
            AgregarPalabraMatriz("POLICIA", 8, 1, true);

            // VERTICALES 
            AgregarPalabraMatriz("RUTAS", 1, 6, false);
            AgregarPalabraMatriz("AGUA", 5, 7, false);
            AgregarPalabraMatriz("MAPA", 6, 1, false);
        }
        private void CargarPalabrasNivel1()
        {
            palabras.Clear();
            descripcionesPalabras.Clear();

            // HORIZONTALES
            palabras.Add(new Palabra { Nombre = "ROBO", Celdas = GenerarCeldas(1, 6, 4, true) });
            palabras.Add(new Palabra { Nombre = "SALUD", Celdas = GenerarCeldas(5, 6, 5, true) });
            palabras.Add(new Palabra { Nombre = "POLICIA", Celdas = GenerarCeldas(8, 1, 7, true) });

            // VERTICALES
            palabras.Add(new Palabra { Nombre = "RUTAS", Celdas = GenerarCeldas(1, 6, 5, false) });
            palabras.Add(new Palabra { Nombre = "AGUA", Celdas = GenerarCeldas(5, 7, 4, false) });
            palabras.Add(new Palabra { Nombre = "MAPA", Celdas = GenerarCeldas(6, 1, 4, false) });

            descripcionesPalabras["ROBO"] =
"Delito que consiste en apropiarse de algo ajeno utilizando fuerza, violencia o intimidación. " +
"Es una conducta castigada por la ley y afecta la seguridad de las personas.";

            descripcionesPalabras["SALUD"] =
            "Estado de bienestar físico, mental y social. No solo significa no tener enfermedades, " +
            "sino también mantener hábitos que favorezcan una vida sana.";

            descripcionesPalabras["POLICIA"] =
            "Institución encargada de mantener el orden público, prevenir delitos y proteger a los ciudadanos. " +
            "Interviene en situaciones de emergencia y seguridad.";

            descripcionesPalabras["RUTAS"] =
            "Caminos o trayectos que permiten desplazarse de un lugar a otro. " +
            "Elegir rutas seguras es importante para prevenir riesgos.";

            descripcionesPalabras["AGUA"] =
            "Recurso natural indispensable para la vida. Es fundamental para la hidratación, " +
            "la higiene y el funcionamiento adecuado del cuerpo humano.";

            descripcionesPalabras["MAPA"] =
            "Representación gráfica de un territorio que muestra calles, rutas y ubicaciones. " +
            "Sirve para orientarse y planificar desplazamientos.";

        }

        private List<(int, int)> GenerarCeldas(int fila, int col, int largo, bool horizontal)
        {
            var celdas = new List<(int, int)>();
            for (int i = 0; i < largo; i++)
            {
                if (horizontal) celdas.Add((fila, col + i));
                else celdas.Add((fila + i, col));
            }
            return celdas;
        }



        private void ReiniciarJuego()
        {
            if (MessageBox.Show("¿Borrar todo el progreso?", "Confirmar",
                MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            procesandoCarga = true;
            juegoIniciado = false;
            timerReloj.Stop();

            totalIntentos = 0;
            segundosAcumulados = 0;
            nivelCompletado = false;

            foreach (var txt in matrizCrucigrama.Values)
            {
                txt.Clear();
                txt.BackColor = Color.White;
            }

            respuestasUsuario.Clear();
            estadoValidacion.Clear();

            string ruta = $"datos/{Sesion.UsuarioActual}_crucigrama_N{nivelActual}.txt";

            if (File.Exists(ruta))
                File.Delete(ruta);

            procesandoCarga = false;

            ActualizarEstadisticas();
        }


        private void restartNivel2_Click(object sender, EventArgs e)
        {
            ReiniciarJuego();
        }

        private void picNivel1_Click(object sender, EventArgs e)
        {
            CambiarDeNivel(1, panelNivel1, panelPistas1);
        }

        private void estadisticas1_Click(object sender, EventArgs e)
        {
            panelEstadisticas1.Visible = true;
            panelVerticales1.Visible = false;
            panelHorizontales1.Visible = false;
        }

        private void verticales1_Click(object sender, EventArgs e)
        {
            panelVerticales1.Visible = true;
            panelHorizontales1.Visible = false;
            panelEstadisticas1.Visible = false;

        }

        private void horizontales1_Click(object sender, EventArgs e)
        {
            panelHorizontales1.Visible = true;
            panelEstadisticas1.Visible = false;
            panelVerticales1.Visible = false;

        }

        private void restartNivel1_Click(object sender, EventArgs e)
        {
            ReiniciarJuego();
        }

        private void ComoJugar_Click(object sender, EventArgs e)
        {
            panelComoJugar.Visible = true;
        }

        private void BtnCerrarPanelComoJugar_Click(object sender, EventArgs e)
        {
            panelComoJugar.Visible = false;
        }

        private void CompletarPalabraConAyuda()
        {
            foreach (var palabra in palabras)
            {
                bool completaYCorrecta = true;

                foreach (var (fila, col) in palabra.Celdas)
                {
                    if (!matrizCrucigrama.TryGetValue((fila, col), out TextBox txt) ||
                        string.IsNullOrWhiteSpace(txt.Text) ||
                        txt.Text.ToUpper()[0] != respuestasCorrectas[(fila, col)])
                    {
                        completaYCorrecta = false;
                        break;
                    }
                }

                // Si encontramos una palabra incompleta o incorrecta
                if (!completaYCorrecta)
                {
                    // Rellenar palabra
                    foreach (var (fila, col) in palabra.Celdas)
                    {
                        if (matrizCrucigrama.TryGetValue((fila, col), out TextBox txt))
                        {
                            txt.Text = respuestasCorrectas[(fila, col)].ToString();
                            txt.BackColor = Color.LightGreen;
                        }
                    }

                    respuestasUsuario.Clear();
                    ValidarPalabras();
                    GuardarProgreso();
                    ActualizarEstadisticas();

                    // Mostrar descripción educativa
                    if (descripcionesPalabras.ContainsKey(palabra.Nombre))
                    {
                        MessageBox.Show(
                            $"Palabra completada: {palabra.Nombre}\n\n" +
                            $"{descripcionesPalabras[palabra.Nombre]}",
                            "Aprendizaje",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }

                    return; // Solo completa una palabra por clic
                }
            }

            MessageBox.Show("Todas las palabras ya están completas.");
        }

        private void btnAyudaNivel1_Click(object sender, EventArgs e)
        {
            CompletarPalabraConAyuda();
        }
    }
}