using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CebuJeepneyCommuter.ViewModels
{
    public class BusCodePageViewModel : INotifyPropertyChanged
    {
        // Bus type options
        public ObservableCollection<string> BusTypes { get; } = new()
        {
            "MyBus", "Jeepney", "Beep"
        };

        // Classification options
        public ObservableCollection<string> Classifications { get; } = new()
        {
            "Economy", "Regular"
        };

        // Available bus codes
        public ObservableCollection<string> AvailableBuses { get; } = new()
        {
            "Code 1", "Code 2"
        };

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

        private string selectedAvailableBus;
        public string SelectedAvailableBus
        {
            get => selectedAvailableBus;
            set
            {
                if (selectedAvailableBus != value)
                {
                    selectedAvailableBus = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand ShowMapCommand { get; }
        public ICommand GoBackCommand { get; }

        public BusCodePageViewModel()
        {
            ShowMapCommand = new Command(ExecuteShowMap);
            GoBackCommand = new Command(ExecuteGoBack);
        }

        private void ExecuteShowMap()
        {
            Application.Current.MainPage.DisplayAlert(
                "Map Info",
                $"Showing: {SelectedBusType}, {SelectedClassification}, {SelectedAvailableBus}",
                "OK");
        }

        private async void ExecuteGoBack()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
