using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks.Dataflow;
using System.Xml;


IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 11000);

UdpClient server = new UdpClient(endpoint);
server.EnableBroadcast = true;

Console.WriteLine("Cliente UDP escuchando en el puerto 11000...");


while (true)
{
    IPEndPoint remoto = new IPEndPoint(IPAddress.Any, 0);
    byte[] buffer = server.Receive(ref remoto);

    //Decodificador
    string mensaje = Encoding.UTF8.GetString(buffer);
    //try
    //{
        var pc = Dns.GetHostEntry(remoto.Address);
        Console.WriteLine($"{pc.HostName}({remoto.Address.ToString()}): {mensaje}");

    //}
    //catch (Exception)
    //{

    //    Console.WriteLine("Hostname desconocido");
    //}
}
