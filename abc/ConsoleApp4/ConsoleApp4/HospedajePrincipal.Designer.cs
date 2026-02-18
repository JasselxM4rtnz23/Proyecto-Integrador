namespace ConsoleApp4
{
    partial class Hospedaje
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Hospedaje));
            this.label1 = new System.Windows.Forms.Label();
            this.pnlHotel = new System.Windows.Forms.Panel();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.bntVolver2 = new System.Windows.Forms.Button();
            this.btnSalir = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Tan;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(54, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(184, 38);
            this.label1.TabIndex = 1;
            this.label1.Text = "Hospedaje";
            // 
            // pnlHotel
            // 
            this.pnlHotel.Location = new System.Drawing.Point(32, 120);
            this.pnlHotel.Name = "pnlHotel";
            this.pnlHotel.Size = new System.Drawing.Size(852, 450);
            this.pnlHotel.TabIndex = 3;
            // 
            // comboBox1
            // 
            this.comboBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.comboBox1.Font = new System.Drawing.Font("Century Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Hotel Palmar del Sol ",
            "Hotel Olimpico",
            "Hotel San Andres Inn",
            "Bravo Grand Hotel",
            "Del Rio Hotel",
            "Hotel Golden",
            "Hotel Ingles"});
            this.comboBox1.Location = new System.Drawing.Point(52, 71);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(359, 33);
            this.comboBox1.TabIndex = 4;
            this.comboBox1.Text = "Hoteles";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // bntVolver2
            // 
            this.bntVolver2.BackColor = System.Drawing.Color.Red;
            this.bntVolver2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bntVolver2.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bntVolver2.ForeColor = System.Drawing.Color.White;
            this.bntVolver2.Location = new System.Drawing.Point(8, 573);
            this.bntVolver2.Name = "bntVolver2";
            this.bntVolver2.Size = new System.Drawing.Size(148, 41);
            this.bntVolver2.TabIndex = 5;
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
            this.btnSalir.Location = new System.Drawing.Point(847, 12);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(51, 37);
            this.btnSalir.TabIndex = 34;
            this.btnSalir.Text = "X";
            this.btnSalir.UseVisualStyleBackColor = false;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // Hospedaje
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(910, 615);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.bntVolver2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.pnlHotel);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Hospedaje";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form3";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlHotel;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button bntVolver2;
        private System.Windows.Forms.Button btnSalir;
    }
}