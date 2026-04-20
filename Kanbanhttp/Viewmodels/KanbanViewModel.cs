using CommunityToolkit.Mvvm.ComponentModel;
using Kanbanhttp.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kanbanhttp.Viewmodels
{
    public partial class KanbanViewModel : ObservableObject
    {
        [ObservableProperty]
        private string errores = "";

        KanbanService Service = new();
        public KanbanViewModel() 
        {
            Service.Iniciar();
        }
    }
}
