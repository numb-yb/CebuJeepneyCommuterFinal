using CebuJeepneyCommuter.ViewModels;

namespace CebuJeepneyCommuter.Views;

public partial class SearchRoutesPage : ContentPage
{
    public SearchRoutesPage()
    {
        InitializeComponent();
        BindingContext = new SearchRoutesViewModel(); // Set ViewModel here only
    }

    private void OnOriginSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count > 0 && BindingContext is SearchRoutesViewModel vm)
        {
            string selected = e.CurrentSelection[0] as string;
            vm.SelectOriginCommand.Execute(selected);
            ((CollectionView)sender).SelectedItem = null;
        }
    }

    private void OnDestinationSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count > 0 && BindingContext is SearchRoutesViewModel vm)
        {
            string selected = e.CurrentSelection[0] as string;
            vm.SelectDestinationCommand.Execute(selected);
            ((CollectionView)sender).SelectedItem = null;
        }
    }
}
