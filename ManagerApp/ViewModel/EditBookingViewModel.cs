using CommunityToolkit.Mvvm.Input;
using ManagerApp.Model;
using ManagerApp.Repository;
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
            //Booking.PickupDate = DateOnly.FromDateTime(DateTime.Now);

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
            Booking.PickupLocationName = "Address A";
        }

        public void ExecuteEndCommand()
        {
            //not implemented
            Booking.DestinationName = "Address B";
        }


        // getters, setters
        public ObservableCollection<string> TransportOptions { get => transportOptions; set => transportOptions = value; }
        public ObservableCollection<BookingDetail> Bookings { get; set; }
        public BookingDetail Booking { get => booking; set => booking = value; }


        // commands
        public ICommand BackCommand { get; }
        public ICommand ConfirmCommand { get; }
        public ICommand StartCommand { get; }
        public ICommand EndCommand { get; }
    }
}
