using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ManagerApp.ViewModel
{
    class AddBookingViewModel : ViewModelBase
    {
        // fields
        // temp users
        //private List<Account> DisplayCustomerList = new List<Account>();


        // constructor
        public AddBookingViewModel()
        {


            BackCommand = new RelayCommand(ExecuteBackCommand);
            ConfirmCommand = new RelayCommand(ExecuteConfirmCommand);
        }

        // execute commands
        public async void ExecuteBackCommand()
        {
            ParentPageNavigation.ViewModel = new BookingScheduleViewModel();
        }

        public async void ExecuteConfirmCommand()
        {

        }


        // getters, setters

        // commands
        public ICommand BackCommand { get; }
        public ICommand ConfirmCommand { get; }
    }
}
