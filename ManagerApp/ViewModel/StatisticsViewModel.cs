using ManagerApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.ViewModel
{
    partial class StatisticsViewModel : ViewModelBase, INotifyPropertyChanged
    {
        // fields
        private int _customerCount;
        private int _driverCount;
        private double _tripPercentage;

        private ObservableCollection<string> _cbOptions;
        private string _selectedOption;

        // constructor
        public StatisticsViewModel() { 
        
            //Initial info
            _customerCount = 0;
            _driverCount = 0;
            _tripPercentage = 0;

            _cbOptions = new ObservableCollection<string>
            {
                "Filter by month",
                "Filter by year"             
            };
        }

        // execute commands

        // getters, setters
        public int CustomerCount { get => _customerCount; set => _customerCount = value; }
        public int DriverCount { get => _driverCount; set => _driverCount = value; }
        public double TripPercentage { get => _tripPercentage; set => _tripPercentage = value; }
        public string SelectedOption 
        { 
            get => _selectedOption;
            set
            {
                _selectedOption = value;
                //OnPropertyChanged(nameof(SelectedOption));
            }
        }
        public ObservableCollection<string> CBOptions { get => _cbOptions; set => _cbOptions = value; }
        
        public event PropertyChangedEventHandler PropertyChanged;

        // commands

    }
}
