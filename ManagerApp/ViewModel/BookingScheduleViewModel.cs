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
            BookingDetail booking = new BookingDetail()
            {
                PhoneNumber = "12346789",
                PickupLocationName = "Address A",
                DestinationName = "Address B",

                PickupTime = new TimeSpan(14, 15, 00),
                PickupDate = new DateTime(2010, 3, 1),

                Price = 100000,
            };

            ParentPageNavigation.ViewModel = new AddBookingViewModel(booking);
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
