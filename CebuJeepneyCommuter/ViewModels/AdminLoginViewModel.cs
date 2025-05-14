using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using CebuJeepneyCommuter.Services;

namespace CebuJeepneyCommuter.ViewModels
{
    public class AdminLoginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _username;
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public ICommand SignInCommand => new Command(async () => await OnSignIn());

        private readonly AdminService _adminService = new();

        private async Task OnSignIn()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Username and password are required.", "OK");
                return;
            }

            var admin = await _adminService.GetAdminByEmailAndPasswordAsync(Username, Password);

            if (admin != null)
            {
                await Application.Current.MainPage.Navigation.PushAsync(new Views.AdminHomePage());
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Login Failed", "Incorrect credentials.", "OK");
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
