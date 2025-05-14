namespace CebuJeepneyCommuter.Views;

public partial class UserProfilePage : ContentPage
{
    public UserProfilePage()
    {
        InitializeComponent();
        BindingContext = new CebuJeepneyCommuter.ViewModels.UserProfileViewModel();
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
