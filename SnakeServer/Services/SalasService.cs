using SnakeServer.Models;
using System.Collections.Concurrent;

namespace SnakeServer.Services
{
    public class SalasService
    {
        public ConcurrentDictionary<string, Sala> Salas { get; set; } = new ConcurrentDictionary<string, Sala>();

        public ConcurrentDictionary<string, string> JugadorEspera { get; set; } = new();

        public bool BuscarSala(string id, string nombre) 
        {
            if (JugadorEspera.ContainsKey(id))
            {
                return false;
            }

            if (JugadorEspera.Count > 0)
            {
                Sala nueva = new();
                nueva.Id = Guid.NewGuid().ToString();
            }
            else 
            {
                JugadorEspera[id] = nombre;
                return true;
            }
        }

    }
}
