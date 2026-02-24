using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PapaCalienteClienteUDP
{
    public partial class frmJuego : Form
    {
        int? tiempoTotal;
        public frmJuego()
        {
            InitializeComponent();
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ClienteJuego? Cliente { set; get; }
        int? tiempoLocal;

        public void MostrarJugadorPapa(string quien)
        {
            pgbLocal.Visible = false;
            lblCombinacion.Visible = false;
            btnDesactivar.Visible = false;
            txtCombinacion.Visible = false;
            lblTienesPapa.Visible = false;
            lblBoom.Visible = false;
            lblTienesPapa.Text = quien + "Tiene la papa";
        }

        public void MostrarPapa(string combinacion)
        {
            pgbGlobal.Visible = true;
            lblCombinacion.Visible = true;
            btnDesactivar.Visible = true;
            txtCombinacion.Visible = true;
            lblTienesPapa.Visible = true;
            lblTienesPapa.Text = "Tienes la papa";
            txtCombinacion.Clear();
            txtCombinacion.Focus();

            pgbLocal.Maximum = 10;
            pgbLocal.Value = 10;
            timer2.Start();
        }

        public void Exploto()
        {
            lblBoom.Visible = true;
            lblTienesPapa.Text = "💥💣 ¡¡¡ Explotaste !!! 💣💥";
            timer1.Stop();
        }

        public void Exploto(string quien)
        {
            lblBoom.Visible = true;
            lblTienesPapa.Text = $"💥💣 ¡¡¡ {quien} HA EXPLOTADO !!! 💣💥";
            timer1.Stop();
        }
        public void EstablecerTimerGlobal(int tiempo)
        {
            if (tiempoTotal == null)
            {

                timer1.Stop();
                pgbGlobal.Maximum = tiempo;
                pgbGlobal.Value = tiempo;
            }
            else
            {
                pgbGlobal.Value = tiempo;
            }
            pgbGlobal.Maximum = tiempo;
        }

        

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (pgbGlobal.Value > 0)
            {
                pgbGlobal.Value--;
            }
            else
            {
                timer1.Stop();
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (pgbLocal.Value > 0)
            {
                pgbLocal.Value--;
            }
            else
            {
                timer2.Stop();
            }
        }

        private void btnDesactivar_Click(object sender, EventArgs e)
        {
            if (txtCombinacion.Text.Length == 5)
            {
                txtCombinacion.Visible = false;
                //Mandar al servidor
                Cliente?.Combinacion(txtCombinacion.Text);
                
            }
        }
    }
}
