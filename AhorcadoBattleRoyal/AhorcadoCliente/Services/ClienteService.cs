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
public event Action? JugadorRechazado, RondaCambiada;
public event Action<TurnoComando>? TurnoCambiado;
public event Action<string>? JugadorExpulsado, JugadorGano;
public event Action<string, string>? JugadorGano;
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

                    case Orden.Expulsar:
                        var ComandoExpulsar = JsonSerializer.Deserialize<ExpulsarComando>(json);

                        if (ComandoExpulsar != null)
                        {
                            JugadorExpulsado?.invoke(ComandoExpulsar.Jugador ?? "", ComandoExpulsar.Palabra ?? "");
                        }
                        break;
                    case Orden.Ganar:
                        var ComandoGanar = JsonSerializer.Deserialize<GanadorComando>(json);

                        if (ComandoGanar != null)
                        {
                            JugadorGano?.invoke(ComandoExpulsar.Jugador ?? "");
                        }
                        break;
                    case Orden.CambiarRonda:
                        RondaCambiada.Invoke();
                        break;
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

public void EnviarLetra(char letra)
{
    var respodner = new ResponderComando
    {
        Comando = Orden.Responder,
        Letra = letra.ToString()
    };

    EnviarComando(responder, client);
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
