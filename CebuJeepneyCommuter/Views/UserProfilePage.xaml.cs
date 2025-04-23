namespace CebuJeepneyCommuter.Views;

public partial class UserProfilePage : ContentPage
{
	public UserProfilePage()
	{
		InitializeComponent();
	}
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void OnLogoutButtonClicked(object sender, EventArgs e)
    {
        // Implement logout logic here
        await DisplayAlert("Logout", "You have logged out.", "OK");
    }
}
