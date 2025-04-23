namespace CebuJeepneyCommuter.Views;

public partial class SearchRoutesPage : ContentPage
{
	public SearchRoutesPage()
	{
		InitializeComponent();
	}
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

}