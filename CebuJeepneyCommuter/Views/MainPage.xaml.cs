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
        private async void OnGoogleSignInClicked(object sender, EventArgs e)
        {
            try
            {
                var authResult = await WebAuthenticator.AuthenticateAsync(
                    new Uri("https://accounts.google.com/o/oauth2/v2/auth" +
                        "?client_id=YOUR_CLIENT_ID" +
                        "&redirect_uri=your-app-scheme:/oauth2redirect" +
                        "&response_type=code" +
                        "&scope=openid%20email%20profile"),
                    new Uri("your-app-scheme:/oauth2redirect"));

                var accessToken = authResult?.AccessToken;

                if (!string.IsNullOrEmpty(accessToken))
                {
                    // ✅ Auth success, navigate or fetch user info
                    await DisplayAlert("Success", "Google Sign-In Successful!", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Sign-in failed: {ex.Message}", "OK");
            }
        }

    }
}