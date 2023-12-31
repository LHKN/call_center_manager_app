﻿using CommunityToolkit.Mvvm.Input;
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
        const string PATH_CALC_REQUEST = "Send Path Calculation Request";
        const string PATH_DETAIL_REQUEST = "Send Path Direction Detail Request";

        const string STANDARD_ROLE = "0";
        const string VIP_ROLE = "1";
        const string NEW_ROLE = "2";

        const string indicator = "AddBooking";

        // fields
        private Dictionary<string, string> VIPPhoneList;
        private Dictionary<string, string> StandardPhoneList;
        DateTime tempDate;
        TimeSpan tempTime;

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
            VIPPhoneList = new Dictionary<string, string>();
            StandardPhoneList = new Dictionary<string, string>();

            transportOptions = new ObservableCollection<string> {
                    "4 Seater Car","7 Seater Car","Motorbike"
            };

            IsDoneFetching = true;

            Bookings = new ObservableCollection<BookingDetail> { newBooking };
            Booking = newBooking;

            tempDate = DateTime.Now;
            Booking.PickupDate = DateOnly.FromDateTime(tempDate);

            tempTime = DateTime.Now.TimeOfDay;
            Booking.PickupTime = tempTime;

            Visibility = false;
            CustomerStatus = "";
            Booking.CustomerRole = NEW_ROLE;

            BackCommand = new RelayCommand(ExecuteBackCommand);
            ConfirmCommand = new RelayCommand(ExecuteConfirmCommand);
            StartCommand = new RelayCommand(ExecuteStartCommand);
            EndCommand = new RelayCommand(ExecuteEndCommand);
            RefreshCommand = new RelayCommand(ExecuteRefreshCommand);
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

            ExecuteRefreshCommand();

            var check = await App.MainRoot.ShowYesCancelDialog("Proceed?", "Yes", "Check my input");

            if (check == null) return;

            await _bookingRepository.Add(Booking);
            ParentPageNavigation.ViewModel = new BookingScheduleViewModel();
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
            await Task.Run(() =>
            {
                var task = _accountRepository.GetAllCustomer();
                List<Customer> customerList = task.Result;

                VIPPhoneList.Clear();
                StandardPhoneList.Clear();

                foreach (var customer in customerList)
                {
                    if (customer.Type.Equals(Type.VIP))
                    {
                        VIPPhoneList.Add(customer.PhoneNumber, customer.Name);
                    }
                    else if (customer.Type.Equals(Type.Standard))
                    {
                        StandardPhoneList.Add(customer.PhoneNumber, customer.Name);
                    }
                }
            });

            // HTTP get price and path detail
            ServerHTTPRequest priceRequest = new ServerHTTPRequest(PATH_CALC_REQUEST, booking);
            ServerHTTPRequest pathRequest = new ServerHTTPRequest(PATH_DETAIL_REQUEST, booking);

            IsDoneFetching = true;

            if ((VIPPhoneList == null && StandardPhoneList == null) || booking.PhoneNumber == null) return;
            if (VIPPhoneList.ContainsKey(booking.PhoneNumber))
            {
                Booking.Status = 0;
                Booking.CustomerRole = VIP_ROLE;
                Booking.CustomerName = VIPPhoneList[booking.PhoneNumber];
                Visibility = true;
                CustomerStatus = "This customer is VIP";
            }
            else
            {
                Booking.Status = 1;
                Visibility = false;
                if (StandardPhoneList.ContainsKey(booking.PhoneNumber))
                {
                    CustomerStatus = "This customer is Standard";
                    Booking.CustomerRole = STANDARD_ROLE;
                    Booking.CustomerName = StandardPhoneList[booking.PhoneNumber];
                }
                else
                {
                    CustomerStatus = "This customer is New";
                    Booking.CustomerRole = NEW_ROLE;
                    Booking.CustomerName = null;
                }
            }
        }

        // getters, setters
        public ObservableCollection<string> TransportOptions { get => transportOptions; set => transportOptions = value; }
        public ObservableCollection<BookingDetail> Bookings { get; set; }
        public BookingDetail Booking
        {
            get => booking;
            set => booking = value;
        }
        public bool Visibility { get; set; }
        public bool IsDoneFetching { get; set; }
        public string CustomerStatus { get; set; }

        // commands
        public ICommand BackCommand { get; }
        public ICommand ConfirmCommand { get; }
        public ICommand StartCommand { get; }
        public ICommand EndCommand { get; }
        public ICommand RefreshCommand { get; }
    }
}
