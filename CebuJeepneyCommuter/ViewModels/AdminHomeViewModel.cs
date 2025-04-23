using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace CebuJeepneyCommuter.ViewModels
{
    public class AdminHomeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // View state properties
        private bool _isMinimumRateVisible = true;
        public bool IsMinimumRateVisible
        {
            get => _isMinimumRateVisible;
            set => SetProperty(ref _isMinimumRateVisible, value);
        }

        private bool _isCreateRouteVisible = false;
        public bool IsCreateRouteVisible
        {
            get => _isCreateRouteVisible;
            set => SetProperty(ref _isCreateRouteVisible, value);
        }

        // Data binding for pickers
        public ObservableCollection<string> VehicleTypes { get; set; } = new()
        {
            "Jeepney", "Beep", "Mybus"
        };

        public ObservableCollection<string> Routes { get; set; } = new()
        {
            "Route A", "Route B", "Route C"
        };

        // Form entries
        private string _selectedVehicleType;
        public string SelectedVehicleType
        {
            get => _selectedVehicleType;
            set => SetProperty(ref _selectedVehicleType, value);
        }

        private string _selectedRoute;
        public string SelectedRoute
        {
            get => _selectedRoute;
            set => SetProperty(ref _selectedRoute, value);
        }

        private string _minimumRate;
        public string MinimumRate
        {
            get => _minimumRate;
            set => SetProperty(ref _minimumRate, value);
        }

        private string _routeCode;
        public string RouteCode
        {
            get => _routeCode;
            set => SetProperty(ref _routeCode, value);
        }

        private string _startStop;
        public string StartStop
        {
            get => _startStop;
            set => SetProperty(ref _startStop, value);
        }

        private string _endStop;
        public string EndStop
        {
            get => _endStop;
            set => SetProperty(ref _endStop, value);
        }

        private string _newMinimumPrice;
        public string NewMinimumPrice
        {
            get => _newMinimumPrice;
            set => SetProperty(ref _newMinimumPrice, value);
        }

        // Commands
        public ICommand ShowMinimumRateCommand => new Command(() =>
        {
            IsMinimumRateVisible = true;
            IsCreateRouteVisible = false;
        });

        public ICommand ShowCreateRouteCommand => new Command(() =>
        {
            IsMinimumRateVisible = false;
            IsCreateRouteVisible = true;
        });

        public ICommand SaveRouteCommand => new Command(() =>
        {
            if (string.IsNullOrWhiteSpace(RouteCode) ||
                string.IsNullOrWhiteSpace(StartStop) ||
                string.IsNullOrWhiteSpace(EndStop) ||
                string.IsNullOrWhiteSpace(NewMinimumPrice) ||
                string.IsNullOrWhiteSpace(SelectedVehicleType))
            {
                Application.Current.MainPage.DisplayAlert("Error", "Please fill in all required fields.", "OK");
                return;
            }

            var newRoute = $"{RouteCode} - {StartStop} to {EndStop} ({SelectedVehicleType}) - ₱{NewMinimumPrice}";
            Routes.Add(newRoute);

            Application.Current.MainPage.DisplayAlert("Success", "Route saved successfully!", "OK");

            // Clear form fields
            RouteCode = StartStop = EndStop = NewMinimumPrice = null;
        });

        public ICommand UpdateRateCommand => new Command(() =>
        {
            if (string.IsNullOrWhiteSpace(SelectedVehicleType) ||
                string.IsNullOrWhiteSpace(SelectedRoute) ||
                string.IsNullOrWhiteSpace(MinimumRate))
            {
                Application.Current.MainPage.DisplayAlert("Error", "Please select a route and vehicle type, and enter a new minimum rate.", "OK");
                return;
            }

            Application.Current.MainPage.DisplayAlert("Success", $"Minimum rate for {SelectedRoute} ({SelectedVehicleType}) updated to ₱{MinimumRate}.", "OK");

            // Clear minimum rate
            MinimumRate = null;
        });

        // Helper methods
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value)) return;

            backingStore = value;
            OnPropertyChanged(propertyName);
        }
    }
}
