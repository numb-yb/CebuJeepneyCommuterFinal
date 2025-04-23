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

        private void OnSignUp()
        {
            // You can replace this with your actual sign-up logic
            Application.Current.MainPage.DisplayAlert(
                "Account Created",
                $"Welcome, {Name}!\nEmail: {Email}\nDOB: {SelectedDate:MMM dd, yyyy}",
                "OK");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
