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
        public frmJuego()
        {
            InitializeComponent();
        }

        public void EstablecerTimerGlobal(int tiempo)
        {
            timer1.Stop();
            pgbGlobal.Maximum = tiempo;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }
    }
}
