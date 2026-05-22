using Microsoft.AspNetCore.SignalR;
using SnakeServer.Services;

namespace SnakeServer.Hubs
{
    public class GameHub : Hub
    {
        private readonly SalasService service;

        public GameHub(SalasService service)
        {
            this.service = service;
            service.TableroActualizado += Service_TableroActualizado;
        }

        private async void Service_TableroActualizado(Models.Sala sala)
        {
            if (sala.IdJugador1 != null && sala.IdJugador1 != null) 
            {
                await Clients.Client(sala.IdJugador1).SendAsync("taleroActualizado", sala.Tablero);
                await Clients.Client(sala.IdJugador1).SendAsync("taleroActualizado", sala.Tablero);
            }
        }

        public async void Conectar(string Nombrejugaor)
        {
            var id = Context.ConnectionId;

            if (!string.IsNullOrWhiteSpace(Nombrejugaor))
            {
                var sala = service.BuscarSala(id, Nombrejugaor);

                if (sala == null) //Estoy en espera
                {
                    await Clients.Caller.SendAsync("Esperando conexión");
                }
                else if(sala.IdJugador1 != null && sala.IdJugador2 != null)
                {

                    service.IniciarJuego(sala);

                    await Clients.Client(sala.IdJugador1).SendAsync("Juego iniciado", sala.NombreJugador2, sala.Tablero);

                    await Clients.Client(sala.IdJugador2).SendAsync("Juego iniciado", sala.NombreJugador1, sala.Tablero);

                    //Iniciar

                }

            }

        }
    }
}
