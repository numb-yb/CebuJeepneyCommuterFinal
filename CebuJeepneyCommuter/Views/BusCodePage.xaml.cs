namespace CebuJeepneyCommuter.Views;

public partial class BusCodePage : ContentPage
{
	public BusCodePage()
	{
		InitializeComponent();
	}
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}