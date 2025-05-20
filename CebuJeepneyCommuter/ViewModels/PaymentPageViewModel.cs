using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using CebuJeepneyCommuter.Services;
using CebuJeepneyCommuter.Models;

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

        private string routeCode;
        public string RouteCode
        {
            get => routeCode;
            set { if (routeCode != value) { routeCode = value; OnPropertyChanged(); } }
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

            PayCommand = new Command(OnPay);
            GoBackCommand = new Command(async () => await Application.Current.MainPage.Navigation.PopAsync());

            _ = CalculateFare();
        }

        private async Task CalculateFare()
        {
            RouteInfo route = await RouteDataService.FindRouteAsync(From, To);

            if (route != null)
            {
                Price = route.RegularFare;
                RouteCode = route.Code; // Get actual jeepney code from data
                Discount = CalculateDiscount(PassengerType, Price);
                TotalFare = Price - Discount;
            }
            else
            {
                Price = 0;
                Discount = 0;
                TotalFare = 0;
                RouteCode = "N/A";
            }

            AddSummaryItem();
        }

        private decimal CalculateDiscount(string passengerType, decimal price)
        {
            return passengerType switch
            {
                "Senior Citizen" => price * 0.20m,
                "Student" => price * 0.10m,
                "PWD" => price * 0.15m,
                _ => 0m,
            };
        }

        private void AddSummaryItem()
        {
            SummaryItems.Clear();

            SummaryItems.Add(new SummaryItem
            {
                Code = RouteCode,
                Classification = Classification,
                PassengerType = PassengerType,
                Fare = TotalFare,
                EstimatedArrival = DateTime.Now.AddMinutes(30).ToString("hh:mm tt"),
                Date = DateTime.Now.ToString("MMM dd, yyyy")
            });
        }

        private void OnPay()
        {
            // Implement payment handling logic
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
