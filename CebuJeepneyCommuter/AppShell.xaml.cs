using CebuJeepneyCommuter.Views;

namespace CebuJeepneyCommuter;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(RegisterPage), typeof(Views.RegisterPage));

        
    }
}
