﻿using CommunityToolkit.Mvvm.Input;
using ManagerApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ManagerApp.ViewModel
{
    class AddBookingViewModel : ViewModelBase
    {
        // fields
        private BookingDetail booking;
        private ObservableCollection<string> transportOptions;

        // constructor
        public AddBookingViewModel(BookingDetail newBooking)
        {
            //initial
            transportOptions = new ObservableCollection<string> {
                    "4 Seater Car","7 Seater Car","Motorbike"
            };
            Bookings = new ObservableCollection<BookingDetail> { newBooking};
            Booking = newBooking;

            BackCommand = new RelayCommand(ExecuteBackCommand);
            ConfirmCommand = new RelayCommand(ExecuteConfirmCommand);
        }

        // execute commands
        public async void ExecuteBackCommand()
        {
            ParentPageNavigation.ViewModel = new BookingScheduleViewModel();
        }

        public async void ExecuteConfirmCommand()
        {

        }


        // getters, setters
        public ObservableCollection<string> TransportOptions { get => transportOptions; set => transportOptions = value; }
        public ObservableCollection<BookingDetail> Bookings { get; set; }
        public BookingDetail Booking { get => booking; set => booking = value; }


        // commands
        public ICommand BackCommand { get; }
        public ICommand ConfirmCommand { get; }
    }
}