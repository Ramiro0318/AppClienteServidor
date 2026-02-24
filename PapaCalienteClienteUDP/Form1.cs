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
            formjuego.Cliente = cliente;

            cliente.UsuariosRecibidos += Cliente_UsuariosRecibidos;
            cliente.PapaEnviada += Cliente_PapaEnviada;
            cliente.JugadorExploto += Cliente_JugadorExploto;
            this.Hide();
            frmConexion conexion = new frmConexion();
            conexion.Cliente = cliente;
            conexion.ShowDialog();
        }

        private void Cliente_JugadorExploto(string quien)
        {
            BeginInvoke(() =>
            {
                if (!quien.StartsWith(cliente.Nick ?? "---"))
                {
                    //No soy yo
                    formjuego.Exploto(quien);
                }
                else
                {
                    formjuego.Exploto();
                }
            });
        }


        private void Cliente_PapaEnviada(string quien, string combinacion, int tiempo)
        {
            BeginInvoke(() =>
            {

                if (!formjuego.Visible)
                {
                    this.Hide();
                    formjuego.Show();
                }

                formjuego.EstablecerTimerGlobal(tiempo);
                if (quien.StartsWith(cliente.Nick ?? "----"))
                {
                    //No soy yo
                    formjuego.MostrarJugadorPapa(quien);
                }
                else
                {
                    formjuego.MostrarPapa(combinacion);
                }
            });
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
