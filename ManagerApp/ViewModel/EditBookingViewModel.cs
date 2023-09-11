using CommunityToolkit.Mvvm.Input;
using ManagerApp.Model;
using ManagerApp.Repository;
using ManagerApp.Services;
using MapControl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ManagerApp.ViewModel
{
    class EditBookingViewModel : ViewModelBase
    {
        const int VIP_ROLE = 1;
        const string indicator = "EditBooking";

        // fields
        private BookingDetail booking;
        //private BookingDetail old;
        private ObservableCollection<string> transportOptions;

        private IBookingRepository _bookingRepository;

        // constructor
        public EditBookingViewModel(BookingDetail oldBooking)
        {
            //initial
            _bookingRepository = new BookingRepository();

            transportOptions = new ObservableCollection<string> {
                    "4 Seater Car","7 Seater Car","Motorbike"
            };

            Bookings = new ObservableCollection<BookingDetail> { oldBooking };
            Booking = oldBooking;
            //old = oldBooking;
            EditVisibility = false;

            if (Booking.CustomerRole == VIP_ROLE)
            {
                CustomerStatus = "This customer is VIP";
                EditVisibility = true;
            }
            else CustomerStatus = "This customer is Regular";

            BackCommand = new RelayCommand(ExecuteBackCommand);
            ConfirmCommand = new RelayCommand(ExecuteConfirmCommand);
            StartCommand = new RelayCommand(ExecuteStartCommand);
            EndCommand = new RelayCommand(ExecuteEndCommand);
        }

        // execute commands
        public void ExecuteBackCommand()
        {
            ParentPageNavigation.ViewModel = new ViewBookingViewModel(booking);
        }

        public async void ExecuteConfirmCommand()
        {
            //if (booking.Equals(old)) return;
            try
            {
                await _bookingRepository.Edit(Booking);
            }
            catch
            {
                return;
            }
            ParentPageNavigation.ViewModel = new ViewBookingViewModel(booking);
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
        public string CustomerStatus { get; set; }
        public bool EditVisibility { get; set; }


        // commands
        public ICommand BackCommand { get; }
        public ICommand ConfirmCommand { get; }
        public ICommand StartCommand { get; }
        public ICommand EndCommand { get; }
    }
}
