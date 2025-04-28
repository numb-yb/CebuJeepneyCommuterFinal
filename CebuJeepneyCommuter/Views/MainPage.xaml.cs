using Microsoft.Maui.Controls;

namespace CebuJeepneyCommuter.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        // Navigate to the registration page
        private async void OnSignUpTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }

        // Navigate to the admin login page
        private async void OnAdminLoginTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AdminLoginPage());
        }
        private async void OnSignInClicked(object sender, EventArgs e)
        {
            // Optionally validate credentials here...

            // Navigate to UserHomePage
            await Navigation.PushAsync(new UserHomePage());
        }
      

    }
}