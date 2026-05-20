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
                    await Clients.Client(sala.IdJugador1).SendAsync("Juego iniciado", sala.NombreJugador2);

                    await Clients.Client(sala.IdJugador2).SendAsync("Juego iniciado", sala.NombreJugador1);



                    //Iniciar
                }




            }



        }
    }
}
