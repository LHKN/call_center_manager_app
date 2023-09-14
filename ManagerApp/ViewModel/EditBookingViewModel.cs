using CommunityToolkit.Mvvm.Input;
using ManagerApp.Model;
using ManagerApp.Repository;
using ManagerApp.Services;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ManagerApp.ViewModel
{
    class EditBookingViewModel : ViewModelBase
    {
        const string PATH_CALC_REQUEST = "Send Path Calculation Request";
        const string PATH_DETAIL_REQUEST = "Send Path Direction Detail Request";

        const string STANDARD_ROLE = "0";
        const string VIP_ROLE = "1";
        const string NEW_ROLE = "2";

        const string indicator = "EditBooking";

        // fields
        DateTime tempDate;
        TimeSpan tempTime;

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

            tempDate = DateTime.Now;
            tempTime = DateTime.Now.TimeOfDay;

            Bookings = new ObservableCollection<BookingDetail> { oldBooking };
            Booking = oldBooking;
            //old = oldBooking;
            EditVisibility = false;
            IsDoneFetching = true;

            if (Booking.CustomerRole == VIP_ROLE)
            {
                CustomerStatus = "This customer is VIP";
                EditVisibility = true;
            }
            else if(Booking.CustomerRole == NEW_ROLE)
            {
                CustomerStatus = "This customer is New";
            } 
            else if(Booking.CustomerRole == STANDARD_ROLE)
            {
                CustomerStatus = "This customer is Standard";
            }

            BackCommand = new RelayCommand(ExecuteBackCommand);
            ConfirmCommand = new RelayCommand(ExecuteConfirmCommand);
            StartCommand = new RelayCommand(ExecuteStartCommand);
            EndCommand = new RelayCommand(ExecuteEndCommand);
            RefreshCommand = new RelayCommand(ExecuteRefreshCommand);
        }

        // execute commands
        public void ExecuteBackCommand()
        {
            ParentPageNavigation.ViewModel = new ViewBookingViewModel(booking);
        }

        public async void ExecuteConfirmCommand()
        {
            //if (booking.Equals(old)) return;
            if (Booking.CheckNullDetail() == false)
            {
                await App.MainRoot.ShowDialog("Missing Detail", "Please fill in all the *Required details!");
                return;
            }

            if (!Booking.CheckValidDate(tempDate, tempTime))
            {
                await App.MainRoot.ShowDialog("Error", "Please fill in approriate Date and Time!");
                return;
            }

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
            ParentPageNavigation.ViewModel = new MapService(booking, indicator);
        }

        public void ExecuteEndCommand()
        {
            ParentPageNavigation.ViewModel = new MapService(booking, indicator);
        }

        public async void ExecuteRefreshCommand()
        {
            IsDoneFetching = false;

            // HTTP get price and path detail
            //ServerHTTPRequest priceRequest = new ServerHTTPRequest(PATH_CALC_REQUEST, booking);
            //ServerHTTPRequest pathRequest = new ServerHTTPRequest(PATH_DETAIL_REQUEST, booking);

            IsDoneFetching = true;
        }

        // getters, setters
        public ObservableCollection<string> TransportOptions { get => transportOptions; set => transportOptions = value; }
        public ObservableCollection<BookingDetail> Bookings { get; set; }
        public BookingDetail Booking { get => booking; set => booking = value; }
        public string CustomerStatus { get; set; }
        public bool EditVisibility { get; set; }
        public bool IsDoneFetching { get; set; }


        // commands
        public ICommand BackCommand { get; }
        public ICommand ConfirmCommand { get; }
        public ICommand StartCommand { get; }
        public ICommand EndCommand { get; }
        public ICommand RefreshCommand { get; }
    }
}
