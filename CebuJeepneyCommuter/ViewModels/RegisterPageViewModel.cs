using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CebuJeepneyCommuter.Models;
using CebuJeepneyCommuter.Services;


namespace CebuJeepneyCommuter.ViewModels
{
    public class RegisterPageViewModel : INotifyPropertyChanged
    {
        private string name;
        public string Name
        {
            get => name;
            set { name = value; OnPropertyChanged(); }
        }

        private string email;
        public string Email
        {
            get => email;
            set { email = value; OnPropertyChanged(); }
        }

        private string number;
        public string Number
        {
            get => number;
            set { number = value; OnPropertyChanged(); }
        }

        private string password;
        public string Password
        {
            get => password;
            set { password = value; OnPropertyChanged(); }
        }

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
                Number = Number,
                Password = Password,
                BirthDate = SelectedDate
            };

            var userService = new UserService();
            await userService.SaveUserAsync(newUser);

            await Application.Current.MainPage.DisplayAlert(
                "Account Created",
                $"Welcome, {Name}!",
                "OK");

            // Optional: Clear form or navigate to login page
            Name = Email = Number = Password = ConfirmPassword = string.Empty;
            SelectedDate = DateTime.Now;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
