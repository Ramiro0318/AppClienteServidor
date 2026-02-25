
//Endpoint remoto

using System.Net;
using System.Net.Mime;
using System.Net.Sockets;
using System.Text;

Console.WriteLine("Ip del servidor:");
var ip = Console.ReadLine();

IPEndPoint remoto = new IPEndPoint(IPAddress.Parse(ip ?? "0.0.0.0"), 12600);
TcpClient tcpClient = new TcpClient(remoto); //Connection request

Console.WriteLine("Escribe un mensaje");
string mensaje;
while ((mensaje = Console.ReadLine()) != null)
{
    byte[] buffer = Encoding.UTF8.GetBytes(mensaje);
    var stream = tcpClient.GetStream();
    stream.Write(buffer, 0, buffer.Length);


    Console.WriteLine("Mensaje enviado");
}

Console.WriteLine("Mensaje enviado");
