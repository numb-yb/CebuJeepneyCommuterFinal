using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CebuJeepneyCommuter.Models;
using CebuJeepneyCommuter.Services;
using System.Linq;
using System.Threading.Tasks;

namespace CebuJeepneyCommuter.ViewModels
{
    public class AdminHomeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<string> VehicleTypes { get; set; }
        public ObservableCollection<string> Stops { get; set; }

        private readonly Dictionary<string, List<string>> vehicleStops = new Dictionary<string, List<string>>
        {
            { "Jeepney", new List<string> {
                "Basak", "Colon", "Bulacao", "SM via Colon", "Ayala via Jones Ave", "Labangon",
                "Carbon via Jones", "Urgello", "IT Park via Jones", "Lahug", "Colon via Jones",
                "Guadalupe", "Carbon", "SM via Mango", "Minglanilla", "SM City Cebu",
                "Tabunok", "Ayala via SRP", "Carbon via Jones Ave"
            }},
            { "MyBus", new List<string> {
                "Talisay (SM Seaside)", "SM City", "IT Park", "Minglanilla", "SM City Cebu"
            }},
            { "Beep", new List<string> {
                "Minglanilla", "Ayala via SRP", "Talisay", "IT Park via SM City",
                "Bulacao", "Ayala via Colon", "Lawaan", "SM City", "Ayala", "Colon"
            }},
        };

        private List<RouteInfo> allRoutes;
        private ObservableCollection<RouteInfo> filteredRoutes;
        public ObservableCollection<RouteInfo> FilteredRoutes
        {
            get => filteredRoutes;
            set
            {
                filteredRoutes = value;
                OnPropertyChanged();
            }
        }

        public ICommand UpdateRateCommand { get; }
        public ICommand SaveRouteCommand { get; }

        public AdminHomeViewModel()
        {
            VehicleTypes = new ObservableCollection<string> { "Jeepney", "Beep", "MyBus" };
            FilteredRoutes = new ObservableCollection<RouteInfo>();
            Stops = new ObservableCollection<string>();

            UpdateRateCommand = new Command(async () => await OnUpdateRateAsync());
            SaveRouteCommand = new Command(async () => await OnSaveRouteAsync());

            _ = LoadRoutesAsync();
        }

        private async Task LoadRoutesAsync()
        {
            allRoutes = await RouteDataService.GetAllRoutesAsync();
            FilterRoutesByVehicleType();
        }

        private string selectedVehicleType;
        public string SelectedVehicleType
        {
            get => selectedVehicleType;
            set
            {
                if (selectedVehicleType != value)
                {
                    selectedVehicleType = value;
                    OnPropertyChanged();
                    FilterRoutesByVehicleType();
                }
            }
        }

        private RouteInfo selectedRoute;
        public RouteInfo SelectedRoute
        {
            get => selectedRoute;
            set
            {
                selectedRoute = value;
                OnPropertyChanged();
            }
        }

        private decimal newRate;
        public decimal NewRate
        {
            get => newRate;
            set
            {
                newRate = value;
                OnPropertyChanged();
            }
        }

        private void FilterRoutesByVehicleType()
        {
            if (allRoutes == null) return;

            var filtered = allRoutes
                .Where(r => r.Type.Equals(SelectedVehicleType, System.StringComparison.OrdinalIgnoreCase))
                .ToList();

            FilteredRoutes = new ObservableCollection<RouteInfo>(filtered);

            var stopsForType = filtered
                .SelectMany(r => new[] { r.Origin, r.Destination })
                .Distinct()
                .OrderBy(stop => stop)
                .ToList();

            Stops.Clear();
            foreach (var stop in stopsForType)
            {
                Stops.Add(stop);
            }
        }

        private async Task OnUpdateRateAsync()
        {
            if (SelectedRoute != null)
            {
                SelectedRoute.RegularFare = NewRate;

                await RouteDataService.SaveRoutesAsync(allRoutes);

                await App.Current.MainPage.DisplayAlert("Success", $"Updated rate for {SelectedRoute.Code} to ₱{NewRate}", "OK");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Please select a route", "OK");
            }
        }

        // -------------------------------
        // Create Route Section
        // -------------------------------

        public string NewRouteCode { get; set; }

        private string selectedCreateRouteVehicleType;
        public string SelectedCreateRouteVehicleType
        {
            get => selectedCreateRouteVehicleType;
            set
            {
                if (selectedCreateRouteVehicleType != value)
                {
                    selectedCreateRouteVehicleType = value;
                    OnPropertyChanged();
                    UpdateStopsForSelectedCreateVehicle();
                }
            }
        }

        private void UpdateStopsForSelectedCreateVehicle()
        {
            if (!string.IsNullOrEmpty(SelectedCreateRouteVehicleType) && vehicleStops.ContainsKey(SelectedCreateRouteVehicleType))
            {
                var relevantStops = vehicleStops[SelectedCreateRouteVehicleType]
                    .Distinct()
                    .OrderBy(s => s)
                    .ToList();

                Stops.Clear();
                foreach (var stop in relevantStops)
                    Stops.Add(stop);
            }
            else
            {
                Stops.Clear();
            }
        }

        public decimal NewRouteMinimumFare { get; set; }

        private string selectedStop1;
        public string SelectedStop1
        {
            get => selectedStop1;
            set
            {
                selectedStop1 = value;
                OnPropertyChanged();
            }
        }

        private string selectedStop2;
        public string SelectedStop2
        {
            get => selectedStop2;
            set
            {
                selectedStop2 = value;
                OnPropertyChanged();
            }
        }

        private async Task OnSaveRouteAsync()
        {
            if (string.IsNullOrWhiteSpace(NewRouteCode) ||
                string.IsNullOrWhiteSpace(SelectedCreateRouteVehicleType) ||
                string.IsNullOrWhiteSpace(SelectedStop1) ||
                string.IsNullOrWhiteSpace(SelectedStop2) ||
                NewRouteMinimumFare <= 0)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Please fill in all fields correctly.", "OK");
                return;
            }

            var newRoute = new RouteInfo
            {
                Code = NewRouteCode.Trim(),
                Type = SelectedCreateRouteVehicleType,
                Origin = SelectedStop1,
                Destination = SelectedStop2,
                RegularFare = NewRouteMinimumFare
            };

            allRoutes.Add(newRoute);

            await RouteDataService.SaveRoutesAsync(allRoutes);

            FilterRoutesByVehicleType();

            await App.Current.MainPage.DisplayAlert("Success", $"Route '{newRoute.Code}' created successfully.", "OK");

            // Clear input fields
            NewRouteCode = string.Empty;
            SelectedStop1 = null;
            SelectedStop2 = null;
            NewRouteMinimumFare = 0;

            OnPropertyChanged(nameof(NewRouteCode));
            OnPropertyChanged(nameof(SelectedStop1));
            OnPropertyChanged(nameof(SelectedStop2));
            OnPropertyChanged(nameof(NewRouteMinimumFare));
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
