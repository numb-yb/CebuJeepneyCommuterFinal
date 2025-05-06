using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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
            // Basic sample check
            if (Password != ConfirmPassword)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Passwords do not match", "OK");
                return;
            }

            await Application.Current.MainPage.DisplayAlert(
                "Account Created",
                $"Welcome, {NewUser.Name}!\nEmail: {NewUser.Email}\nDOB: {SelectedDate:MMM dd, yyyy}",
                "OK");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
