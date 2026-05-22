using SnakeServer.Models;
using System.Collections.Concurrent;
using System.Data.SqlTypes;
using System.Drawing;
using System.Xml.Serialization;

namespace SnakeServer.Services
{
    public class SalasService
    {
        public static ConcurrentDictionary<string, Sala> Salas { get; set; } = new ConcurrentDictionary<string, Sala>();
        public static ConcurrentDictionary<string, string> JugadorEspera { get; set; } = new();
        public static ConcurrentDictionary<string, Timer> Timers { get; set; } = new(); }

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
                    NombreJugador2 = nombre

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


        public void IniciarJuego(Sala sala)
        {

            //sala.Tablero.Serpiente1 = new List<System.Drawing.Point>
            //{

            //};
            sala.Tablero.Serpiente1 = [
                new(5,6), new(2,6), new(3,6)
            ];

            sala.Tablero.Serpiente1 = [
                new(14,7), new(15,7), new(16,7)
            ];

            sala.Tablero.Direccion1 = Direccion.Derecha;
            sala.Tablero.Direccion1 = Direccion.Izquierda;


            sala.Tablero.Puntos1 = 0;
            sala.Tablero.Puntos2 = 0;


            var timer = new Timer((x) =>
            {

                MoverSerpiente(sala);
            }, null, 100, 900
            );
            CrearComida(sala);

            Timers[sala.Id] = timer;
        }

        public event Action<Sala>? TableroActualizado;

        public void MoverSerpiente(Sala sala) 
        {
            var s1 = sala.Tablero.Serpiente1;
            var s2 = sala.Tablero.Serpiente1;

            s1.RemoveAt(s1.Count - 1);
            s2.RemoveAt(s2.Count - 1);

            var nuevo = new Point(s1[0].X, s1[0].Y);
            s1.Insert(0, nuevo);

            switch (sala.Tablero.Direccion1)
            {
                case Direccion.Izquierda:
                    nuevo.X--;
                    break;
                case Direccion.Derecha:
                    nuevo.X ++;
                    break;
                case Direccion.Arriba:
                    nuevo.Y--;
                    break;
                case Direccion.Abajo:
                    nuevo.Y++;
                    break;
            }

            var nuevo2 = new Point(s1[0].X, s1[0].Y);
            s2.Insert(0, nuevo);

            switch (sala.Tablero.Direccion2)
            {
                case Direccion.Izquierda:
                    nuevo2.X--;
                    break;
                case Direccion.Derecha:
                    nuevo2.X++;
                    break;
                case Direccion.Arriba:
                    nuevo2.Y--;
                    break;
                case Direccion.Abajo:
                    nuevo2.Y++;
                    break;
            }

            //Cheecar colisiones

            TableroActualizado?.Invoke(sala);
        }

        public void CrearComida(Sala sala) 
        {
            //Si no hay espacios, fin de juego
            if (sala.Tablero.Ancho + sala.Tablero.Largo == sala.Tablero.Serpiente1.Count +sala.Tablero.Serpiente2.Count)
            {
                //fin juego
            }
            //Si hay espacios asignar al azar
            Random r = new();
            var point = 0;
            do
            {
                point = new Point(r.Next(sala.Tablero.Ancho), r.Next(sala.Tablero.Largo));
            } while (sala.Tablero.Serpiente1.Any(x => x.X == point.x && x.Point.Y) ||);
        }


    }
}
