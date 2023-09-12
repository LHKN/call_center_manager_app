using CommunityToolkit.Mvvm.Input;
using ManagerApp.Model;
using ManagerApp.Repository;
using ManagerApp.Services;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ManagerApp.ViewModel
{
    class ViewBookingViewModel : ViewModelBase
    {
        const int VIP_ROLE = 1;
        const string indicator = "ViewBooking";

        // fields
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
            Booking = curBooking;
            
            if (curBooking.Status != 0)
            {
                EditVisibility = false;
            }
            else EditVisibility = true;

            if (Booking.CustomerRole == VIP_ROLE)
            {
                CustomerStatus = "This customer is VIP";
                DisplayText = "Optional";
            }
            else
            {
                CustomerStatus = "This customer is Regular";
                DisplayText = "Can not edit";
            }

            if (curBooking.PickupLocationLatitude == 0 && curBooking.PickupLocationLongitude == 0)
            {
                ViewStartVisibility = false;
            }

            if (curBooking.DestinationLatitude == 0 && curBooking.DestinationLongitude == 0)
            {
                ViewEndVisibility = false;
            }

            _bookingRepository = new BookingRepository();

            BackCommand = new RelayCommand(ExecuteBackCommand);
            EditCommand = new RelayCommand(ExecuteEditCommand);
            DeleteCommand = new RelayCommand(ExecuteDeleteCommand);
            StartCommand = new RelayCommand(ExecuteStartCommand);
            EndCommand = new RelayCommand(ExecuteEndCommand);
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
                    await App.MainRoot.ShowDialog("Error", ex.Message);
                    return;
                }
            }

        }


        public async void ExecuteEditCommand()
        {
            ParentPageNavigation.ViewModel = new EditBookingViewModel(Booking);
        }

        public void ExecuteStartCommand()
        {
            ParentPageNavigation.ViewModel = new MapService(booking, indicator);
        }

        public void ExecuteEndCommand()
        {
            ParentPageNavigation.ViewModel = new MapService(booking, indicator);
        }

        // getters, setters
        public ObservableCollection<string> TransportOptions { get => transportOptions; set => transportOptions = value; }
        public BookingDetail Booking { get => booking; set => booking = value; }
        public bool EditVisibility { get; set; }
        public string CustomerStatus { get; set; }
        public string DisplayText { get; set; }
        public bool ViewStartVisibility { get; set; }
        public bool ViewEndVisibility { get; set; }

        // commands
        public ICommand BackCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand StartCommand { get; }
        public ICommand EndCommand { get; }

    }
}
