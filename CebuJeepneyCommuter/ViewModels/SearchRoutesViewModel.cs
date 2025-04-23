using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CebuJeepneyCommuter.ViewModels
{
    public class SearchRoutesViewModel : INotifyPropertyChanged
    {
        private string origin;
        private string destination;

        public string Origin
        {
            get => origin;
            set { origin = value; OnPropertyChanged(); }
        }

        public string Destination
        {
            get => destination;
            set { destination = value; OnPropertyChanged(); }
        }

        public ICommand GoBackCommand { get; }
        public ICommand ShowMapCommand { get; }

        public SearchRoutesViewModel()
        {
            GoBackCommand = new Command(async () => await Application.Current.MainPage.Navigation.PopAsync());
            ShowMapCommand = new Command(OnShowMap);
        }

        private void OnShowMap()
        {
            // Logic to show map
            Application.Current.MainPage.DisplayAlert("Map", $"Showing route from {Origin} to {Destination}", "OK");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
