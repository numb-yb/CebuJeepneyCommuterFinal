using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;

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

        public ICommand SignInCommand => new Command(OnSignIn);

        private async void OnSignIn()
        {
            const string DefaultUsername = "admin";
            const string DefaultPassword = "password123";

            if (Username == DefaultUsername && Password == DefaultPassword)
            {
                // Navigate to the admin home page
                await Application.Current.MainPage.Navigation.PushAsync(new Views.AdminHomePage());
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Login Failed", "Incorrect username or password.", "OK");
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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
