using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Transactions;
using System.Windows.Forms;

namespace PapaCalienteServidorUDP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        ServidorJuego servidor = new();

        private void btnAbrir_Click(object sender, EventArgs e)
        {
            servidor.AbrirSala();
            btnAbrir.Enabled = false;
            btnComenzar.Enabled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            servidor.JugadorAceptado += Servidor_JugadorAceptado;
        }

        private void Servidor_JugadorAceptado(JugadorInfo obj)
        {
            //Cross thread calls

            lstJugadores.Items.Add(obj.Usuario);

        }
    }
}
