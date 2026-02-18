namespace PapaCalienteClienteUDP
{
    partial class frmJuego
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmJuego));
            pgbGlobal = new ProgressBar();
            pgbLocal = new ProgressBar();
            label1 = new Label();
            imgPapa = new PictureBox();
            btnDesactivar = new Button();
            txtCombinacion = new TextBox();
            label3 = new Label();
            lblCombinacion = new Label();
            lblTienesPapa = new Label();
            timer1 = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)imgPapa).BeginInit();
            SuspendLayout();
            // 
            // pgbGlobal
            // 
            pgbGlobal.Location = new Point(180, 12);
            pgbGlobal.Name = "pgbGlobal";
            pgbGlobal.Size = new Size(593, 32);
            pgbGlobal.TabIndex = 0;
            // 
            // pgbLocal
            // 
            pgbLocal.Location = new Point(414, 77);
            pgbLocal.Name = "pgbLocal";
            pgbLocal.Size = new Size(286, 29);
            pgbLocal.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 13.8F);
            label1.Location = new Point(12, 14);
            label1.Name = "label1";
            label1.Size = new Size(162, 31);
            label1.TabIndex = 2;
            label1.Text = "Tiempo global";
            // 
            // imgPapa
            // 
            imgPapa.BackColor = Color.Transparent;
            imgPapa.Image = (Image)resources.GetObject("imgPapa.Image");
            imgPapa.Location = new Point(12, 50);
            imgPapa.Name = "imgPapa";
            imgPapa.Size = new Size(286, 381);
            imgPapa.SizeMode = PictureBoxSizeMode.Zoom;
            imgPapa.TabIndex = 4;
            imgPapa.TabStop = false;
            // 
            // btnDesactivar
            // 
            btnDesactivar.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnDesactivar.Location = new Point(436, 258);
            btnDesactivar.Name = "btnDesactivar";
            btnDesactivar.Size = new Size(198, 57);
            btnDesactivar.TabIndex = 5;
            btnDesactivar.Text = "Desactivar";
            btnDesactivar.UseVisualStyleBackColor = true;
            // 
            // txtCombinacion
            // 
            txtCombinacion.Font = new Font("Segoe UI", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtCombinacion.Location = new Point(398, 188);
            txtCombinacion.Name = "txtCombinacion";
            txtCombinacion.Size = new Size(321, 43);
            txtCombinacion.TabIndex = 6;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Segoe UI", 13.8F);
            label3.Location = new Point(425, 141);
            label3.Name = "label3";
            label3.Size = new Size(249, 31);
            label3.TabIndex = 7;
            label3.Text = "Escribe la combinación";
            // 
            // lblCombinacion
            // 
            lblCombinacion.AutoSize = true;
            lblCombinacion.BackColor = Color.Transparent;
            lblCombinacion.Font = new Font("Vivaldi", 22.2F, FontStyle.Bold | FontStyle.Italic | FontStyle.Underline, GraphicsUnit.Point, 0);
            lblCombinacion.Location = new Point(12, 373);
            lblCombinacion.Name = "lblCombinacion";
            lblCombinacion.Size = new Size(300, 44);
            lblCombinacion.TabIndex = 8;
            lblCombinacion.Text = "ABCDXYZ";
            // 
            // lblTienesPapa
            // 
            lblTienesPapa.AutoSize = true;
            lblTienesPapa.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTienesPapa.ForeColor = Color.IndianRed;
            lblTienesPapa.Location = new Point(38, 65);
            lblTienesPapa.Name = "lblTienesPapa";
            lblTienesPapa.Size = new Size(219, 41);
            lblTienesPapa.TabIndex = 9;
            lblTienesPapa.Text = "Tienes la papa";
            // 
            // timer1
            // 
            timer1.Tick += timer1_Tick;
            // 
            // frmJuego
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lblTienesPapa);
            Controls.Add(lblCombinacion);
            Controls.Add(label3);
            Controls.Add(txtCombinacion);
            Controls.Add(btnDesactivar);
            Controls.Add(label1);
            Controls.Add(pgbLocal);
            Controls.Add(pgbGlobal);
            Controls.Add(imgPapa);
            Name = "frmJuego";
            Text = "Papa caliente";
            ((System.ComponentModel.ISupportInitialize)imgPapa).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ProgressBar pgbGlobal;
        private ProgressBar pgbLocal;
        private Label label1;
        private PictureBox imgPapa;
        private Button btnDesactivar;
        private TextBox txtCombinacion;
        private Label label3;
        private Label lblCombinacion;
        private Label lblTienesPapa;
        private System.Windows.Forms.Timer timer1;
    }
}