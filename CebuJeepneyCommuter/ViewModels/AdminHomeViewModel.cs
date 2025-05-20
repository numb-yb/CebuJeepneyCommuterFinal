using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CebuJeepneyCommuter.Models;
using CebuJeepneyCommuter.Services;
using System.Net.Mail;
using System.Linq;
using System.Threading.Tasks;

namespace CebuJeepneyCommuter.ViewModels
{
    public class AdminHomeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<string> VehicleTypes { get; set; }
        public ObservableCollection<string> Stops { get; set; }

        private readonly Dictionary<string, List<string>> vehicleStops = new Dictionary<string, List<string>>
        {
            { "Jeepney", new List<string> {
                "Basak", "Colon", "Bulacao", "SM via Colon", "Ayala via Jones Ave", "Labangon",
                "Carbon via Jones", "Urgello", "IT Park via Jones", "Lahug", "Colon via Jones",
                "Guadalupe", "Carbon", "SM via Mango", "Minglanilla", "SM City Cebu",
                "Tabunok", "Ayala via SRP", "Carbon via Jones Ave"
            }},
            { "MyBus", new List<string> {
                "Talisay (SM Seaside)", "SM City", "IT Park", "Minglanilla", "SM City Cebu"
            }},
            { "Beep", new List<string> {
                "Minglanilla", "Ayala via SRP", "Talisay", "IT Park via SM City",
                "Bulacao", "Ayala via Colon", "Lawaan", "SM City", "Ayala", "Colon"
            }},
        };

        private List<RouteInfo> allRoutes;
        private ObservableCollection<RouteInfo> filteredRoutes;
        public ObservableCollection<RouteInfo> FilteredRoutes
        {
            get => filteredRoutes;
            set
            {
                filteredRoutes = value;
                OnPropertyChanged();
            }
        }

        public ICommand UpdateRateCommand { get; }
        public ICommand SaveRouteCommand { get; }
        public ICommand AddUserCommand { get; }
        public ICommand UpdateUserCommand { get; }
        public ICommand DeleteUserCommand { get; }


        public AdminHomeViewModel()
        {
            VehicleTypes = new ObservableCollection<string> { "Jeepney", "Beep", "MyBus" };
            FilteredRoutes = new ObservableCollection<RouteInfo>();
            Stops = new ObservableCollection<string>();

            UpdateRateCommand = new Command(async () => await OnUpdateRateAsync());
            SaveRouteCommand = new Command(async () => await OnSaveRouteAsync());


            _ = LoadRoutesAsync();

            AddUserCommand = new Command(async () => await AddUserAsync());
            UpdateUserCommand = new Command(async () => await UpdateUserAsync());
            DeleteUserCommand = new Command(async () => await DeleteUserAsync());

            _ = LoadUsersAsync();

        }

        private async Task LoadRoutesAsync()
        {
            allRoutes = await RouteDataService.GetAllRoutesAsync();
            FilterRoutesByVehicleType();
        }

        private string selectedVehicleType;
        public string SelectedVehicleType
        {
            get => selectedVehicleType;
            set
            {
                if (selectedVehicleType != value)
                {
                    selectedVehicleType = value;
                    OnPropertyChanged();
                    FilterRoutesByVehicleType();
                }
            }
        }

        private RouteInfo selectedRoute;
        public RouteInfo SelectedRoute
        {
            get => selectedRoute;
            set
            {
                selectedRoute = value;
                OnPropertyChanged();
            }
        }

        private decimal newRate;
        public decimal NewRate
        {
            get => newRate;
            set
            {
                newRate = value;
                OnPropertyChanged();
            }
        }

        private void FilterRoutesByVehicleType()
        {
            if (allRoutes == null) return;

            var filtered = allRoutes
                .Where(r => r.Type.Equals(SelectedVehicleType, System.StringComparison.OrdinalIgnoreCase))
                .ToList();

            FilteredRoutes = new ObservableCollection<RouteInfo>(filtered);

            var stopsForType = filtered
                .SelectMany(r => new[] { r.Origin, r.Destination })
                .Distinct()
                .OrderBy(stop => stop)
                .ToList();

            Stops.Clear();
            foreach (var stop in stopsForType)
            {
                Stops.Add(stop);
            }
        }

        private async Task OnUpdateRateAsync()
        {
            if (SelectedRoute != null)
            {
                SelectedRoute.RegularFare = NewRate;

                await RouteDataService.SaveRoutesAsync(allRoutes);

                await App.Current.MainPage.DisplayAlert("Success", $"Updated rate for {SelectedRoute.Code} to ₱{NewRate}", "OK");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Please select a route", "OK");
            }
        }

