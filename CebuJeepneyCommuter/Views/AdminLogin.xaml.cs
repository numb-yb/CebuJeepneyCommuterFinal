using CebuJeepneyCommuter.ViewModels;
using Microsoft.Maui.Controls;

namespace CebuJeepneyCommuter.Views
{
    public partial class AdminLoginPage : ContentPage
    {
        public AdminLoginPage()
        {
            InitializeComponent();
            BindingContext = new AdminLoginViewModel(); // Important for MVVM binding
        }
    }
}
