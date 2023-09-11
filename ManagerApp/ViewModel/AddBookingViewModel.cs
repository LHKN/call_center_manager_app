using CommunityToolkit.Mvvm.Input;
using ManagerApp.Model;
using ManagerApp.Repository;
using ManagerApp.Services;
using MapControl;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ManagerApp.ViewModel
{
    class AddBookingViewModel : ViewModelBase
    {
        const string indicator = "AddBooking";

        // fields
        private BookingDetail booking;
        private ObservableCollection<string> transportOptions;

        private IBookingRepository _bookingRepository;

        // constructor
        public AddBookingViewModel(BookingDetail newBooking)
        {
            const int VIP_ROLE = 1;

            //initial
            _bookingRepository = new BookingRepository();
            
            transportOptions = new ObservableCollection<string> {
                    "4 Seater Car","7 Seater Car","Motorbike"
            };

            Bookings = new ObservableCollection<BookingDetail> {newBooking};
            Booking = newBooking;

            Booking.PickupDate = DateOnly.FromDateTime(DateTime.Now);
            Booking.PickupTime = DateTime.Now.TimeOfDay;
            Visibility = false;
            CustomerStatus = "This customer is Regular";

            if (Booking.CustomerRole == VIP_ROLE)
            {
                Visibility = true;
                CustomerStatus = "This customer is VIP";
            }
            else
            {
                Booking.Status = 1;
            }

            BackCommand = new RelayCommand(ExecuteBackCommand);
            ConfirmCommand = new RelayCommand(ExecuteConfirmCommand);
            StartCommand = new RelayCommand(ExecuteStartCommand);
            EndCommand = new RelayCommand(ExecuteEndCommand);
        }

        // execute commands
        public void ExecuteBackCommand()
        {
            ParentPageNavigation.ViewModel = new BookingScheduleViewModel();
        }

        public async void ExecuteConfirmCommand()
        {
            if (Booking.CheckNullDetail() == false)
            {
                await App.MainRoot.ShowDialog("Missing Detail", "Please fill in all the *Required details!");
                return;
            }

            try
            {
                await _bookingRepository.Add(Booking);
                ParentPageNavigation.ViewModel = new BookingScheduleViewModel();
            }
            catch
            {
                return;
            }
        }

        public void ExecuteStartCommand()
        {
            //not implemented
            Booking.PickupLocationName = "119 Đ. Võ Văn Kiệt, Phường 7, Quận 6, Thành phố Hồ Chí Minh, Vietnam";
            Booking.PickupLocationLatitude = 10.740126;
            Booking.PickupLocationLongitude = 106.641168;
            ParentPageNavigation.ViewModel = new MapService(booking, indicator);
        }

        public void ExecuteEndCommand()
        {
            //not implemented
            Booking.DestinationName = "107-54 Trương Định, Phường 6, Quận 3, Thành phố Hồ Chí Minh, Vietnam";
            Booking.DestinationLatitude = 10.778695;
            Booking.DestinationLongitude = 106.688538;
            ParentPageNavigation.ViewModel = new MapService(booking, indicator);
        }

        // getters, setters
        public ObservableCollection<string> TransportOptions { get => transportOptions; set => transportOptions = value; }
        public ObservableCollection<BookingDetail> Bookings { get; set; }
        public BookingDetail Booking { get => booking; set => booking = value; }
        public bool Visibility { get; set; }
        public string CustomerStatus { get; set; }

        // commands
        public ICommand BackCommand { get; }
        public ICommand ConfirmCommand { get; }
        public ICommand StartCommand { get; }
        public ICommand EndCommand { get; }
    }
}