        // -------------------------------
        // Create Route Section
        // -------------------------------

        public string NewRouteCode { get; set; }

        private string selectedCreateRouteVehicleType;
        public string SelectedCreateRouteVehicleType
        {
            get => selectedCreateRouteVehicleType;
            set
            {
                if (selectedCreateRouteVehicleType != value)
                {
                    selectedCreateRouteVehicleType = value;
                    OnPropertyChanged();
                    UpdateStopsForSelectedCreateVehicle();
                }
            }
        }

        private void UpdateStopsForSelectedCreateVehicle()
        {
            if (!string.IsNullOrEmpty(SelectedCreateRouteVehicleType) && vehicleStops.ContainsKey(SelectedCreateRouteVehicleType))
            {
                var relevantStops = vehicleStops[SelectedCreateRouteVehicleType]
                    .Distinct()
                    .OrderBy(s => s)
                    .ToList();

                Stops.Clear();
                foreach (var stop in relevantStops)
                    Stops.Add(stop);
            }
            else
            {
                Stops.Clear();
            }
        }

        public decimal NewRouteMinimumFare { get; set; }

        private string selectedStop1;
        public string SelectedStop1
        {
            get => selectedStop1;
            set
            {
                selectedStop1 = value;
                OnPropertyChanged();
            }
        }

        private string selectedStop2;
        public string SelectedStop2
        {
            get => selectedStop2;
            set
            {
                selectedStop2 = value;
                OnPropertyChanged();
            }
        }

        async Task OnSaveRouteAsync()
        {
            // Create new route object
            var newRoute = new RouteInfo
            {
                Code = NewRouteCode.Trim(),
                Type = SelectedCreateRouteVehicleType,
                Origin = SelectedStop1,
                Destination = SelectedStop2,
                RegularFare = NewRouteMinimumFare
            };

            await RouteDataService.SaveOrUpdateRouteAsync(newRoute);

            // Notify SearchRoutesViewModel to refresh
            MessagingCenter.Send(this, "RoutesUpdated");

            // Clear fields
            ClearRouteInputFields();

            await App.Current.MainPage.DisplayAlert("Success", $"Route '{newRoute.Code}' saved/updated successfully.", "OK");
        }

        private void ClearRouteInputFields()
        {
            NewRouteCode = string.Empty;
            SelectedStop1 = null;
            SelectedStop2 = null;
            NewRouteMinimumFare = 0;

            // Notify UI of changes
            OnPropertyChanged(nameof(NewRouteCode));
            OnPropertyChanged(nameof(SelectedStop1));
            OnPropertyChanged(nameof(SelectedStop2));
            OnPropertyChanged(nameof(NewRouteMinimumFare));
        }





        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    
    public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();

        private User selectedUser;
        public User SelectedUser
        {
            get => selectedUser;
            set
            {
                selectedUser = value;
                OnPropertyChanged();
                if (value != null)
                {
                    NewUserName = value.Name;
                    NewUserEmail = value.Email;
                    NewUserPhone = value.PhoneNumber;
                    NewUserPassword = value.Password;
                }
            }
        }
        private string newUserName;
        public string NewUserName
        {
            get => newUserName;
            set { newUserName = value; OnPropertyChanged(); }
        }

        private string newUserEmail;
        public string NewUserEmail
        {
            get => newUserEmail;
            set { newUserEmail = value; OnPropertyChanged(); }
        }

        private string newUserPhone;
        public string NewUserPhone
        {
            get => newUserPhone;
            set { newUserPhone = value; OnPropertyChanged(); }
        }

        private string newUserPassword;
        public string NewUserPassword
        {
            get => newUserPassword;
            set { newUserPassword = value; OnPropertyChanged(); }
        }
        private async Task LoadUsersAsync()
        {
            var userList = await UserDataService.GetAllUsersAsync();
            Users.Clear();
            foreach (var user in userList)
            {
                Users.Add(user);
            }
        }


