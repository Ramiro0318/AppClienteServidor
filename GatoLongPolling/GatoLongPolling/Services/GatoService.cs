using GatoLongPolling.DTOs;
using GatoLongPolling.Models;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace GatoLongPolling.Services
{
    public class GatoService
    {
        private HttpListener servidor;
        private bool activo;
        Salas salas = new();

        public event Action<string>? OnLog;

        public GatoService()
        {
            servidor = new HttpListener();
            string url = $"http://localhost:8080/gato/";
            servidor.Prefixes.Add(url);
        }

        public void Iniciar()
        {
            servidor.Start();
            activo = true;

            Thread hiloPrincipal = new Thread(EscucharPeticiones)
            {
                IsBackground = true
            };
            hiloPrincipal.Start();

            OnLog?.Invoke($"Servidor iniciado");
        }

        private void EscucharPeticiones()
        {
            while (activo)
            {
                try
                {
                    var context = servidor.GetContext();

                    Thread hiloPeticion = new Thread(() => ProcesarPeticion(context))
                    {
                        IsBackground = true,
                    };
                    hiloPeticion.Start();
                }
                catch (HttpListenerException ex)
                {
                    OnLog?.Invoke($"Error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    OnLog?.Invoke($"Error inesperado: {ex.Message}");
                }
            }
        }

        private void ProcesarPeticion(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;

            try
            {
                if (request.HttpMethod == "GET" && request.RawUrl == "/gato/")
                {
                    ServirArchivo(response, "index.html", "text/html");
                }
                else if (request.HttpMethod == "POST" && request.RawUrl == "/gato/jugar")
                {
                    var buffer = new byte[request.ContentLength64];
                    request.InputStream.ReadExactly(buffer, 0, buffer.Length);

                    var json = Encoding.UTF8.GetString(buffer);
                    var jugada = JsonSerializer.Deserialize<JugadaDTO>(json);

                    Sala? sala = salas.SolicitarSala(jugada.Id);
                    
                    if (sala != null)
                    {
                        response.StatusCode = 404;
                    }
                    else
                    {
                        var simbolo = sala.IdJugador1 == jugada.Id ? "X" : "O";
                        if (sala.Gato.HacerMovimiento(simbolo, jugada.Posicion)) 
                        {
                            RegresarTablero(response, sala);
                        }
                        else
                        {
                            response.StatusCode = 400;
                        }
                    }
                }
                else if (request.HttpMethod == "GET" && request.Url.AbsolutePath == "/gato/esperarTurno")
                {
                    //onbtener el id
                    var id = request.QueryString["id"];
                    if (id == null)
                    {
                        response.StatusCode = 400; //bad request
                        response.Close();
                    }
                    else
                    {
                        //buscar la sala del jugador
                        var sala = salas.SolicitarSala(id);
                        if (sala == null)
                        {
                            response.StatusCode = 404; //Not found
                            response.Close();
                        }
                        else
                        {
                            while (id != (sala.Gato.Turno == "X" ? sala.IdJugador1 : sala.IdJugador2))
                            {
                                Thread.Sleep(500);
                            }
                            RegresarTablero(response, sala);
                        }
                        //esperar hasta que sea su turno
                        //regresar el tablero.
                    }

                }
                else if (request.HttpMethod == "POST" && request.RawUrl == "/gato/registrar")
                {
                    byte[] buffer = new byte[request.ContentLength64];
                    request.InputStream.ReadExactly(buffer, 0, buffer.Length);

                    var json = Encoding.UTF8.GetString(buffer);

                    var usuario = JsonSerializer.Deserialize<RegistrarDTO>(json);

                    if (usuario == null)
                    {
                        response.StatusCode = 400; //bad request
                        response.Close();
                    }
                    else
                    {
                        var sala = salas.SolicitarSala(usuario.Nombre, usuario.Id);

                        if (sala.EstaLlena)
                        {
                            RegresarTablero(response, sala);
                        }
                        else
                        {
                            //Long polling
                            //No reseponder hasta quee esté llena

                            while (!sala.EstaLlena)
                            {
                                Thread.Sleep(500);
                            }
                            RegresarTablero(response, sala);
                        }
                    }
                }
                else
                {
                    response.StatusCode = 404;
                }
            }
            catch (Exception ex)
            {
                OnLog?.Invoke($"Error: {ex.Message}");
                response.StatusCode = 500;
            }
            finally
            {
                response.Close();
            }
        }

        private void RegresarTablero(HttpListenerResponse response, Sala sala)
        {
            TableroDTO tablero = new()
            {
                IdTurno = sala.Gato.Turno == "X" ? sala.IdJugador1 : sala.IdJugador2,
                Tablero = sala.Gato.Tablero,
                MensajeSuperior = $"Sala #{sala.Numero}<br>{sala.NombreJugador1} vs {sala.NombreJugador2}",
                MensajeInferior = $"Turno de {(sala.Gato.Turno == "X" ? sala.NombreJugador1 : sala.NombreJugador2)}",
            };

            var json = JsonSerializer.Serialize(tablero);
            byte[] buffer = Encoding.UTF8.GetBytes(json);
            response.ContentType = "application/json";
            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer, 0, buffer.Length);

            //response.Close();
        }

        private void ServirArchivo(HttpListenerResponse response, string nombreArchivo, string contentType)
        {
            string ruta = Path.Combine("Assets", nombreArchivo);

            if (File.Exists(ruta))
            {
                byte[] buffer = File.ReadAllBytes(ruta);
                response.ContentLength64 = buffer.Length;
                response.ContentType = contentType;
                response.OutputStream.Write(buffer, 0, buffer.Length);
                response.StatusCode = 200;
            }
            else
            {
                response.StatusCode = 404;
            }
        }

        public void Detener()
        {
            activo = false;
            servidor.Stop();
            OnLog?.Invoke("Servidor detenido");
        }

    }
}