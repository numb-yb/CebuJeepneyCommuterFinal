using Mapsui.Projections;
using Mapsui.Tiling;
using Mapsui.UI.Maui;

namespace CebuJeepneyCommuter.Views;

public partial class MapUI : ContentPage
{
    private readonly string origin;
    private readonly string destination;

    public MapUI(string origin, string destination)
    {
        InitializeComponent();

        this.origin = origin;
        this.destination = destination;

        var mapControl = new MapControl();

        mapControl.Map.Layers.Add(OpenStreetMap.CreateTileLayer());

        // Coordinates for Cebu, Philippines
        var (x, y) = SphericalMercator.FromLonLat(123.8854, 10.3157);
        var cebuCenter = new Mapsui.MPoint(x, y);

        mapControl.Map.Navigator.CenterOn(cebuCenter);
        mapControl.Map.Navigator.ZoomTo(10);

        Content = mapControl;
    }
}

    