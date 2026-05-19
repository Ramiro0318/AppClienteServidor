using Microsoft.AspNetCore.SignalR;
using SnakeServer.Services;

namespace SnakeServer.Hubs
{
    public class GameHub:Hub
    {
        private readonly SalasService service;

        public GameHub(SalasService service)
        {
            this.service = service;
        }

        public void Conectar(string Nombrejugaor) 
        {
            
        }
    }
}
