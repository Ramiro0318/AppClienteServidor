
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;

List<TcpClient> Clients = new List<TcpClient>();
TcpListener servidor = new TcpListener(IPAddress.Any, 12600);

servidor.Start(); //Inicia a escuchar por peticiones

Console.WriteLine("Servidor TCP iniciado, escuchando en el puerto 12600");

while (true)
{
    var cliente = servidor.AcceptTcpClient(); //Espera a que un cliente se acepte

    Clients.Add(cliente);   //Agregarlo a la lista
    Console.WriteLine("Conexión aceptada con: " + cliente.Client?.RemoteEndPoint ?? "");

    Thread hilo = new Thread(EscucharCliente);
    hilo.IsBackground = true;
    hilo.Start(cliente);

}

void EscucharCliente(object? c)
{
    if (c != null)
    {
        TcpClient tcpClient = (TcpClient)c; //Unboxing
        while (tcpClient.Connected)
        {
            var stream = tcpClient.GetStream();
            if (tcpClient.Available > 0)
            {
                byte[] buffer = new byte[tcpClient.Available];
                stream.ReadExactly(buffer, 0, buffer.Length);
                var mensaje = Encoding.UTF8.GetString(buffer);
                Console.WriteLine(tcpClient.Client.RemoteEndPoint + "Dice esto: " + mensaje);
                Relay(tcpClient, buffer);
            }

            Thread.Sleep(100);
        }
        Clients.Remove(tcpClient);

    }

    void Relay(TcpClient origen, byte[] mensaje)
    {
        foreach (var cliente in Clients)
        {
            try
            {
                if (cliente != origen && cliente.Connected)
                {
                    var stream = cliente.GetStream();
                    stream.Write(mensaje, 0, mensaje.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}