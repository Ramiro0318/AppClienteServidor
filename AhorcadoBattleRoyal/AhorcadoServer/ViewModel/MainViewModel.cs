using AhorcadoServer.Models;
using AhorcadoServer.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Threading;

namespace AhorcadoServer.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private bool _modalNuevaRondaVisible;

        public bool ModalNuevaRondaVisible
        {
            get => _modalNuevaRondaVisible;
            set { _modalNuevaRondaVisible = value; OnPropertyChanged(); }
        }

        AhorcadoService service = new();
        public ObservableCollection<Cliente> Clientes { get; set; } = [];
        public ObservableCollection<string> Logs { get; set; } = [];
        public bool SalaCerrada { get; set; } = true;
        public ICommand MostrarModalNuevaRondaCommand { get; }
        public ICommand AbrirSalaCommand { get; }
        public ICommand CancelarNuevaRondaCommand { get; }
        public ICommand IniciarNuevaRondaCommand { get; }

        Dispatcher hiloUI;
        public MainViewModel()
        {
            hiloUI = Dispatcher.CurrentDispatcher;
            MostrarModalNuevaRondaCommand = new RelayCommand(MostrarModalNuevaRonda);
            AbrirSalaCommand = new RelayCommand(AbrirSala);
            CancelarNuevaRondaCommand = new RelayCommand(CancelarNuevaRonda);
            IniciarNuevaRondaCommand = new RelayCommand(IniciarNuevaRonda);

            service.LogEnviado += Service_LogEnviado;
            service.ClienteConectado += Service_ClienteConectado;
        }

        
        private void Service_ClienteConectado(Cliente obj)
        {
            App.Current.Dispatcher.BeginInvoke(() =>
            {
                Clientes.Add(obj);
            });
        }

        private void Service_LogEnviado(string obj)
        {
            hiloUI.BeginInvoke(() =>
            {
                Logs.Add(obj);
            });
        }

        private void MostrarModalNuevaRonda(object? parameter = null)
        {
            ModalNuevaRondaVisible = true;
        }

        private void AbrirSala(object? parameter = null)
        {
            service.AbrirSala();
            SalaCerrada = false;
            OnPropertyChanged(nameof(SalaCerrada));
        }

        private void CancelarNuevaRonda(object? parameter = null)
        {
            ModalNuevaRondaVisible = false;
        }

        private void IniciarNuevaRonda(object? parameter)
        {
            string frase = parameter as string ?? "";
            ModalNuevaRondaVisible = false;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}