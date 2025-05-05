using System;
using System.Collections.Generic;
using CebuJeepneyCommuter.ViewModels;
using Microsoft.Maui.Controls;

namespace CebuJeepneyCommuter.Views;

public partial class RegisterPage : ContentPage
{
    
    public DateTime SelectedDate { get; set; } = DateTime.Now;

    public RegisterPage()
    {
        InitializeComponent();
        BindingContext = new RegisterPageViewModel();
    }
}
