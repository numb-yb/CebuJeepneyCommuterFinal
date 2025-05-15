namespace CebuJeepneyCommuter.Views;

public partial class MapUI : ContentPage
{
	public MapUI()
	{
		InitializeComponent();

        var mapControl = new Mapsui.UI.Maui.MapControl();
        mapControl.Map?.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer());
        Content = mapControl;
    }
}