namespace PapaCalienteClienteUDP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        ClienteJuego cliente = new();
        private void Form1_Load(object sender, EventArgs e)
        {
            cliente.UsuariosRecibidos += Cliente_UsuariosRecibidos;
            this.Hide();
            frmConexion conexion = new frmConexion();
            conexion.Cliente = cliente;
            conexion.ShowDialog();
        }

        private void Cliente_UsuariosRecibidos(string[] obj)
        {
            lstUsuarios.Items.Clear();
            foreach (var u in obj)
            {
                lstUsuarios.Items.Add(u);
            }
        }
    }
}
