using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CebuJeepneyCommuter.Models;
using Microsoft.Maui.Controls;
using CebuJeepneyCommuter.Views;
using CebuJeepneyCommuter.Services;

namespace CebuJeepneyCommuter.ViewModels
{
    public class SearchRoutesViewModel : INotifyPropertyChanged
    {
        private string origin;
        private string destination;
        private List<RouteInfo> allRoutes;
        private ObservableCollection<string> originSuggestions;
        private ObservableCollection<string> destinationSuggestions;
        private ObservableCollection<RouteInfo> matchingRoutes;

        public ObservableCollection<string> OriginSuggestions
        {
            get => originSuggestions;
            set { originSuggestions = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> DestinationSuggestions
        {
            get => destinationSuggestions;
            set { destinationSuggestions = value; OnPropertyChanged(); }
        }

        public ObservableCollection<RouteInfo> MatchingRoutes
        {
            get => matchingRoutes;
            set { matchingRoutes = value; OnPropertyChanged(); }
        }

        public string Origin
        {
            get => origin;
            set
            {
                if (origin != value)
                {
                    origin = value?.Trim();
                    OnPropertyChanged();
                    FilterOriginSuggestions();
                    FilterDestinationSuggestions();
                    UpdateMatchingRoutes();
                }
            }
        }

        public string Destination
        {
            get => destination;
            set
            {
                if (destination != value)
                {
                    destination = value?.Trim();
                    OnPropertyChanged();
                    FilterDestinationSuggestions();
                    UpdateMatchingRoutes();
                }
            }
        }

        public ICommand SelectOriginCommand { get; }
        public ICommand SelectDestinationCommand { get; }
        public ICommand GoBackCommand { get; }
        public ICommand ShowMapCommand { get; }
        public ICommand ShowOnMapCommand { get; }

        public SearchRoutesViewModel()
        {
            GoBackCommand = new Command(async () => await Application.Current.MainPage.Navigation.PopAsync());
            ShowMapCommand = new Command(OnShowMap);
            ShowOnMapCommand = new Command(OnShowOnMap);
            SelectOriginCommand = new Command<string>(text => Origin = text);
            SelectDestinationCommand = new Command<string>(text => Destination = text);

            OriginSuggestions = new ObservableCollection<string>();
            DestinationSuggestions = new ObservableCollection<string>();
            MatchingRoutes = new ObservableCollection<RouteInfo>();

            LoadRoutesAsync();

            // ✅ Subscribe to route updates
            MessagingCenter.Subscribe<object>(this, "RoutesUpdated", async (sender) =>
            {
                await RefreshRoutesAsync();
            });
        }

        private async void LoadRoutesAsync()
        {
            allRoutes = await RouteDataService.GetAllRoutesAsync();
            FilterOriginSuggestions();
            FilterDestinationSuggestions();
            UpdateMatchingRoutes();
        }

        private void FilterOriginSuggestions()
        {
            var query = Origin?.ToLower() ?? "";
            var matches = allRoutes
                .Select(r => r.Origin?.Trim())
                .Where(o => !string.IsNullOrWhiteSpace(o))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Where(o => o.ToLower().StartsWith(query))
                .OrderBy(o => o)
                .ToList();

            OriginSuggestions.Clear();
            foreach (var match in matches)
                OriginSuggestions.Add(match);
        }

        private void FilterDestinationSuggestions()
        {
            var query = Destination?.ToLower() ?? "";
            var matches = allRoutes
                .Where(r => string.IsNullOrEmpty(Origin) || r.Origin.Equals(Origin, StringComparison.OrdinalIgnoreCase))
                .Select(r => r.Destination?.Trim())
                .Where(d => !string.IsNullOrWhiteSpace(d))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Where(d => d.ToLower().StartsWith(query))
                .OrderBy(d => d)
                .ToList();

            DestinationSuggestions.Clear();
            foreach (var match in matches)
                DestinationSuggestions.Add(match);
        }

        private void UpdateMatchingRoutes()
        {
            if (string.IsNullOrWhiteSpace(Origin) || string.IsNullOrWhiteSpace(Destination))
            {
                MatchingRoutes.Clear();
                return;
            }

            var matches = allRoutes
                .Where(r =>
                    r.Origin.Equals(Origin, StringComparison.OrdinalIgnoreCase) &&
                    r.Destination.Equals(Destination, StringComparison.OrdinalIgnoreCase))
                .OrderBy(r => r.Type)
                .ToList();

            MatchingRoutes = new ObservableCollection<RouteInfo>(matches);
        }

        private async void OnShowMap()
        {
            if (!string.IsNullOrEmpty(Origin) && !string.IsNullOrEmpty(Destination))
            {
                await Application.Current.MainPage.Navigation.PushAsync(
                    new BusCodePage(Origin, Destination));
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please select both Origin and Destination", "OK");
            }
        }

        private async void OnShowOnMap()
        {
            if (!string.IsNullOrEmpty(Origin) && !string.IsNullOrEmpty(Destination))
            {
                await Application.Current.MainPage.Navigation.PushAsync(new MapUI(Origin, Destination));
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please select both Origin and Destination", "OK");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        // ✅ This is already present, used by the MessagingCenter
        public async Task RefreshRoutesAsync()
        {
            allRoutes = await RouteDataService.GetAllRoutesAsync();
            FilterOriginSuggestions();
            FilterDestinationSuggestions();
            UpdateMatchingRoutes();
        }
    }
}
