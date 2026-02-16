using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace PapaCalienteClienteUDP
{
    public partial class frmConexion : Form
    {
        public frmConexion()
        {
            InitializeComponent();
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ClienteJuego Cliente { get; internal set; }

        private void btnUnirse_Click(object sender, EventArgs e)
        {
            if (!IPAddress.TryParse(txtIp.Text, out IPAddress? ip))
            {
                MessageBox.Show("La direccion IP es incorrecta.");
                return;
            }
            if (txtNombre.Text.Length < 3)
            {
                MessageBox.Show("Escriba un nombre de 3 caracteres o más.");
                return;
            }

            if (Cliente != null)
            {
                Cliente.Conectar(ip, txtNombre.Text);
                Close();
            }
        }
    }
}
