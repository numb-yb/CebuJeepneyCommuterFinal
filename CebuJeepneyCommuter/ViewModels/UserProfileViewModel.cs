using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CebuJeepneyCommuter.Models;
using Microsoft.Maui.Controls;

public class UserProfileViewModel : INotifyPropertyChanged
{
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
        set
        {
            if (CurrentUser != null && CurrentUser.Name != value)
            {
                CurrentUser.Name = value;
                OnPropertyChanged();
            }
        }
    }

    public string Email
    {
        get => CurrentUser?.Email;
        set
        {
            if (CurrentUser != null && CurrentUser.Email != value)
            {
                CurrentUser.Email = value;
                OnPropertyChanged();
            }
        }
    }

    public string PhoneNumber
    {
        get => CurrentUser?.PhoneNumber;
        set
        {
            if (CurrentUser != null && CurrentUser.PhoneNumber != value)
            {
                CurrentUser.PhoneNumber = value;
                OnPropertyChanged();
            }
        }
    }

    public string Password
    {
        get => CurrentUser?.Password;
        set
        {
            if (CurrentUser != null && CurrentUser.Password != value)
            {
                CurrentUser.Password = value;
                OnPropertyChanged();
            }
        }
    }

    public ICommand LogoutCommand { get; }

    public UserProfileViewModel()
    {
        // Initialize with dummy user
        CurrentUser = new User
        {
            Name = "Juan Dela Cruz",
            Email = "juan@gmail.com",
            PhoneNumber = "09123456789",
            Password = "password"
        };

        LogoutCommand = new Command(OnLogout);
    }

    private async void OnLogout()
    {
        await Application.Current.MainPage.DisplayAlert("Logout", "You have logged out.", "OK");
        Application.Current.MainPage = new NavigationPage(new CebuJeepneyCommuter.Views.MainPage());
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
}
