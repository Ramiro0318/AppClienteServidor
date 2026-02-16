namespace PapaCalienteClienteUDP
{
    partial class frmConexion
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
            label1 = new Label();
            label2 = new Label();
            txtIp = new TextBox();
            txtNombre = new TextBox();
            btnUnirse = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 19.8000011F);
            label1.Location = new Point(32, 23);
            label1.Name = "label1";
            label1.Size = new Size(390, 46);
            label1.TabIndex = 0;
            label1.Text = "Dirección Ip del servidor:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 19.8000011F);
            label2.Location = new Point(32, 144);
            label2.Name = "label2";
            label2.Size = new Size(324, 46);
            label2.TabIndex = 1;
            label2.Text = "Nombre del usuario:";
            // 
            // txtIp
            // 
            txtIp.Font = new Font("Segoe UI", 19.8000011F);
            txtIp.Location = new Point(32, 72);
            txtIp.Name = "txtIp";
            txtIp.Size = new Size(390, 51);
            txtIp.TabIndex = 2;
            // 
            // txtNombre
            // 
            txtNombre.Font = new Font("Segoe UI", 19.8000011F);
            txtNombre.Location = new Point(32, 193);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(390, 51);
            txtNombre.TabIndex = 3;
            // 
            // btnUnirse
            // 
            btnUnirse.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnUnirse.Location = new Point(151, 286);
            btnUnirse.Name = "btnUnirse";
            btnUnirse.Size = new Size(155, 92);
            btnUnirse.TabIndex = 4;
            btnUnirse.Text = "Solicitar unirse";
            btnUnirse.UseVisualStyleBackColor = true;
            btnUnirse.Click += btnUnirse_Click;
            // 
            // frmConexion
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(471, 415);
            Controls.Add(btnUnirse);
            Controls.Add(txtNombre);
            Controls.Add(txtIp);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Name = "frmConexion";
            Text = "Conexion";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private TextBox txtIp;
        private TextBox txtNombre;
        private Button btnUnirse;
    }
}