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

        int? tiempoLocal;

        public void MostrarJugadorPapa(string quien) 
        {
            pgbGlobal.Visible = false;
            lblCombinacion.Visible = false;
            btnDesactivar.Visible = false;
            txtCombinacion.Visible = false;
            txtCombinacion.Visible = false;
            lblTienesPapa.Visible = false;
            lblTienesPapa.Text = quien + "Tiene la papa";
        }

        public void MostrarPapa(string combinacion)
        {
            pgbGlobal.Visible = true;
            lblCombinacion.Visible = true;
            btnDesactivar.Visible = true;
            txtCombinacion.Visible = true;
            txtCombinacion.Visible = true;
            lblTienesPapa.Visible = true;
            lblTienesPapa.Text = "Tienes la papa";
            txtCombinacion.Clear();
            txtCombinacion.Focus();
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
    }
}
