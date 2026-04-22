using Kanbanhttp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;

namespace Kanbanhttp.Services
{
    public class KanbanService
    {
        public List<Tarea> Tareas { get; set; } = new();
        HttpListener servidor = new();

        public KanbanService()
        {
            //string url = "http://*:8080/kanban/";
            string url = "http://+:8080/kanban/";
            servidor.Prefixes.Add(url);
            Deserializar();
        }

        public void Iniciar()
        {
            servidor.Start();
            new Thread(EscuchaPeticiones)
            {
                IsBackground = true
            }.Start();
        }

        //Request Pool
        private void EscuchaPeticiones(object? obj)
        {
            var context = servidor.GetContext();
            new Thread(EscuchaPeticiones)
            {
                IsBackground = true
            }.Start();

            //Atender

            //Regresar archivos (index.html estilos.css, script.js)

            //Regresar el tablero (/tablero = index.html)

            //Regresar los datos GET(/tareas)

            //Camniar la tarea put (/tareas)

            var request = context.Request;
            var response = context.Response;

            if (request.HttpMethod == "GET" && request.RawUrl == "/kanban/tablero")
            {
                byte[]? buffer = LeerRecurso("/index.html");
                Enviar(response, buffer, "text/html");
            }
            else if (request.HttpMethod == "GET" && request.RawUrl == "/kanban/tareas")
            {
                string json = "";
                lock (Tareas)
                {
                    json = JsonSerializer.Serialize(GetAll());
                }

                var buffer = Encoding.UTF8.GetBytes(json);
                Enviar(response, buffer, "aplication/json");

            }
            else if (request.HttpMethod == "POST" && request.RawUrl == "/kanban/movertarea")
            {
                var buffer = new byte[request.ContentLength64];
                request.InputStream.ReadExactly(buffer, 0, buffer.Length);

                var json = Encoding.UTF8.GetString(buffer);
                Tarea? t = JsonSerializer.Deserialize<Tarea>(json);

                if (t != null)
                {
                    response.StatusCode = (int)HttpStatusCode.OK;
                }
                else
                {
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                }

            }
            //else if (request.HttpMethod == "GET" && Path.IsPathFullyQualified(request.RawUrl ?? ""))
            else if (request.HttpMethod == "GET" && new string[] { "/kanban/index.html", "/kanban/estilos.css", "/kanban/script.js" }.Contains(request.RawUrl))
            {
                if (!File.Exists("assets/" + Path.GetFileName(request.RawUrl)))
                {   //get file name
                    response.StatusCode = 404;
                }
                else
                {
                    var archivo = File.ReadAllBytes("assets/" + Path.GetFileName(request.RawUrl));
                    Enviar(response, archivo, getMime(Path.GetExtension(request.RawUrl ?? "")));
                }
            }
            else { }
            response.Close();

        }

        string getMime(string extension)
        {
            switch (extension)
            {
                case ".html": return "text/html";
                case ".css": return "text/css";
                case ".js": return "text/javascript";
                default: return "text";
            }
        }
        private static void Enviar(HttpListenerResponse response, byte[]? buffer, string type)
        {
            if (buffer != null)
            {
                response.ContentLength64 = buffer.Length;
                response.ContentType = type;
                response.StatusCode = 200;
                response.OutputStream.Write(buffer, 0, buffer.Length);
            }
            else
            {
                response.StatusCode = 404;
            }
        }

        byte[]? LeerRecurso(string nombre)
        {
            try
            {
                byte[] buffer = File.ReadAllBytes("assets/" + nombre);
                return buffer;
            }
            catch (Exception)
            {

                return null;
            }

        }

        public event Action<Tarea>? TareaCreada, TareaModificada, TareaEliminada;
        public void CrearTarea(Tarea t)
        {
            lock (Tareas)
            {
                t.Estado = Estados.Pendiente;
                if (Tareas.Count > 0)
                {
                    t.Id = Tareas.Max(x => x.Id) + 1;
                }
                else
                {
                    t.Id = 1;
                }
                Tareas.Add(t);
                Serializar();
                TareaCreada?.Invoke(t);
            }
        }

        public IEnumerable<Tarea> GetAll()
        {
            return Tareas.OrderByDescending(x => x.Fecha).ThenBy(x => x.Descripcion);
        }

        public void EliminarTarea(int id)
        {
            lock (Tareas)
            {
                var tarea = Tareas.FirstOrDefault(x => x.Id == id);
                if (tarea != null)
                {
                    Tareas.Remove(tarea);
                    Serializar();
                    TareaEliminada?.Invoke(tarea);
                }
            }
        }

        public void CambiarEstado(Tarea t)
        {
            if (t.Estado != Estados.Finalizado) return;
            //t.Estado = t.Estado + 1;
            t.Estado++;
            Serializar();
            TareaModificada?.Invoke(t);
        }

        public void CambiarEstadoAdmin(Tarea t, Estados estado)
        {
            t.Estado = estado;
            Serializar();
            TareaModificada?.Invoke(t);
        }

        void Serializar()
        {
            lock (Tareas)
            {
                File.WriteAllText("kanban.json", JsonSerializer.Serialize(Tareas));
            }
        }

        void Deserializar()
        {
            lock (Tareas)
            {
                try
                {

                    Tareas?.AddRange(JsonSerializer.Deserialize<List<Tarea>>(File.ReadAllText("kanban.json")) ?? []);
                    Tareas?.ForEach(x => TareaCreada?.Invoke(x));
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public void Detener()
        {
            servidor.Stop();
        }




    }
}
