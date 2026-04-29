namespace GatoLongPolling
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnIniciar;
        private System.Windows.Forms.Button btnDetener;
        private System.Windows.Forms.ListBox lstLog;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.TextBox txtPuerto;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.btnIniciar = new System.Windows.Forms.Button();
            this.btnDetener = new System.Windows.Forms.Button();
            this.lstLog = new System.Windows.Forms.ListBox();
            this.lblEstado = new System.Windows.Forms.Label();
            this.txtPuerto = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            
            // txtPuerto
            this.txtPuerto.Location = new System.Drawing.Point(12, 12);
            this.txtPuerto.Size = new System.Drawing.Size(60, 23);
            this.txtPuerto.Text = "8080";
            this.txtPuerto.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            
            // btnIniciar
            this.btnIniciar.Location = new System.Drawing.Point(78, 12);
            this.btnIniciar.Size = new System.Drawing.Size(80, 23);
            this.btnIniciar.Text = "▶ Iniciar";
            this.btnIniciar.UseVisualStyleBackColor = true;
            this.btnIniciar.Click += new System.EventHandler(this.BtnIniciar_Click);
            
            // btnDetener
            this.btnDetener.Location = new System.Drawing.Point(164, 12);
            this.btnDetener.Size = new System.Drawing.Size(80, 23);
            this.btnDetener.Text = "⏹ Detener";
            this.btnDetener.UseVisualStyleBackColor = true;
            this.btnDetener.Enabled = false;
            this.btnDetener.Click += new System.EventHandler(this.BtnDetener_Click);
            
            // lblEstado
            this.lblEstado.Location = new System.Drawing.Point(12, 45);
            this.lblEstado.Size = new System.Drawing.Size(250, 23);
            this.lblEstado.Text = "● Servidor DETENIDO";
            this.lblEstado.ForeColor = System.Drawing.Color.Red;
            
            // lstLog
            this.lstLog.Location = new System.Drawing.Point(12, 75);
            this.lstLog.Size = new System.Drawing.Size(760, 350);
            this.lstLog.Font = new System.Drawing.Font("Consolas", 9F);
            
            // Form1
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(this.txtPuerto);
            this.Controls.Add(this.btnIniciar);
            this.Controls.Add(this.btnDetener);
            this.Controls.Add(this.lblEstado);
            this.Controls.Add(this.lstLog);
            this.Text = "Servidor Gato · Long Polling";
            this.ResumeLayout(false);
        }
    }
}
