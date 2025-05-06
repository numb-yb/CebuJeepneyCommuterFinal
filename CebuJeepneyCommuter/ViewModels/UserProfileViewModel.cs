using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CebuJeepneyCommuter.Models;
using CebuJeepneyCommuter.Services;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;

namespace CebuJeepneyCommuter.ViewModels
{
    public class UserProfileViewModel : INotifyPropertyChanged
    {
        private readonly AdminService adminService = new();

        private User currentUser;
        public User CurrentUser
        {
            get => currentUser;
            set
            {
                currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
                OnPropertyChanged(nameof(UserName));
                OnPropertyChanged(nameof(Email));
                OnPropertyChanged(nameof(PhoneNumber));
                OnPropertyChanged(nameof(Password));
            }
        }

        public string UserName
        {
            get => CurrentUser?.Name;
            set { if (CurrentUser != null && CurrentUser.Name != value) { CurrentUser.Name = value; OnPropertyChanged(); } }
        }

        public string Email
        {
            get => CurrentUser?.Email;
            set { if (CurrentUser != null && CurrentUser.Email != value) { CurrentUser.Email = value; OnPropertyChanged(); } }
        }

        public string PhoneNumber
        {
            get => CurrentUser?.PhoneNumber;
            set { if (CurrentUser != null && CurrentUser.PhoneNumber != value) { CurrentUser.PhoneNumber = value; OnPropertyChanged(); } }
        }

        public string Password
        {
            get => CurrentUser?.Password;
            set { if (CurrentUser != null && CurrentUser.Password != value) { CurrentUser.Password = value; OnPropertyChanged(); } }
        }

        public ICommand LogoutCommand { get; }
        public ICommand SaveCommand { get; }

        public UserProfileViewModel()
        {
            LogoutCommand = new Command(OnLogout);
            SaveCommand = new Command(async () => await SaveUserAsync());
            _ = LoadUserAsync(); // fire-and-forget
        }

        private async Task LoadUserAsync()
        {
            // Simulate load by email (use real login later)
            string email = "juan@gmail.com";
            CurrentUser = await adminService.GetUserByEmailAsync(email);
        }

        private async Task SaveUserAsync()
        {
            if (CurrentUser != null)
            {
                await adminService.UpdateUserAsync(CurrentUser);
                await Application.Current.MainPage.DisplayAlert("Success", "Profile updated.", "OK");
            }
        }

        private async void OnLogout()
        {
            await Application.Current.MainPage.DisplayAlert("Logout", "You have logged out.", "OK");
            Application.Current.MainPage = new NavigationPage(new Views.MainPage());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
