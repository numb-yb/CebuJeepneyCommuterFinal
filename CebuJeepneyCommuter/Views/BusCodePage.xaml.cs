using CebuJeepneyCommuter.ViewModels;

namespace CebuJeepneyCommuter.Views
{
    public partial class BusCodePage : ContentPage
    {
        public BusCodePage(string origin, string destination)
        {
            InitializeComponent();
            BindingContext = new BusCodePageViewModel(origin, destination);
        }

        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
