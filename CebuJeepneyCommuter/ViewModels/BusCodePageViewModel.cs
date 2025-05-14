using CebuJeepneyCommuter.Services;
using CebuJeepneyCommuter.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CebuJeepneyCommuter.ViewModels
{
    public class BusCodePageViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> BusTypes { get; } = new() { "MyBus", "Jeepney", "Beep" };
        public ObservableCollection<string> Classifications { get; } = new() { "Economy", "Regular" };
        public ObservableCollection<string> AvailableBuses { get; } = new();

        private string selectedBusType;
        public string SelectedBusType
        {
            get => selectedBusType;
            set { selectedBusType = value; OnPropertyChanged(); }
        }

        private string selectedClassification;
        public string SelectedClassification
        {
            get => selectedClassification;
            set { selectedClassification = value; OnPropertyChanged(); }
        }

        private string selectedAvailableBus;
        public string SelectedAvailableBus
        {
            get => selectedAvailableBus;
            set { selectedAvailableBus = value; OnPropertyChanged(); }
        }

        public ICommand ShowMapCommand { get; }
        public ICommand GoBackCommand { get; }

        public BusCodePageViewModel(string origin, string destination)
        {
            ShowMapCommand = new Command(ExecuteShowMap);
            GoBackCommand = new Command(async () => await Application.Current.MainPage.Navigation.PopAsync());

            LoadRouteData(origin, destination);
        }

        private void LoadRouteData(string origin, string destination)
        {
            var route = RouteDataService.FindRoute(origin, destination);

            if (route != null)
            {
                SelectedBusType = route.Type;
                SelectedClassification = route.Classification;
                AvailableBuses.Clear();
                AvailableBuses.Add(route.Code);
                SelectedAvailableBus = route.Code;
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Route not found", "No route matches your selection.", "OK");
            }
        }

        private void ExecuteShowMap()
        {
            Application.Current.MainPage.DisplayAlert(
                "Map Info",
                $"Showing: {SelectedBusType}, {SelectedClassification}, {SelectedAvailableBus}",
                "OK");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
