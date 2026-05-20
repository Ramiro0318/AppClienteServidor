using SnakeServer.Models;
using System.Collections.Concurrent;

namespace SnakeServer.Services
{
    public class SalasService
    {
        public static ConcurrentDictionary<string, Sala> Salas { get; set; } = new ConcurrentDictionary<string, Sala>();
        public static ConcurrentDictionary<string, string> JugadorEspera { get; set; } = new();

        public Sala? BuscarSala(string id, string nombre) 
        {
            if (JugadorEspera.ContainsKey(id))
            {
                return null;
            }

            if (JugadorEspera.Count > 0)
            {
                Sala nueva = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    IdJugador1 = JugadorEspera.Keys.First(),
                    NombreJugador1 = JugadorEspera.Values.First(),
                    IdJugador2 = id,
                    NombreJugador2  = nombre
                
                };
                //nueva.Id = Guid.NewGuid().ToString();
                JugadorEspera.Remove(nueva.IdJugador1, out string? valor);
                Salas[nueva.Id] = nueva;
                return nueva;
            }
            else 
            {
                JugadorEspera[id] = nombre;
                return null;

            }
        }

    }
}
