namespace CebuJeepneyCommuter.Views;

public partial class MapUI : ContentPage
{
    private string Origin { get; }
    private string Destination { get; }

    public MapUI(string origin, string destination)
    {
        InitializeComponent();

        Origin = origin;
        Destination = destination;

        var mapControl = new Mapsui.UI.Maui.MapControl();

        mapControl.Map?.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer());

        // TODO: Add logic to fetch/display route from origin to destination

        Content = mapControl;
    }
}
