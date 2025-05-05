using Microsoft.Maui.Controls;
using CebuJeepneyCommuter.Services;
using CebuJeepneyCommuter.Models;

namespace CebuJeepneyCommuter.Views
{
    public partial class MainPage : ContentPage
    {
        private readonly UserService _userService;

        public MainPage()
        {
            InitializeComponent();
            _userService = new UserService(); // Initialize the user service
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
            var email = EmailEntry.Text; // Get email from the Entry
            var password = PasswordEntry.Text; // Get password from the Entry

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Error", "Please enter both email and password.", "OK");
                return;
            }

            // Check if user exists and password matches
            bool loginSuccess = await _userService.ValidateUserLoginAsync(email, password);

            if (loginSuccess)
            {
                // Navigate to the UserHomePage if login is successful
                await Navigation.PushAsync(new UserHomePage());
            }
            else
            {
                // Show error message if credentials are incorrect
                await DisplayAlert("Login Failed", "Incorrect email or password.", "OK");
            }
        }

    }
}
