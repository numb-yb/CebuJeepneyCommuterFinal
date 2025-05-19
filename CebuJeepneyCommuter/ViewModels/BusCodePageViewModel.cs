using CebuJeepneyCommuter.Services;
using CebuJeepneyCommuter.Views;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CebuJeepneyCommuter.ViewModels
{
    public class BusCodePageViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> PassengerTypes { get; } = new()
        {
            "Regular",
            "Senior Citizen",
            "Student",
            "PWD"
        };

        public ObservableCollection<string> BusTypes { get; } = new() { "Jeepney", "MyBus", "Beep" };

        public ObservableCollection<string> Classifications { get; } = new() { "Regular", "Economy", "Express", "Special" };

        public ObservableCollection<string> RouteCodes { get; } = new();

        private string selectedPassengerType;
        public string SelectedPassengerType
        {
            get => selectedPassengerType;
            set
            {
                if (selectedPassengerType != value)
                {
                    selectedPassengerType = value;
                    OnPropertyChanged();
                }
            }
        }

        private string selectedBusType;
        public string SelectedBusType
        {
            get => selectedBusType;
            set
            {
                if (selectedBusType != value)
                {
                    selectedBusType = value;
                    OnPropertyChanged();
                }
            }
        }

        private string selectedClassification;
        public string SelectedClassification
        {
            get => selectedClassification;
            set
            {
                if (selectedClassification != value)
                {
                    selectedClassification = value;
                    OnPropertyChanged();
                }
            }
        }

        private string selectedRouteCode;
        public string SelectedRouteCode
        {
            get => selectedRouteCode;
            set
            {
                if (selectedRouteCode != value)
                {
                    selectedRouteCode = value;
                    OnPropertyChanged();
                }
            }
        }

        private readonly string origin;
        private readonly string destination;

        public ICommand ShowMapCommand { get; }

        public BusCodePageViewModel(string origin, string destination)
        {
            this.origin = origin;
            this.destination = destination;

            SelectedPassengerType = "Regular";

            _ = LoadRouteAsync(origin, destination);

            ShowMapCommand = new Command(OnShowMap);
        }

        private async Task LoadRouteAsync(string origin, string destination)
        {
            var route = await RouteDataService.FindRouteAsync(origin, destination);

            if (route != null)
            {
                SelectedBusType = route.Type;
                SelectedClassification = route.Classification;

                RouteCodes.Add(route.Code);
                SelectedRouteCode = route.Code;
            }
            else
            {
                SelectedBusType = BusTypes[0];
                SelectedClassification = Classifications[0];
            }
        }

        private async void OnShowMap()
        {
            var paymentPage = new PaymentPage(origin, destination, SelectedPassengerType, SelectedClassification);
            paymentPage.BindingContext = new PaymentPageViewModel(origin, destination, SelectedPassengerType, SelectedClassification);
            await Application.Current.MainPage.Navigation.PushAsync(paymentPage);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
