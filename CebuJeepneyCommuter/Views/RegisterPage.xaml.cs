using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;

namespace CebuJeepneyCommuter.Views;

public partial class RegisterPage : ContentPage
{
    
    public DateTime SelectedDate { get; set; } = DateTime.Now;

    public RegisterPage()
    {
        InitializeComponent();
        BindingContext = this;
    }
}
