namespace ConsoleApp4
{
    partial class Ahorcado
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Ahorcado));
            this.flFichasDeJuego = new System.Windows.Forms.FlowLayoutPanel();
            this.lblPista = new System.Windows.Forms.Label();
            this.picAhorcado = new System.Windows.Forms.PictureBox();
            this.flPalabra = new System.Windows.Forms.FlowLayoutPanel();
            this.lblMensaje = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnIniciarJuego = new System.Windows.Forms.PictureBox();
            this.bntVolver2 = new System.Windows.Forms.Button();
            this.btnSalir = new System.Windows.Forms.Button();
            this.lblTiempo = new System.Windows.Forms.Label();
            this.lblReiniciar = new System.Windows.Forms.Label();
            this.panelPista = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.picAhorcado)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnIniciarJuego)).BeginInit();
            this.panelPista.SuspendLayout();
            this.SuspendLayout();
            // 
            // flFichasDeJuego
            // 
            this.flFichasDeJuego.Location = new System.Drawing.Point(44, 89);
            this.flFichasDeJuego.Name = "flFichasDeJuego";
            this.flFichasDeJuego.Size = new System.Drawing.Size(445, 300);
            this.flFichasDeJuego.TabIndex = 0;
            // 
            // lblPista
            // 
            this.lblPista.BackColor = System.Drawing.Color.Transparent;
            this.lblPista.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPista.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblPista.Location = new System.Drawing.Point(0, 0);
            this.lblPista.Name = "lblPista";
            this.lblPista.Size = new System.Drawing.Size(178, 167);
            this.lblPista.TabIndex = 17;
            this.lblPista.Text = "label1";
            this.lblPista.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picAhorcado
            // 
            this.picAhorcado.Location = new System.Drawing.Point(496, 89);
            this.picAhorcado.Name = "picAhorcado";
            this.picAhorcado.Size = new System.Drawing.Size(250, 300);
            this.picAhorcado.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picAhorcado.TabIndex = 1;
            this.picAhorcado.TabStop = false;
            // 
            // flPalabra
            // 
            this.flPalabra.Location = new System.Drawing.Point(191, 404);
            this.flPalabra.Name = "flPalabra";
            this.flPalabra.Size = new System.Drawing.Size(608, 88);
            this.flPalabra.TabIndex = 2;
            // 
            // lblMensaje
            // 
            this.lblMensaje.AutoSize = true;
            this.lblMensaje.Font = new System.Drawing.Font("Century Gothic", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMensaje.ForeColor = System.Drawing.Color.Red;
            this.lblMensaje.Location = new System.Drawing.Point(504, 50);
            this.lblMensaje.Name = "lblMensaje";
            this.lblMensaje.Size = new System.Drawing.Size(189, 32);
            this.lblMensaje.TabIndex = 3;
            this.lblMensaje.Text = "¡ Incorrecto ! ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Century Gothic", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(73, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(265, 33);
            this.label2.TabIndex = 4;
            this.label2.Text = "Adivina la palabra ";
            // 
            // btnIniciarJuego
            // 
            this.btnIniciarJuego.Image = global::ConsoleApp4.Properties.Resources.btnStart;
            this.btnIniciarJuego.Location = new System.Drawing.Point(14, 13);
            this.btnIniciarJuego.Name = "btnIniciarJuego";
            this.btnIniciarJuego.Size = new System.Drawing.Size(445, 46);
            this.btnIniciarJuego.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnIniciarJuego.TabIndex = 5;
            this.btnIniciarJuego.TabStop = false;
            this.btnIniciarJuego.Click += new System.EventHandler(this.btnIniciarJuego_Click);
            // 
            // bntVolver2
            // 
            this.bntVolver2.BackColor = System.Drawing.Color.Red;
            this.bntVolver2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bntVolver2.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bntVolver2.ForeColor = System.Drawing.Color.White;
            this.bntVolver2.Location = new System.Drawing.Point(1, 508);
            this.bntVolver2.Name = "bntVolver2";
            this.bntVolver2.Size = new System.Drawing.Size(115, 35);
            this.bntVolver2.TabIndex = 6;
            this.bntVolver2.Text = "REGRESAR";
            this.bntVolver2.UseVisualStyleBackColor = false;
            this.bntVolver2.Click += new System.EventHandler(this.bntVolver2_Click);
            // 
            // btnSalir
            // 
            this.btnSalir.BackColor = System.Drawing.Color.Red;
            this.btnSalir.FlatAppearance.BorderSize = 0;
            this.btnSalir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalir.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSalir.ForeColor = System.Drawing.Color.White;
            this.btnSalir.Location = new System.Drawing.Point(759, 8);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(51, 37);
            this.btnSalir.TabIndex = 14;
            this.btnSalir.Text = "X";
            this.btnSalir.UseVisualStyleBackColor = false;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // lblTiempo
            // 
            this.lblTiempo.AutoSize = true;
            this.lblTiempo.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTiempo.Location = new System.Drawing.Point(553, 19);
            this.lblTiempo.Name = "lblTiempo";
            this.lblTiempo.Size = new System.Drawing.Size(110, 23);
            this.lblTiempo.TabIndex = 15;
            this.lblTiempo.Text = "Tiempo: 60";
            // 
            // lblReiniciar
            // 
            this.lblReiniciar.AutoSize = true;
            this.lblReiniciar.ForeColor = System.Drawing.Color.White;
            this.lblReiniciar.Location = new System.Drawing.Point(460, 13);
            this.lblReiniciar.Name = "lblReiniciar";
            this.lblReiniciar.Size = new System.Drawing.Size(35, 13);
            this.lblReiniciar.TabIndex = 16;
            this.lblReiniciar.Text = "label3";
            // 
            // panelPista
            // 
            this.panelPista.Controls.Add(this.lblPista);
            this.panelPista.Location = new System.Drawing.Point(7, 335);
            this.panelPista.Name = "panelPista";
            this.panelPista.Size = new System.Drawing.Size(178, 167);
            this.panelPista.TabIndex = 18;
            this.panelPista.Visible = false;
            // 
            // Ahorcado
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(815, 549);
            this.Controls.Add(this.panelPista);
            this.Controls.Add(this.lblReiniciar);
            this.Controls.Add(this.lblTiempo);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.bntVolver2);
            this.Controls.Add(this.btnIniciarJuego);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblMensaje);
            this.Controls.Add(this.flPalabra);
            this.Controls.Add(this.picAhorcado);
            this.Controls.Add(this.flFichasDeJuego);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Ahorcado";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ahorcado";
            this.Load += new System.EventHandler(this.Ahorcado_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picAhorcado)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnIniciarJuego)).EndInit();
            this.panelPista.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flFichasDeJuego;
        private System.Windows.Forms.PictureBox picAhorcado;
        private System.Windows.Forms.FlowLayoutPanel flPalabra;
        private System.Windows.Forms.Label lblMensaje;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox btnIniciarJuego;
        private System.Windows.Forms.Button bntVolver2;
        private System.Windows.Forms.Button btnSalir;
        private System.Windows.Forms.Label lblTiempo;
        private System.Windows.Forms.Label lblReiniciar;
        private System.Windows.Forms.Label lblPista;
        private System.Windows.Forms.Panel panelPista;
    }
}