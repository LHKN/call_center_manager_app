using CommunityToolkit.Mvvm.Input;
using ManagerApp.Model;
using ManagerApp.Repository;
using ManagerApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ManagerApp.ViewModel
{
    class ViewBookingViewModel : ViewModelBase
    {
        // fields
        //private ObservableCollection<BookingDetail> bookings;
        private BookingDetail booking;
        private ObservableCollection<string> transportOptions;

        private IBookingRepository _bookingRepository;

        // constructor
        public ViewBookingViewModel(BookingDetail curBooking)
        {
            //initial
            transportOptions = new ObservableCollection<string> {
                    "4 Seater Car","7 Seater Car","Motorbike"
            };
            //Bookings = curBooking;
            //Booking = curBooking.FirstOrDefault();
            Booking = curBooking;

            _bookingRepository = new BookingRepository();

            BackCommand = new RelayCommand(ExecuteBackCommand);
            EditCommand = new RelayCommand(ExecuteEditCommand);
            DeleteCommand = new RelayCommand(ExecuteDeleteCommand);
        }

        // execute commands
        public async void ExecuteBackCommand()
        {
            ParentPageNavigation.ViewModel = new BookingScheduleViewModel();
        }

        public async void ExecuteDeleteCommand()
        {
            var result = await App.MainRoot.ShowYesCancelDialog("Delete this booking?", "Confirm", "Cancel");
            if ((bool)result)
            {
                try
                {
                     await _bookingRepository.Delete(booking.Id);
                    ExecuteBackCommand();
                }
                catch (Exception ex)
                {
                    return;
                }
            }

        }


        public async void ExecuteEditCommand()
        {
            ParentPageNavigation.ViewModel = new EditBookingViewModel(Booking);
        }


        // getters, setters
        public ObservableCollection<string> TransportOptions { get => transportOptions; set => transportOptions = value; }
        //public ObservableCollection<BookingDetail> Bookings { get; set; }
        public BookingDetail Booking { get => booking; set => booking = value; }


        // commands
        public ICommand BackCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

    }
}
