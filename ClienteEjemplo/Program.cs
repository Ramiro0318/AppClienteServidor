using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml;

Console.WriteLine("Direccion IP del servidor");

var ip = Console.ReadLine();

if (!IPAddress.TryParse(ip,out IPAddress? address))
{
    Console.WriteLine("La dirección IP es incorrecta");
    return;
}

UdpClient client = new UdpClient();
client.EnableBroadcast = true;
client.Connect(address, 11000);

while (true) 
{
    Console.WriteLine("Escribe el mensaje a enviar");
    var mensaje = Console.ReadLine();

    //Codificar
    byte[] buffer = Encoding.UTF8.GetBytes(mensaje ?? "");
    client.Send(buffer, buffer.Length);

}