        private async Task<bool> AddUserAsync()
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(NewUserName) ||
                string.IsNullOrWhiteSpace(NewUserEmail) ||
                string.IsNullOrWhiteSpace(NewUserPhone) ||
                string.IsNullOrWhiteSpace(NewUserPassword))
            {
                await App.Current.MainPage.DisplayAlert("Error", "All fields are required", "OK");
                return false;
            }

            // Email format validation
            try
            {
                var addr = new MailAddress(NewUserEmail);
            }
            catch
            {
                await App.Current.MainPage.DisplayAlert("Error", "Invalid email format", "OK");
                return false;
            }

            var userList = await UserDataService.GetAllUsersAsync();

            // Prevent duplicate email
            if (userList.Any(u => u.Email.Equals(NewUserEmail, StringComparison.OrdinalIgnoreCase)))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Email address already exists", "OK");
                return false;
            }

            var newUser = new User
            {
                Id = userList.Count > 0 ? userList.Max(u => u.Id) + 1 : 1,
                Name = NewUserName,
                Email = NewUserEmail,
                PhoneNumber = NewUserPhone,
                Password = NewUserPassword,
                BirthDate = DateTime.Now // Customize as needed
            };

            userList.Add(newUser);
            await UserDataService.SaveUsersAsync(userList);
            await LoadUsersAsync();

            ClearUserForm();
            await App.Current.MainPage.DisplayAlert("Success", "User added successfully", "OK");
            return true;
        }

        private async Task<bool> UpdateUserAsync()
{
    if (SelectedUser == null)
    {
        await App.Current.MainPage.DisplayAlert("Error", "Please select a user to update", "OK");
        return false;
    }

    // Basic validation
    if (string.IsNullOrWhiteSpace(NewUserName) ||
        string.IsNullOrWhiteSpace(NewUserEmail) ||
        string.IsNullOrWhiteSpace(NewUserPhone))
    {
        await App.Current.MainPage.DisplayAlert("Error", "Name, Email, and Phone are required", "OK");
        return false;
    }

    // Email format validation
    try
    {
        var addr = new MailAddress(NewUserEmail);
    }
    catch
    {
        await App.Current.MainPage.DisplayAlert("Error", "Invalid email format", "OK");
        return false;
    }

    var userList = await UserDataService.GetAllUsersAsync();
    var userToUpdate = userList.FirstOrDefault(u => u.Id == SelectedUser.Id);

    if (userToUpdate != null)
    {
        // Prevent duplicate email on update (except for the same user)
        if (userList.Any(u => u.Id != SelectedUser.Id && u.Email.Equals(NewUserEmail, StringComparison.OrdinalIgnoreCase)))
        {
            await App.Current.MainPage.DisplayAlert("Error", "Another user with the same email already exists", "OK");
            return false;
        }

        // Update user fields
        userToUpdate.Name = NewUserName;
        userToUpdate.Email = NewUserEmail;
        userToUpdate.PhoneNumber = NewUserPhone;

        // ✅ Update password only if admin provided a new one
        if (!string.IsNullOrWhiteSpace(NewUserPassword))
        {
            userToUpdate.Password = NewUserPassword;
        }

        // Save changes to user list
        await UserDataService.SaveUsersAsync(userList);
        await LoadUsersAsync();

        ClearUserForm();
        await App.Current.MainPage.DisplayAlert("Success", "User updated successfully", "OK");
        return true;
    }
    else
    {
        await App.Current.MainPage.DisplayAlert("Error", "User not found", "OK");
        return false;
            }
        }

        public async Task DeleteUserAsync()
        {
            // Validate input fields
            if (string.IsNullOrWhiteSpace(NewUserName) && string.IsNullOrWhiteSpace(NewUserEmail))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter user name or email to delete.", "OK");
                return;
            }

            var userList = await UserDataService.GetAllUsersAsync();

            // Find user matching input name or email (case insensitive)
            var userToDelete = userList.FirstOrDefault(u =>
                (!string.IsNullOrWhiteSpace(NewUserName) && u.Name.Equals(NewUserName, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrWhiteSpace(NewUserEmail) && u.Email.Equals(NewUserEmail, StringComparison.OrdinalIgnoreCase))
            );

            if (userToDelete == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "User not found with the given name or email.", "OK");
                return;
            }

            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Confirm Delete",
                $"Are you sure you want to delete {userToDelete.Name}?",
                "Yes", "No");

            if (!confirm) return;

            await UserDataService.DeleteUserAsync(userToDelete);
            await LoadUsersAsync(); // Refresh the UI with updated list

            await Application.Current.MainPage.DisplayAlert("Success", "User deleted successfully.", "OK");
            ClearUserForm();


            // Clear input fields
            ClearUserForm();
        }




        private void ClearUserForm()
        {
            NewUserName = string.Empty;
            NewUserEmail = string.Empty;
            NewUserPhone = string.Empty;
            NewUserPassword = string.Empty;
            SelectedUser = null;
        }

    }

}
