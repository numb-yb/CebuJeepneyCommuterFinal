using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using CebuJeepneyCommuter.Models;
using CebuJeepneyCommuter.Services;

namespace CebuJeepneyCommuter.ViewModels
{
    public class AdminHomeViewModel : INotifyPropertyChanged
    {
        private readonly AdminService _adminService = new();

        public event PropertyChangedEventHandler PropertyChanged;

        // Users list for UI binding
        public ObservableCollection<User> Users { get; set; } = new();

        private User _selectedUser;
        public User SelectedUser
        {
            get => _selectedUser;
            set => SetProperty(ref _selectedUser, value);
        }

        public ICommand LoadUsersCommand => new Command(async () =>
        {
            var users = await _adminService.GetAllUsersAsync();
            Users.Clear();
            foreach (var user in users)
                Users.Add(user);
        });

        public ICommand AddUserCommand => new Command(async () =>
        {
            if (string.IsNullOrWhiteSpace(NewUserName) || string.IsNullOrWhiteSpace(NewUserEmail))
            {
                await Application.Current.MainPage.DisplayAlert("Validation", "Name and Email are required", "OK");
                return;
            }

            var newUser = new User
            {
                Name = NewUserName,
                Email = NewUserEmail,
                PhoneNumber = NewUserPhone,
                Password = NewUserPassword
            };

            await _adminService.AddUserAsync(newUser);
            Users.Add(newUser);
            ClearNewUserFields();
        });

        public ICommand DeleteUserCommand => new Command(async () =>
        {
            if (SelectedUser == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No user selected", "OK");
                return;
            }

            await _adminService.DeleteUserAsync(SelectedUser.Email);
            Users.Remove(SelectedUser);
        });

        public ICommand UpdateUserCommand => new Command(async () =>
        {
            if (SelectedUser == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No user selected", "OK");
                return;
            }

            await _adminService.UpdateUserAsync(SelectedUser);
            await Application.Current.MainPage.DisplayAlert("Success", "User updated", "OK");
        });

        // Fields for adding new user
        public string NewUserName { get => _newUserName; set => SetProperty(ref _newUserName, value); }
        public string NewUserEmail { get => _newUserEmail; set => SetProperty(ref _newUserEmail, value); }
        public string NewUserPhone { get => _newUserPhone; set => SetProperty(ref _newUserPhone, value); }
        public string NewUserPassword { get => _newUserPassword; set => SetProperty(ref _newUserPassword, value); }

        private string _newUserName;
        private string _newUserEmail;
        private string _newUserPhone;
        private string _newUserPassword;

        private void ClearNewUserFields()
        {
            NewUserName = NewUserEmail = NewUserPhone = NewUserPassword = null;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected void SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value)) return;
            backingStore = value;
            OnPropertyChanged(propertyName);
        }
    }
}
