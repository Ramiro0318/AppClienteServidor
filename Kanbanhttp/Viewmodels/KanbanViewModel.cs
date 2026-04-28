using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kanbanhttp.Models;
using Kanbanhttp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Windows.Threading;

namespace Kanbanhttp.Viewmodels
{
    public partial class KanbanViewModel : ObservableObject
    {
        [ObservableProperty]
        private string errores = "";

        KanbanService Service = new();

        Dispatcher current;
        public KanbanViewModel()
        {
            current = Dispatcher.CurrentDispatcher;
            Service.Iniciar();
            Service.TareaCreada += Service_TareaCreada;
            Service.TareaModificada += Service_TareaModificada;
        }



        public ObservableCollection<Tarea> Pendientes { get; set; } = new();
        public ObservableCollection<Tarea> EnProceso { get; set; } = new();
        public ObservableCollection<Tarea> Hecho { get; set; } = new();
        public ObservableCollection<Tarea> Finalizado { get; set; } = new();

        [ObservableProperty]
        private string texto = "";

        [RelayCommand]
        void Agregar()
        {
            Tarea t = new Tarea();
            t.Descripcion = Texto;
            t.Usuario = "";
            t.Fecha = DateTime.Now;
            Service.CrearTarea(t);

            Texto = "";
        }

        private void Service_TareaCreada(Tarea obj)
        {
            Pendientes.Add(obj);
        }

        private void Service_TareaModificada(Tarea obj)
        {
            LlenarLista();
        }


        void LlenarLista()
        {
            current.BeginInvoke(() =>
            {
                lock (Service.Tareas)
                {
                    Pendientes.Clear();

                    foreach (var p in Service.Tareas.Where(x => x.Estado == Estados.Pendiente))
                    {
                        Pendientes.Add(p);
                    }
                    foreach (var p in Service.Tareas.Where(x => x.Estado == Estados.EnProceso))
                    {
                        EnProceso.Add(p);
                    }
                    foreach (var p in Service.Tareas.Where(x => x.Estado == Estados.Hecho))
                    {
                        Hecho.Add(p);
                    }
                    foreach (var p in Service.Tareas.Where(x => x.Estado == Estados.Finalizado))
                    {
                        Finalizado.Add(p);
                    }

                }
            });
        }




    }
}
