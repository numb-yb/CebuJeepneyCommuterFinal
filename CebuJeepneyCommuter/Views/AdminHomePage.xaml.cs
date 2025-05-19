using System;
using CebuJeepneyCommuter.ViewModels;

namespace CebuJeepneyCommuter.Views;

public partial class AdminHomePage : ContentPage
{
    public AdminHomePage()
    {
        InitializeComponent();
        BindingContext = new AdminHomeViewModel();
    }

    private void OnCreateNewRouteClicked(object sender, EventArgs e)
    {
        MinimumRateView.IsVisible = false;
        CreateRouteView.IsVisible = true;
        ManageUsersView.IsVisible = false;
    }

    private void OnChangeMinimumRateClicked(object sender, EventArgs e)
    {
        CreateRouteView.IsVisible = false;
        MinimumRateView.IsVisible = true;
        ManageUsersView.IsVisible = false;
    }

    private void OnManageUsersClicked(object sender, EventArgs e)
    {
        CreateRouteView.IsVisible = false;
        MinimumRateView.IsVisible = false;
        ManageUsersView.IsVisible = true;

        //// Load users from ViewModel
        //if (BindingContext is AdminHomeViewModel vm)
        //{
        //    if (vm.LoadUsersCommand.CanExecute(null))
        //        vm.LoadUsersCommand.Execute(null);
        //}
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        // Navigate to login page (replace with your actual login page name)
        await Navigation.PushAsync(new MainPage());

        // Optionally: Clear navigation stack to prevent going back
        Application.Current.MainPage = new NavigationPage(new MainPage());
    }

}
