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
            var email = EmailEntry.Text;
            var password = PasswordEntry.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Error", "Please enter both email and password.", "OK");
                return;
            }

            var user = await _userService.GetUserByLoginAsync(email, password);

            if (user != null)
            {
                // ✅ Save the logged-in user to current_user.json
                await _userService.SaveLoggedInUserAsync(user);

                // ✅ Navigate to the homepage
                await Navigation.PushAsync(new UserHomePage());
            }
            else
            {
                await DisplayAlert("Login Failed", "Incorrect email or password.", "OK");
            }
        }
    }
}
