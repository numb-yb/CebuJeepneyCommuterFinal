using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;

namespace CebuJeepneyCommuter.Views
{
    public partial class UserHomePage : ContentPage
    {
        public UserHomePage()
        {
            InitializeComponent();
      
        }
        private async void OnProceedToPaymentClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PaymentPage("Cebu", "Mandaue", "Student", "Regular"));
        }
        private async void OnSearchRoutesClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SearchRoutesPage());
        }
        private async void OnBusCodeClicked(object sender, EventArgs e)
        {
        await Navigation.PushAsync(new BusCodePage("SampleOrigin", "SampleDestination"));
        }
        private async void OnProfileTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UserProfilePage());
        }
    }
}
