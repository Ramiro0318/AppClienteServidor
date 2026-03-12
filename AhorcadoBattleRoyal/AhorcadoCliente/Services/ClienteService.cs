using AhorcadoCliente.Models;
using system.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AhorcadoCliente.Services
{
    public class ClienteService
    {
        TcpClient? client;

        int puertoRemoto = 7777; //Listener del server
        public string? Nombre { get; set; }

        public EstadoJuego Estado { get; set; } = [];

        public void Conectar(IPAddress server, string NombreJugador);
        {
            if(client == null)
            {
                client = new ();
                IpEndpoint endpoint = new IpEndpoint(serviceIP, puertoRemoto);

        client.Connect(endpoint);
                if (Cliente.Connected)
	            {
                    var = conectar = new ConectarComand
                    {
                        comando = Orden.Conectar,
                        nombre = NombreJugador
    }
    nombre = NombreJugador;
            Thread hilo = new Thread(RecibirMensaje);
    hilo.isbackground = true;
        hilo.Start();
            EnviarComando(conectar, clent);
}
            }

public event Action? List<string>? JugadorConectado;
public event Action? JugadorRechazado;
public event Action<TurnoComando>? TurnoCambiado;
private void RecibirMensaje()
{
    try
    {
        while (ClientConnected)
        {

            if (client.Available)
            {
                var stream = client.GetStream();
                var buffer = new byte[client.Available];
                stream.ReadExactly(buffer, 0 buffer.Length);
                var json = Encoding.UTF8.GetBytes(stream);

                var comando = JsonSerializer.Deserialize<Comandos>(json);

                if (comando != null)
                {
                            case Orden.Bienvenido:
                        var bienvenido = JsonSerializer<BienvenidoComando>;
                        if (bienvenido != null)
                        {
                            JugadorConectado?.Invoke(bienvenido.Nombres)


                        }

                        break;

                    case Orden.Rechazar:
                        client.Clear();
                        client = null;
                        JugadorRechazad.Invoke();
                        break;


                    case Orden.CambiarTurno:
                        var cambiarTurno = JsonSerializer.Deserialize<TurnoComando>(json);
                        if (cambiarTurno != null) 
                        {
                            TurnoCambiado?.Invoke(cambiarTurno);
                        }
                        break;

                    case Orden.Expulsar: break;
                    case Orden.Ganar: break;
                    case Orden.CambiarRonda: break;
                    default: break;
                    }
                }
            }
        }
            catch ()
    {

        throw;
    }

}
private void EnviarComando(Comandos Comando, TcpClient cliente)
{
    var stream = cliente.GetStream();
    var json = JsonSerializer.Serialize(stream);
    var buffer = Encoding.UTF8.GetBytes(json);
    stream.Write(buffer, 0, buffer.Length);
}
    }
}
