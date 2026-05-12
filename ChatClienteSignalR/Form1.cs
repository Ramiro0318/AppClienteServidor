using Microsoft.AspNetCore.SignalR.Client;

namespace ChatClienteSignalR
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        HubConnection? conexion;
        private async Task btnEnviar_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && conexion != null)
            {
                await conexion.SendAsync("EnviarMensaje", "Ramiro", textBox1.Text);
                textBox1.Text = "";
            }
        }

        private async Task Form1_Load(object sender, EventArgs e)
        {
            conexion = new HubConnectionBuilder().WithUrl("https://localhost:7118/hubs/chat")
               .WithAutomaticReconnect().Build();

            conexion.On<string, string>("MensajeRecibido", (usuario, mensaje) =>
            {
                this.BeginInvoke(() =>
                {
                    lstMensajes.Items.Add($"{usuario} dice: {mensaje}");
                });
            });

            await conexion.StartAsync();
        }
    }
}
