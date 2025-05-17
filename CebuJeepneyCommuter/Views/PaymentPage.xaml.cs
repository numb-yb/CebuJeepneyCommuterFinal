using Microsoft.Maui.Controls;

namespace CebuJeepneyCommuter.Views
{
    public partial class PaymentPage : ContentPage
    {
        public PaymentPage(string origin, string destination, string passengerType, string classification)
        {
            InitializeComponent();

            // Set the BindingContext to the ViewModel with parameters
            BindingContext = new ViewModels.PaymentPageViewModel(origin, destination, passengerType, classification);
        }
    }
}
