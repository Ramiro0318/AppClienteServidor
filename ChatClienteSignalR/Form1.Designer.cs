namespace ChatClienteSignalR
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lstMensajes = new ListBox();
            btnEnviar = new Button();
            textBox1 = new TextBox();
            SuspendLayout();
            // 
            // lstMensajes
            // 
            lstMensajes.FormattingEnabled = true;
            lstMensajes.Location = new Point(27, 63);
            lstMensajes.Name = "lstMensajes";
            lstMensajes.Size = new Size(730, 344);
            lstMensajes.TabIndex = 0;
            // 
            // btnEnviar
            // 
            btnEnviar.Location = new Point(663, 12);
            btnEnviar.Name = "btnEnviar";
            btnEnviar.Size = new Size(94, 29);
            btnEnviar.TabIndex = 1;
            btnEnviar.Text = "button1";
            btnEnviar.UseVisualStyleBackColor = true;
            btnEnviar.Click += this.btnEnviar_Click;
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Segoe UI", 14F);
            textBox1.Location = new Point(37, 12);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(545, 39);
            textBox1.TabIndex = 2;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(textBox1);
            Controls.Add(btnEnviar);
            Controls.Add(lstMensajes);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox lstMensajes;
        private Button btnEnviar;
        private TextBox textBox1;
    }
}
