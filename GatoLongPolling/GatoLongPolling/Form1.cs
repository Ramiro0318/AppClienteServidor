using System;
using System.Windows.Forms;
using GatoLongPolling.Services;

namespace GatoLongPolling
{
    public partial class Form1 : Form
    {
        private GatoService? _service;

        public Form1()
        {
            InitializeComponent();
        }

        private void BtnIniciar_Click(object sender, EventArgs e)
        {
            try
            {
                _service = new GatoService();
                _service.OnLog += AgregarLog;
                _service.Iniciar();
                
                btnIniciar.Enabled = false;
                btnDetener.Enabled = true;
                lblEstado.Text = $"Servidor INICIADO";
                lblEstado.ForeColor = System.Drawing.Color.Green;
            }
            catch (Exception ex)
            {
                AgregarLog($"Error al iniciar: {ex.Message}");
            }
        }

        private void BtnDetener_Click(object sender, EventArgs e)
        {
            try
            {
                _service?.Detener();
                btnIniciar.Enabled = true;
                btnDetener.Enabled = false;
                lblEstado.Text = "Servidor DETENIDO";
                lblEstado.ForeColor = System.Drawing.Color.Red;
                AgregarLog("Servidor detenido");
            }
            catch (Exception ex)
            {
                AgregarLog($"Error al detener: {ex.Message}");
            }
        }

        private void AgregarLog(string mensaje)
        {
            if (lstLog.InvokeRequired)
            {
                lstLog.Invoke(new Action(() => AgregarLog(mensaje)));
                return;
            }
            
            lstLog.Items.Add($"[{DateTime.Now:HH:mm:ss}] {mensaje}");
            lstLog.TopIndex = lstLog.Items.Count - 1;
        }
    }
}
