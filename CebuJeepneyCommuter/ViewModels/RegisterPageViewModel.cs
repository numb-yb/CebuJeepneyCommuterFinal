using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CebuJeepneyCommuter.Models;
using CebuJeepneyCommuter.Services;

namespace CebuJeepneyCommuter.ViewModels
{
    public class RegisterPageViewModel : INotifyPropertyChanged
    {
        public User NewUser { get; set; } = new User();

        private string confirmPassword;
        public string ConfirmPassword
        {
            get => confirmPassword;
            set { confirmPassword = value; OnPropertyChanged(); }
        }

        private DateTime selectedDate = DateTime.Now;
        public DateTime SelectedDate
        {
            get => selectedDate;
            set { selectedDate = value; OnPropertyChanged(); }
        }

        // Forward User properties to the View
        public string Name
        {
            get => NewUser.Name;
            set { NewUser.Name = value; OnPropertyChanged(); }
        }

        public string Email
        {
            get => NewUser.Email;
            set { NewUser.Email = value; OnPropertyChanged(); }
        }

        public string Number
        {
            get => NewUser.PhoneNumber;
            set { NewUser.PhoneNumber = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get => NewUser.Password;
            set { NewUser.Password = value; OnPropertyChanged(); }
        }

        public ICommand SignUpCommand { get; }

        public RegisterPageViewModel()
        {
            SignUpCommand = new Command(OnSignUp);
        }

        private async void OnSignUp()
        {
            if (Password != ConfirmPassword)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Passwords do not match.", "OK");
                return;
            }

            var newUser = new User
            {
                Name = Name,
                Email = Email,
                PhoneNumber = Number,
                Password = Password,
                BirthDate = SelectedDate
            };

            var userService = new UserService();

            try
            {
                await userService.SaveUserAsync(newUser);
                await userService.SaveLoggedInUserAsync(newUser); // ✅ Save current user to a separate file

                await Application.Current.MainPage.DisplayAlert(
                    "Account Created",
                    $"Welcome, {Name}!",
                    "OK");

                // Optional: Clear form
                Name = Email = Number = Password = ConfirmPassword = string.Empty;
                SelectedDate = DateTime.Now;

                // Optional: Navigate to another page (e.g., ProfilePage)
                // await Application.Current.MainPage.Navigation.PushAsync(new ProfilePage());

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to save user: {ex.Message}", "OK");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
