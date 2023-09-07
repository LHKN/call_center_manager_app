using CommunityToolkit.Mvvm.Input;
using ManagerApp.Model;
using ManagerApp.Repository;
using ManagerApp.Services;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ManagerApp.ViewModel
{
    partial class BookingScheduleViewModel : ViewModelBase
    {
        // fields
        // temp users
        //private List<Account> DisplayCustomerList = new List<Account>();
        private DateOnly? _selectedDate;
        private ObservableCollection<BookingDetail> bookings;
        private ObservableCollection<BookingDetail> sortedBookings;
        private BookingDetail _selectedBooking;
        //private string _warning = "Select before viewing";
        private string _warning = "";

        private IBookingRepository _bookingRepository;

        // constructor
        public BookingScheduleViewModel() {
            // initial data
                //bookings = new ObservableCollection<BookingDetail> {
                //    new BookingDetail()
                //    {
                //        PhoneNumber = "12346789",
                //        PickupLocationName = "Address A",
                //        DestinationName = "Address B",
                //        Transport = "4 Seater Car",

                //        PickupTime = new TimeSpan(14, 15, 00),
                //        PickupDate = new DateOnly(2023, 9, 10),

                //        Price = 100000,
                //    },
                //    new BookingDetail()
                //    {
                //        PhoneNumber = "12346789",
                //        PickupLocationName = "Address A",
                //        DestinationName = "Address B",

                //        PickupTime = new TimeSpan(14, 15, 00),
                //        PickupDate = new DateOnly(2023, 9, 10),

                //        Price = 100000,
                //    },
                //};
                //_selectedBooking = bookings[1];

            _bookingRepository = new BookingRepository();

            var task = _bookingRepository.GetAll();
            bookings = task.Result;

            sortedBookings = new ObservableCollection<BookingDetail>();
            _selectedDate = DateOnly.FromDateTime(DateTimeOffset.Now.Date);

            AddCommand = new RelayCommand(ExecuteAddCommand);
            ViewCommand = new RelayCommand(ExecuteViewCommand);
            EditCommand = new RelayCommand(ExecuteEditCommand);
            DeleteCommand = new RelayCommand(ExecuteDeleteCommand);
        }

        // execute commands
        public async void ExecuteAddCommand()
        {
            BookingDetail newBooking = SelectedBooking; //temp data

            ParentPageNavigation.ViewModel = new AddBookingViewModel(newBooking);
        }

        public async void ExecuteViewCommand()
        {
            if (SelectedBooking == null)
            {
                await App.MainRoot.ShowDialog("No selected booking", "Please select a booking first!");
                return;
            }

            ParentPageNavigation.ViewModel = new ViewBookingViewModel(SelectedBooking);
        }

        public async void ExecuteEditCommand()
        {
            if (SelectedBooking == null)
            {
                await App.MainRoot.ShowDialog("No selected booking", "Please select a booking first!");
                return;
            }

            //edit
        }

        public async void ExecuteDeleteCommand()
        {
            if (SelectedBooking == null)
            {
                await App.MainRoot.ShowDialog("No selected booking", "Please select a booking first!");
                return;
            }

            //delete
        }

        public DateOnly? Date
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;

                if (_selectedDate == null) return;

                sortedBookings.Clear();
                foreach (var booking in bookings)
                {
                    if (booking.PickupDate.ToString().Equals(_selectedDate.ToString()))
                    {
                        sortedBookings.Add(booking);
                    }
                }

                OnPropertyChanged(nameof(Date));
            }
        }

        public ObservableCollection<BookingDetail> BookingList { get => bookings; set => bookings = value; }
        public ObservableCollection<BookingDetail> SortedBookingList { get => sortedBookings; set => sortedBookings = value; }
        public BookingDetail SelectedBooking { get => _selectedBooking; set => _selectedBooking = value; }
        public string Warning { get => _warning; set => _warning = value; }

        // commands
        public ICommand AddCommand { get; }
        public ICommand ViewCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
    }
}
