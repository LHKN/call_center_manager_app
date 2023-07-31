using CommunityToolkit.Mvvm.Input;
using ManagerApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ManagerApp.ViewModel
{
    partial class BookingScheduleViewModel : ViewModelBase
    {
        // fields
            // temp users
        //private List<Account> DisplayCustomerList = new List<Account>();


        // constructor
        public BookingScheduleViewModel() {


            AddCommand = new RelayCommand(ExecuteAddCommand);
            ViewCommand = new RelayCommand(ExecuteViewCommand);
            EditCommand = new RelayCommand(ExecuteEditCommand);
            DeleteCommand = new RelayCommand(ExecuteDeleteCommand);
        }

        // execute commands
        public async void ExecuteAddCommand()
        {
            ParentPageNavigation.ViewModel = new AddBookingViewModel();
        }

        public async void ExecuteViewCommand()
        {

        }

        public async void ExecuteEditCommand()
        {

        }

        public async void ExecuteDeleteCommand()
        {

        }

        // getters, setters

        // commands
        public ICommand AddCommand { get; }
        public ICommand ViewCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
    }
}
