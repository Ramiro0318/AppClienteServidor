using Kanbanhttp.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Kanbanhttp.Services
{
    public class KanbanService
    {
        public List<Tarea> Tareas { get; set; } = new();
        HttpListener servidor = new();

        public KanbanService()
        {
            string url = "http://*:8080/kanban/";
            servidor.Prefixes.Add(url);
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

        }

        public event Action<Tarea>? TareaCreada;
        public event Action<Tarea>? TareaEliminada;
        public void CrearTarea(Tarea t)
        {
            lock (Tareas)
            {
                t.Id = Tareas.Max(x => x.Id) + 1;
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

        void Serializar()
        {

        }

        public void Detener()
        {
            servidor.Stop();
        }




    }
}
