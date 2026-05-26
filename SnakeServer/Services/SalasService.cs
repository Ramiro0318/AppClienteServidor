using Microsoft.AspNetCore.SignalR;
using SnakeServer.Hubs;
using SnakeServer.Models;
using System.Collections.Concurrent;
using System.Data.SqlTypes;
using System.Drawing;
using System.Security.Principal;
using System.Xml.Serialization;

namespace SnakeServer.Services
{
    public class SalasService
    {
        private readonly IHubContext<GameHub> hub;

        public static ConcurrentDictionary<string, Sala> Salas { get; set; } = new ConcurrentDictionary<string, Sala>();
        public static ConcurrentDictionary<string, string> JugadorEspera { get; set; } = new();
        public static ConcurrentDictionary<string, Timer> Timers { get; set; } = new();


        public SalasService(IHubContext<GameHub> hub)
        {
            this.hub = hub;
        }

        public Sala? BuscarSala(string id)
        {
            return Salas.FirstOrDefault(x => x.Value.IdJugador1 == id || x.Value.IdJugador2 == id).Value;
        }
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


        public async void IniciarJuego(Sala sala)
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
            }, null, 100, 200
            );
            CrearComida(sala);

            Timers[sala.Id] = timer;
        }

        public event Action<Sala>? TableroActualizado;

        public async void MoverSerpiente(Sala sala)
        {
            var s1 = sala.Tablero.Serpiente1;
            var s2 = sala.Tablero.Serpiente1;

            var nuevo = new Point(s1[0].X, s1[0].Y);
            switch (sala.Tablero.Direccion1)
            {
                case Direccion.Izquierda:
                    nuevo.X--;
                    break;
                case Direccion.Derecha:
                    nuevo.X++;
                    break;
                case Direccion.Arriba:
                    nuevo.Y--;
                    break;
                case Direccion.Abajo:
                    nuevo.Y++;
                    break;
            }

            var nuevo2 = new Point(s1[0].X, s1[0].Y);


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
            //Colision contra pared
            if (nuevo.X < 0 || nuevo.Y < 0 || nuevo.X > sala.Tablero.Ancho || nuevo.Y > sala.Tablero.Largo)
            {
                sala.Tablero.Terminado = true;
                await hub.Clients.Clients([sala.IdJugador1 ?? "", sala.IdJugador2 ?? ""]).SendAsync("JugadorPerdio", sala.NombreJugador1);
            }

            if (nuevo.X < 0 || nuevo.Y < 0 || nuevo.X > sala.Tablero.Ancho || nuevo.Y > sala.Tablero.Largo)
            {
                sala.Tablero.Terminado = true;
                await hub.Clients.Clients([sala.IdJugador1 ?? "", sala.IdJugador2 ?? ""]).SendAsync("JugadorPerdio", sala.NombreJugador2);

            }

            //Colision contra oponente
            //Colision contra ti mismo
            if (s1.Contains(nuevo) || s2.Contains(nuevo))
            {
                sala.Tablero.Terminado = true;
                await hub.Clients.Clients([sala.IdJugador1 ?? "", sala.IdJugador2 ?? ""]).SendAsync("JugadorPerdio", sala.NombreJugador1);
            }

            if (s1.Contains(nuevo2) || s2.Contains(nuevo2))
            {
                sala.Tablero.Terminado = true;
                await hub.Clients.Clients([sala.IdJugador1 ?? "", sala.IdJugador2 ?? ""]).SendAsync("JugadorPerdio", sala.NombreJugador1);
            }


            //colision contra comida
            if (sala.Tablero.Manzana == nuevo)
            {
                sala.Tablero.Puntos1 ++;
                CrearComida(sala);
            }

            if (sala.Tablero.Manzana == nuevo2)
            {
                sala.Tablero.Puntos2++;
                CrearComida(sala);
            }


            s1.Insert(0, nuevo);
            s2.Insert(0, nuevo);

            if (s1.Count >= 3 + sala.Tablero.Puntos1)
            {
                s1.RemoveAt(s1.Count - 1);
            }

            if (s1.Count >= 3 + sala.Tablero.Puntos1)
            {
                s1.RemoveAt(s2.Count - 1);
            }


            if (!sala.Tablero.Terminado)
            {

                await hub.Clients.Client(sala.IdJugador1).SendAsync("taleroActualizado", sala.Tablero);
                await hub.Clients.Client(sala.IdJugador1).SendAsync("taleroActualizado", sala.Tablero);
            }
            else
            {
                Timers[sala.Id].Dispose();
                Timers.TryRemove(sala.Id, out Timer? t);
                Salas.TryRemove(sala.Id, out Sala? s);

            }

            

        }

        public void CrearComida(Sala sala)
        {
            //Si no hay espacios, fin de juego
            if (sala.Tablero.Ancho + sala.Tablero.Largo == sala.Tablero.Serpiente1.Count + sala.Tablero.Serpiente2.Count)
            {
                //fin juego
            }
            //Si hay espacios asignar al azar
            Random r = new();
            var point = 0;
            //do
            //{
            //    //point = new Point(r.Next(sala.Tablero.Ancho), r.Next(sala.Tablero.Largo));
            //} while (sala.Tablero.Serpiente1.Any(x => x.X == Point. && x.Y == x.Point.Y));
        }

        public void CambiarDireccion(Sala sala, string id, Direccion nueva)
        {
            if (sala.IdJugador1 == id)
            {
                var actual = sala.Tablero.Direccion1;

                switch (nueva)
                {
                    case Direccion.Izquierda:
                        if (actual != Direccion.Derecha)
                        {
                            sala.Tablero.Direccion1 = nueva;
                        }
                        break;

                    case Direccion.Arriba:
                        if (actual != Direccion.Abajo)
                        {
                            sala.Tablero.Direccion1 = nueva;
                        }
                        break;

                    case Direccion.Abajo:
                        if (actual != Direccion.Arriba)
                        {
                            sala.Tablero.Direccion1 = nueva;
                        }
                        break;

                    case Direccion.Derecha:
                        if (actual != Direccion.Izquierda)
                        {
                            sala.Tablero.Direccion1 = nueva;
                        }
                        break;

                }
            }
            else
            {
                var actual = sala.Tablero.Direccion2;

                switch (nueva)
                {
                    case Direccion.Izquierda:
                        if (actual != Direccion.Derecha)
                        {
                            sala.Tablero.Direccion1 = nueva;
                        }
                        break;

                    case Direccion.Arriba:
                        if (actual != Direccion.Abajo)
                        {
                            sala.Tablero.Direccion1 = nueva;
                        }
                        break;

                    case Direccion.Abajo:
                        if (actual != Direccion.Arriba)
                        {
                            sala.Tablero.Direccion1 = nueva;
                        }
                        break;

                    case Direccion.Derecha:
                        if (actual != Direccion.Izquierda)
                        {
                            sala.Tablero.Direccion1 = nueva;
                        }
                        break;
                }

            }
        }
    }
