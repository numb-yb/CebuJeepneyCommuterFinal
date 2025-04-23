namespace CebuJeepneyCommuter.Views;

public partial class AdminHomePage : ContentPage
{
    public AdminHomePage()
    {
        InitializeComponent();
    }

    private void OnCreateNewRouteClicked(object sender, EventArgs e)
    {
        MinimumRateView.IsVisible = false;
        CreateRouteView.IsVisible = true;
    }

    private void OnChangeMinimumRateClicked(object sender, EventArgs e)
    {
        CreateRouteView.IsVisible = false;
        MinimumRateView.IsVisible = true;
    }
    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        // Navigate to login page (replace with your actual login page name)
        await Navigation.PushAsync(new MainPage());

        // Optionally: Clear navigation stack to prevent going back
        Application.Current.MainPage = new NavigationPage(new MainPage());
    }

}
