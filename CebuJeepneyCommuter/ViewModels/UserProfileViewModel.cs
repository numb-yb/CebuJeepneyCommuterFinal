using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace CebuJeepneyCommuter.ViewModels
{
    public class UserProfileViewModel : INotifyPropertyChanged
    {
        private string userName;
        private string email;
        private string phoneNumber;
        private string password;

        public string UserName
        {
            get => userName;
            set => SetProperty(ref userName, value);
        }

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        public string PhoneNumber
        {
            get => phoneNumber;
            set => SetProperty(ref phoneNumber, value);
        }

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        public ICommand LogoutCommand { get; }

        public UserProfileViewModel()
        {
            // Dummy data
            UserName = "Juan Dela Cruz";
            Email = "juan@gmail.com";
            PhoneNumber = "09123456789";
            Password = "password";

            LogoutCommand = new Command(OnLogout);
        }

        private async void OnLogout()
        {
            await Application.Current.MainPage.DisplayAlert("Logout", "You have logged out.", "OK");

            // Reset navigation and go to MainPage
            Application.Current.MainPage = new NavigationPage(new CebuJeepneyCommuter.Views.MainPage());
        }

        public event PropertyChangedEventHandler PropertyChanged;

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
