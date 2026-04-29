using GatoLongPolling.Models;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Text;
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
                else if (request.HttpMethod == "POST" && request.RawUrl == "/gato/registrar") 
                {
                    byte[] buffer = new byte[request.ContentLength64];
                    request.InputStream.ReadExactly(buffer, 0, buffer.Length);

                    var json = Encoding.UTF8.GetString(buffer);
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