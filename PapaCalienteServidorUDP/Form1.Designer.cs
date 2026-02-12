namespace PapaCalienteServidorUDP
{
    partial class Form1
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
            btnAbrir = new Button();
            btnComenzar = new Button();
            lstJugadores = new ListBox();
            SuspendLayout();
            // 
            // btnAbrir
            // 
            btnAbrir.Font = new Font("Segoe UI", 18F);
            btnAbrir.Location = new Point(393, 22);
            btnAbrir.Name = "btnAbrir";
            btnAbrir.Size = new Size(243, 182);
            btnAbrir.TabIndex = 0;
            btnAbrir.Text = "Abrir Sala";
            btnAbrir.UseVisualStyleBackColor = true;
            // 
            // btnComenzar
            // 
            btnComenzar.Enabled = false;
            btnComenzar.Font = new Font("Segoe UI", 18F);
            btnComenzar.Location = new Point(393, 224);
            btnComenzar.Name = "btnComenzar";
            btnComenzar.Size = new Size(243, 182);
            btnComenzar.TabIndex = 1;
            btnComenzar.Text = "Comenzar ronda";
            btnComenzar.UseVisualStyleBackColor = true;
            // 
            // lstJugadores
            // 
            lstJugadores.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lstJugadores.FormattingEnabled = true;
            lstJugadores.Location = new Point(21, 22);
            lstJugadores.Name = "lstJugadores";
            lstJugadores.Size = new Size(345, 368);
            lstJugadores.TabIndex = 2;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lstJugadores);
            Controls.Add(btnComenzar);
            Controls.Add(btnAbrir);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private Button btnAbrir;
        private Button btnComenzar;
        private ListBox lstJugadores;
    }
}