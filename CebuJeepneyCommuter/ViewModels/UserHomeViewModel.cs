using CebuJeepneyCommuter.Models;
using CebuJeepneyCommuter.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CebuJeepneyCommuter.ViewModels
{
    public class UserHomeViewModel : INotifyPropertyChanged
    {
        private string userName;
        public string UserName
        {
            get => userName;
            set { userName = value; OnPropertyChanged(); }
        }

        private string userEmail;
        public string UserEmail
        {
            get => userEmail;
            set { userEmail = value; OnPropertyChanged(); }
        }

        public UserHomeViewModel()
        {
            LoadLoggedInUser();
        }

        private async void LoadLoggedInUser()
        {
            var userService = new UserService();
            var user = await userService.GetLoggedInUserAsync();
            if (user != null)
            {
                UserName = user.Name;
                UserEmail = user.Email;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
