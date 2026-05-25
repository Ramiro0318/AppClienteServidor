using Microsoft.AspNetCore.SignalR;
using SnakeServer.Models;
using SnakeServer.Services;

namespace SnakeServer.Hubs
{
    public class GameHub : Hub
    {
        private readonly SalasService service;

        public GameHub(SalasService service)
        {
            this.service = service;
        }


        public async void Conectar(string Nombrejugaor)
        {
            var id = Context.ConnectionId;

            if (!string.IsNullOrWhiteSpace(Nombrejugaor))
            {
                var sala = service.BuscarSala(id, Nombrejugaor);

                if (sala == null) //Estoy en espera
                {
                    await Clients.Caller.SendAsync("EsperandoConexion");
                }
                else if(sala.IdJugador1 != null && sala.IdJugador2 != null)
                {

                    service.IniciarJuego(sala);

                    await Clients.Client(sala.IdJugador1).SendAsync("JuegoIniciado", sala.NombreJugador2, sala.Tablero);

                    await Clients.Client(sala.IdJugador2).SendAsync("JuegoIniciado", sala.NombreJugador1, sala.Tablero);

                    //Iniciar

                }

            }

        }


        public async Task Mover(string nombreJugador, string direccion) 
        {
            var id = Context.ConnectionId;
            var sala = service.BuscarSala(id);

            if (sala != null)
            {
                var dir = Enum.Parse(typeof(Direccion), direccion);
                
                service.CambiarDireccion(sala, id, (Direccion)dir);
            }



        }
    }
}
