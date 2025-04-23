using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CebuJeepneyCommuter.ViewModels
{
    public class UserHomeViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> PassengerTypes { get; set; } = new()
        {
            "Regular", "Senior", "Student", "PWD"
        };

        private string selectedPassengerType;
        public string SelectedPassengerType
        {
            get => selectedPassengerType;
            set
            {
                selectedPassengerType = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
