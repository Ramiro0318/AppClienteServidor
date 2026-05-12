using Microsoft.AspNetCore.SignalR;

namespace AppSignalR.Hubs
{
    public class ChatHub: Hub
    {
        public async Task EnviarMensaje(string usuario, string mensaje) 
        {
            await Clients.Caller.SendAsync("Mensaje recibido", usuario, mensaje);    
        }



    }
}
