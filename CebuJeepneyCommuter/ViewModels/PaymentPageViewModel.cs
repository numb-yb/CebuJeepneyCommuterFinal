using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace CebuJeepneyCommuter.ViewModels
{
    public class SummaryItem
    {
        public string Code { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string Fare { get; set; }
        public string EstimatedArrival { get; set; }
        public string Date { get; set; }
    }

    public class PaymentPageViewModel : INotifyPropertyChanged
    {
        private string destination;
        private string from;
        private string to;
        private string price;
        private string totalFare;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Destination
        {
            get => destination;
            set
            {
                if (destination != value)
                {
                    destination = value;
                    OnPropertyChanged();
                    UpdateTotalFare();
                }
            }
        }

        public string From
        {
            get => from;
            set
            {
                if (from != value)
                {
                    from = value;
                    OnPropertyChanged();
                }
            }
        }

        public string To
        {
            get => to;
            set
            {
                if (to != value)
                {
                    to = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Price
        {
            get => price;
            set
            {
                if (price != value)
                {
                    price = value;
                    OnPropertyChanged();
                    UpdateTotalFare();
                }
            }
        }

        public string TotalFare
        {
            get => totalFare;
            set
            {
                if (totalFare != value)
                {
                    totalFare = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<SummaryItem> SummaryItems { get; set; }

        public ICommand PayCommand { get; }
        public ICommand GoBackCommand { get; }

        public PaymentPageViewModel()
        {
            SummaryItems = new ObservableCollection<SummaryItem>
            {
                new SummaryItem
                {
                    Code = "J101",
                    Origin = "Ayala",
                    Destination = "Colon",
                    Fare = "₱12",
                    EstimatedArrival = "10:30 AM",
                    Date = "Apr 10"
                }
            };

            PayCommand = new Command(OnPay);
            GoBackCommand = new Command(OnGoBack);
        }

        private void OnPay()
        {
            // Handle payment logic here
            Application.Current.MainPage.DisplayAlert("Payment", "Payment successful!", "OK");
        }

        private async void OnGoBack()
        {
            // Navigation logic for going back (only works if using Shell or NavigationPage)
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private void UpdateTotalFare()
        {
            if (decimal.TryParse(price, out decimal priceValue))
            {
                TotalFare = $"₱{priceValue:F2}";
            }
            else
            {
                TotalFare = "₱0.00";
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
