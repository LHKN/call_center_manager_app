using CommunityToolkit.Mvvm.Input;
using ManagerApp.Model;
using Microsoft.UI.Xaml.Controls;
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
        private DateTime? _selectedDate;
        private ObservableCollection<BookingDetail> bookings;
        private BookingDetail _selectedBooking;
        private string _warning = "Select before viewing";

        // constructor
        public BookingScheduleViewModel() {
            // initial data
            bookings = new ObservableCollection<BookingDetail> {
                new BookingDetail()
                {
                    PhoneNumber = "12346789",
                    PickupLocationName = "Address A",
                    DestinationName = "Address B",
                    Transport = "4 Seater Car",

                    PickupTime = new TimeSpan(14, 15, 00),
                    PickupDate = new DateOnly(2010, 3, 1),

                    Price = 100000,
                },
                new BookingDetail()
                {
                    PhoneNumber = "12346789",
                    PickupLocationName = "Address A",
                    DestinationName = "Address B",

                    PickupTime = new TimeSpan(14, 15, 00),
                    PickupDate = new DateOnly(2010, 3, 1),

                    Price = 100000,
                },
            };


       

            AddCommand = new RelayCommand(ExecuteAddCommand);
            ViewCommand = new RelayCommand(ExecuteViewCommand);
            EditCommand = new RelayCommand(ExecuteEditCommand);
            DeleteCommand = new RelayCommand(ExecuteDeleteCommand);
        }

        // execute commands
        public async void ExecuteAddCommand()
        {
            ParentPageNavigation.ViewModel = new AddBookingViewModel(bookings[1]);
        }

        public async void ExecuteViewCommand()
        {
            var temp = new ObservableCollection<BookingDetail>
            {
                bookings[1],
            };

                ParentPageNavigation.ViewModel = new ViewBookingViewModel(temp);
        }

        public async void ExecuteEditCommand()
        {

        }

        public async void ExecuteDeleteCommand()
        {

        }

        // getters, setters
        public DateTime? Date { get => _selectedDate; set => _selectedDate = value; }
        public ObservableCollection<BookingDetail> BookingList { get => bookings; set => bookings = value; }
        public BookingDetail SelectedBooking { get => _selectedBooking; set => _selectedBooking = value; }
        public string Warning { get => _warning; set => _warning = value; }

        // commands
        public ICommand AddCommand { get; }
        public ICommand ViewCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
    }
}
