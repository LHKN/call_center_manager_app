using CommunityToolkit.Mvvm.Input;
using ManagerApp.Model;
using ManagerApp.Repository;
using ManagerApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Type = ManagerApp.Model.Type;

namespace ManagerApp.ViewModel
{
    class AddBookingViewModel : ViewModelBase
    {
        const int STANDARD_ROLE = 0;
        const int VIP_ROLE = 1;
        const int NEW_ROLE = 2;

        const string indicator = "AddBooking";

        // fields
        private List<string> VIPPhoneList;
        private List<string> StandardPhoneList;
        DateTime tempDate;
        TimeSpan tempTime;
        bool isDoneFetching;

        private BookingDetail booking;
        private ObservableCollection<string> transportOptions;

        private IBookingRepository _bookingRepository;
        private IAccountRepository _accountRepository;

        // constructor
        public AddBookingViewModel(BookingDetail newBooking)
        {
            //initial
            _bookingRepository = new BookingRepository();
            _accountRepository = new AccountRepository();
            VIPPhoneList = new List<string>();
            StandardPhoneList = new List<string>();

            transportOptions = new ObservableCollection<string> {
                    "4 Seater Car","7 Seater Car","Motorbike"
            };

            isDoneFetching = false;

            Task.Run(() =>
            {
                var task = _accountRepository.GetAllCustomer();
                List<Customer> customerList = task.Result;

                foreach (var customer in customerList)
                {
                    if (customer.Type.Equals(Type.VIP))
                    {
                        VIPPhoneList.Add(customer.PhoneNumber);
                    }
                    else if (customer.Type.Equals(Type.Standard))
                    {
                        StandardPhoneList.Add(customer.PhoneNumber);
                    }
                }
                isDoneFetching = true;
            });

            Bookings = new ObservableCollection<BookingDetail> { newBooking };
            Booking = newBooking;

            tempDate = DateTime.Now;
            Booking.PickupDate = DateOnly.FromDateTime(tempDate);

            tempTime = DateTime.Now.TimeOfDay;
            Booking.PickupTime = tempTime;

            Visibility = false;
            CustomerStatus = "";

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

            if (!Booking.CheckValidDate(tempDate, tempTime))
            {
                await App.MainRoot.ShowDialog("Error", "Please fill in approriate Date and Time!");
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
            ParentPageNavigation.ViewModel = new MapService(booking, indicator);
        }

        public void ExecuteEndCommand()
        {
            ParentPageNavigation.ViewModel = new MapService(booking, indicator);
        }

        // getters, setters
        public ObservableCollection<string> TransportOptions { get => transportOptions; set => transportOptions = value; }
        public ObservableCollection<BookingDetail> Bookings { get; set; }
        public BookingDetail Booking
        {
            get => booking;
            set
            {
                booking = value;

                if (VIPPhoneList == null) return;
                if (VIPPhoneList.Contains(booking.PhoneNumber))
                {
                    Booking.Status = 0;
                    Booking.CustomerRole = VIP_ROLE;
                    Visibility = true;
                    CustomerStatus = "This customer is VIP";
                }
                else
                {
                    Booking.Status = 1;
                    Visibility = false;
                    if (StandardPhoneList.Contains(booking.PhoneNumber))
                    {
                        CustomerStatus = "This customer is Standard";
                        Booking.CustomerRole = STANDARD_ROLE;
                    }
                    else
                    {
                        CustomerStatus = "This customer is New";
                        Booking.CustomerRole = NEW_ROLE;
                    }
                }

                OnPropertyChanged(nameof(Booking));
            }
        }
        public bool Visibility { get; set; }
        //public bool IsDoneFetching { get => isDoneFetching; set => isDoneFetching = value; }
        public string CustomerStatus { get; set; }

        // commands
        public ICommand BackCommand { get; }
        public ICommand ConfirmCommand { get; }
        public ICommand StartCommand { get; }
        public ICommand EndCommand { get; }
    }
}
