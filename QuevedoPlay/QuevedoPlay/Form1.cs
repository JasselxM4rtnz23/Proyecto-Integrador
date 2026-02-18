using QuevedoPlay.Estilos;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;


namespace QuevedoPlay
{
    public partial class Form1 : Form

    {
        public Form1()
        {
            InitializeComponent();
            AplicarTema();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //login
            txtUsuario.Text = "Ej: UserTurismo";
            txtUsuario.ForeColor = Color.Gray;
            txtPassword.Text = "Ej: ******";
            txtPassword.ForeColor = Color.Gray;
            //Registro
            txtRegUsuario.Text = "Ej: UserTurismo";
            txtRegUsuario.ForeColor = Color.Gray;

            // Botón principal
            RedondeoHelper.Aplicar(btnIngresar, 30);
            RedondeoHelper.Aplicar(btnSalir, 30);

            // Panel del login
            RedondeoHelper.Aplicar(panelLogin, 30);

            //Panel del Registro
            RedondeoHelper.Aplicar(panelRegistro, 10);

            //Boton registro
            RedondeoHelper.Aplicar(btnRegistrar, 10);

            // TextBox
            RedondeoHelper.Aplicar(txtUsuario, 15);
            RedondeoHelper.Aplicar(txtPassword, 15);

            btnIngresar.FlatStyle = FlatStyle.Flat;
            btnIngresar.FlatAppearance.BorderSize = 0;
            btnSalir.FlatStyle = FlatStyle.Flat;
            btnSalir.FlatAppearance.BorderSize = 0;

            //carpeta datos para el login
            if (!Directory.Exists("datos"))
            {
                Directory.CreateDirectory("datos");
            }

        }
        //Login
        private void txtUsuario_Enter(object sender, EventArgs e)
        {
            if (txtUsuario.ForeColor == Color.Gray)
            {
                txtUsuario.Text = "";
                txtUsuario.ForeColor = Color.Black;
            }
        }
        private void txtPassword_Enter(object sender, EventArgs e)
        {
            if (txtPassword.ForeColor == Color.Gray)
            {
                txtPassword.Text = "";
                txtPassword.ForeColor = Color.Black;
            }
        }
        //Registro
        private void txtRegUsuario_Enter(object sender, EventArgs e)
        {
            if (txtRegUsuario.ForeColor == Color.Gray)
            {
                txtRegUsuario.Text = "";
                txtRegUsuario.ForeColor = Color.Black;
            }
        }
        //Boton salir
        private void button1_Click_1(object sender, EventArgs e)
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
        private void AplicarTema()
        {
            this.BackColor = TemaApp.FondoPrincipal;

            panelLogin.BackColor = TemaApp.FondoPanel;

            lblTitulo.ForeColor = TemaApp.Secundario;
            lblDescripcion.ForeColor = TemaApp.TextoSecundario;

            txtUsuario.ForeColor = TemaApp.TextoPrincipal;
            txtPassword.ForeColor = TemaApp.TextoPrincipal;

            btnIngresar.BackColor = TemaApp.BotonPrincipal;
            btnIngresar.ForeColor = TemaApp.BotonTexto;
            btnIngresar.FlatStyle = FlatStyle.Flat;
            btnIngresar.FlatAppearance.BorderSize = 0;
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            string usuario = txtRegUsuario.Text.Trim();
            string pass = txtRegPassword.Text.Trim();
            string pass2 = txtRegConfirmPassword.Text.Trim();

            if (usuario == "" || pass == "" || pass2 == "")
            {
                MessageBox.Show("Complete todos los campos");
                return;
            }

            if (pass != pass2)
            {
                MessageBox.Show("Las contraseñas no coinciden");
                return;
            }

            string ruta = $"datos/{usuario}.txt";

            if (File.Exists(ruta))
            {
                MessageBox.Show("El usuario ya existe");
                return;
            }

            using (StreamWriter sw = new StreamWriter(ruta))
            {
                sw.WriteLine($"usuario={usuario}");
                sw.WriteLine($"password={pass}");
                sw.WriteLine("tiempo=0");
                sw.WriteLine();
                sw.WriteLine("# letras");
            }

            MessageBox.Show("Cuenta creada correctamente");

            panelRegistro.Visible = false;

            txtUsuario.Text = usuario;
            txtPassword.Clear();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            panelRegistro.Visible = true;
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string pass = txtPassword.Text.Trim();

            if (usuario == "" || pass == "")
            {
                MessageBox.Show("Ingrese usuario y contraseña");
                return;
            }

            string ruta = $"datos/{usuario}.txt";

            if (!File.Exists(ruta))
            {
                MessageBox.Show("El usuario no existe");
                return;
            }

            string[] lineas = File.ReadAllLines(ruta);

            string passGuardada = "";

            foreach (string linea in lineas)
            {
                if (linea.StartsWith("password="))
                {
                    passGuardada = linea.Replace("password=", "");
                    break;
                }
            }

            if (passGuardada == "")
            {
                MessageBox.Show("Archivo de usuario dañado");
                return;
            }

            if (pass != passGuardada)
            {
                MessageBox.Show("Contraseña incorrecta");
                return;
            }

            // LOGIN CORRECTO
            Sesion.UsuarioActual = txtUsuario.Text.Trim();
            new FrmOpciones().Show();
            this.Hide();
        }

        private void label9_Click(object sender, EventArgs e)
        {
            panelRegistro.Visible = false;
        }

        // Ver y ocultar Contraseña
        bool verPass1 = false;
        bool verPass2 = false;

        private void pbVerPass1_Click(object sender, EventArgs e)
        {
            verPass1 = !verPass1;

            txtRegPassword.UseSystemPasswordChar = !verPass1;
            pbVerPass1.Image = verPass1
                ? Properties.Resources.ojo_abierto
                : Properties.Resources.ojo_cerrado;
        }

        private void pbVerPass2_Click(object sender, EventArgs e)
        {
            verPass2 = !verPass2;

            txtRegConfirmPassword.UseSystemPasswordChar = !verPass2;
            pbVerPass2.Image = verPass2
                ? Properties.Resources.ojo_abierto
                : Properties.Resources.ojo_cerrado;
        }
        private void ptboxLogo_Click(object sender, EventArgs e)
        {

        }

        private void panelRegistro_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}