namespace PapaCalienteClienteUDP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        ClienteJuego cliente = new();
        frmJuego formjuego = new();
        private void Form1_Load(object sender, EventArgs e)
        {
            cliente.UsuariosRecibidos += Cliente_UsuariosRecibidos;
            cliente.PapaEnviada += Cliente_PapaEnviada;
            this.Hide();
            frmConexion conexion = new frmConexion();
            conexion.Cliente = cliente;
            conexion.ShowDialog();
        }

        private void Cliente_PapaEnviada(string quien, string combinacion, int tiempo)
        {
            if (!formjuego.Visible)
            {
                this.Hide();
                formjuego.ShowDialog();
            }

            formjuego.EstablecerTimerGlobal(tiempo);
            if (quien.StartsWith(cliente.Nick ?? "----"))
            {
                //No soy yo
            }
        }

        private void Cliente_UsuariosRecibidos(string[] obj)
        {
            BeginInvoke(() =>
            {
                lstUsuarios.Items.Clear();
                foreach (var u in obj)
                {
                    lstUsuarios.Items.Add(u);
                }
            });
        }


    }
}
