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

namespace CebuJeepneyCommuter.ViewModels
{
    public class SearchRoutesViewModel : INotifyPropertyChanged
    {
        private string origin;
        private string destination;
        private List<RouteInfo> allRoutes;
        private ObservableCollection<string> originSuggestions;
        private ObservableCollection<string> destinationSuggestions;

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

        public string Origin
        {
            get => origin;
            set
            {
                if (origin != value)
                {
                    origin = value;
                    OnPropertyChanged();
                    FilterOriginSuggestions();
                    FilterDestinationSuggestions();
                }
            }
        }

        public string Destination
        {
            get => destination;
            set
            {
                destination = value;
                OnPropertyChanged();
                FilterDestinationSuggestions();
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
            ShowMapCommand = new Command(OnShowMap); // Existing "PROCEED" button
            ShowOnMapCommand = new Command(OnShowOnMap); // New red "SHOW ON MAP" button
            SelectOriginCommand = new Command<string>(text => Origin = text);
            SelectDestinationCommand = new Command<string>(text => Destination = text);

            LoadRoutes();
            OriginSuggestions = new ObservableCollection<string>();
            DestinationSuggestions = new ObservableCollection<string>();
        }

        private void LoadRoutes()
        {
            var rawRoutes = new List<string>
            {
                // JEEPNEYS
                "Basak - Colon", "Bulacao - SM via Colon", "Bulacao - Ayala via Jones Ave",
                "Labangon - Carbon via Jones", "Labangon - Colon", "Urgello - IT Park via Jones",
                "Lahug - Colon via Jones", "Guadalupe - Carbon", "Guadalupe - SM via Mango",
                "Minglanilla - Carbon", "Minglanilla - SM City Cebu", "Tabunok - Carbon",
                "Tabunok - Ayala via SRP", "Bulacao - Ayala via SRP", "Lahug - Carbon via Jones Ave",

                // MYBUS
                "Talisay (SM Seaside) - SM City - IT Park", "Minglanilla - SM City Cebu",

                // BEEP
                "Minglanilla - Ayala via SRP", "Talisay - IT Park via SM City",
                "Bulacao - Ayala via Colon", "Lawaan - SM City - Ayala", "Minglanilla - Colon"
            };

            allRoutes = rawRoutes.Select(desc =>
            {
                var parts = desc.Split(" - ");
                return new RouteInfo
                {
                    Origin = parts[0].Trim(),
                    Destination = parts.Length > 1 ? parts[1].Trim() : string.Empty
                };
            }).ToList();
        }

        private void FilterOriginSuggestions()
        {
            var query = Origin?.ToLower() ?? "";
            var matches = allRoutes
                .Select(r => r.Origin)
                .Distinct()
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
            var filtered = allRoutes
                .Where(r => string.IsNullOrEmpty(Origin) || r.Origin.Equals(Origin, StringComparison.OrdinalIgnoreCase))
                .Select(r => r.Destination)
                .Distinct()
                .Where(d => d.ToLower().StartsWith(query))
                .OrderBy(d => d)
                .ToList();

            DestinationSuggestions.Clear();
            foreach (var match in filtered)
                DestinationSuggestions.Add(match);
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

        private void OnShowOnMap()
        {
            Application.Current.MainPage.DisplayAlert("Preview Route", $"Map preview for:\n{Origin} ➝ {Destination}", "OK");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
