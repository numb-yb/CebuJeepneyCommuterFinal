using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using CebuJeepneyCommuter.Services;

namespace CebuJeepneyCommuter.ViewModels
{
    public class PaymentPageViewModel : INotifyPropertyChanged
    {
        public string From { get; set; }
        public string To { get; set; }
        public string PassengerType { get; set; }
        public string Classification { get; set; }

        private decimal price;
        public decimal Price
        {
            get => price;
            set { if (price != value) { price = value; OnPropertyChanged(); } }
        }

        private decimal discount;
        public decimal Discount
        {
            get => discount;
            set { if (discount != value) { discount = value; OnPropertyChanged(); } }
        }

        private decimal totalFare;
        public decimal TotalFare
        {
            get => totalFare;
            set { if (totalFare != value) { totalFare = value; OnPropertyChanged(); } }
        }

        public ObservableCollection<SummaryItem> SummaryItems { get; set; } = new();

        public ICommand PayCommand { get; }
        public ICommand GoBackCommand { get; }

        public PaymentPageViewModel(string origin, string destination, string passengerType, string classification)
        {
            From = origin;
            To = destination;
            PassengerType = passengerType;
            Classification = classification;

            CalculateFare();

            PayCommand = new Command(OnPay);
            GoBackCommand = new Command(async () => await Application.Current.MainPage.Navigation.PopAsync());

            AddSummaryItem();
        }

        private void CalculateFare()
        {
            var route = RouteDataService.FindRoute(From, To);

            if (route != null)
            {
                Price = route.RegularFare;

                // Calculate discount according to PassengerType
                Discount = PassengerType switch
                {
                    "Senior Citizen" => Price * 0.20m,
                    "Student" => Price * 0.10m,
                    "PWD" => Price * 0.15m,
                    _ => 0m,
                };

                TotalFare = Price - Discount;
            }
            else
            {
                Price = 0;
                Discount = 0;
                TotalFare = 0;
            }
        }

        private void AddSummaryItem()
        {
            SummaryItems.Clear();

            SummaryItems.Add(new SummaryItem
            {
                Code = $"{From}-{To}",
                Classification = Classification,
                PassengerType = PassengerType,
                Fare = TotalFare,
                EstimatedArrival = DateTime.Now.AddMinutes(30).ToString("hh:mm tt"),
                Date = DateTime.Now.ToString("MMM dd, yyyy")
            });
        }

        private void OnPay()
        {
            // Implement payment processing logic if needed
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public class SummaryItem
    {
        public string Code { get; set; }
        public string Classification { get; set; }
        public string PassengerType { get; set; }
        public decimal Fare { get; set; }
        public string EstimatedArrival { get; set; }
        public string Date { get; set; }
    }
}
