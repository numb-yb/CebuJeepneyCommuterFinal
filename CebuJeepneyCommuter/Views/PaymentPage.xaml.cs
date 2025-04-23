using CebuJeepneyCommuter.ViewModels;
using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;

namespace CebuJeepneyCommuter.Views
{
    public partial class PaymentPage : ContentPage
    {
        public PaymentPage()
        {
            InitializeComponent();
         
        }
        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            // Navigate to UserHomePage
            await Navigation.PushAsync(new UserHomePage());
        }
        private void OnPaymentButtonClicked(object sender, EventArgs e)
        {
            // Handle payment processing logic here
            // For example, you can navigate to another page or show a message
            DisplayAlert("Payment", "Payment button clicked!", "OK");
        }

    }

}
