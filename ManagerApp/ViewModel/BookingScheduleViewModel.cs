using CommunityToolkit.Mvvm.Input;
using LiveChartsCore.SkiaSharpView.Painting;
using ManagerApp.Model;
using ManagerApp.Repository;
using ManagerApp.Services;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.UI;
using Windows.UI;
using Microsoft.UI.Xaml.Controls;
using System.Linq;

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
            //};
            //_selectedBooking = bookings[1];

            bookings = new ObservableCollection<BookingDetail>();

            _bookingRepository = new BookingRepository();
            try
            {
                var task = _bookingRepository.GetAll();
                bookings = task.Result;
            }
            catch (Exception ex)
            {
                return;
            }
        
            //ExecuteRefreshCommand();
       

            sortedBookings = new ObservableCollection<BookingDetail>();
            _selectedDate = DateOnly.FromDateTime(DateTimeOffset.Now.Date);

            AddCommand = new RelayCommand(ExecuteAddCommand);
            ViewCommand = new RelayCommand(ExecuteViewCommand);
            EditCommand = new RelayCommand(ExecuteEditCommand);
            DeleteCommand = new RelayCommand(ExecuteDeleteCommand);
            RefreshCommand = new RelayCommand(ExecuteRefreshCommand);
            //RefreshCommand = new RelayCommand<CalendarView>(ExecuteRefreshCommand);
        }

        // execute commands
        public async void ExecuteAddCommand()
        {
            var task = await _bookingRepository.GetAvailableId();
            if (task == -1) return;

            BookingDetail newBooking = new BookingDetail()
            {
                Id = task,
            };

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

        //private Dictionary<DateOnly?, ObservableCollection<BookingDetail>> bookingsDictionary;

        //public async void ExecuteRefreshCommand(CalendarView obj)
        public async void ExecuteRefreshCommand()
        {
            await Task.Run(() =>
            {
                _bookingRepository = new BookingRepository();
                var task = _bookingRepository.GetAll();
                bookings = task.Result;


                //var element = obj as CalendarView;

                //List<Color> densityColors = new List<Color>();

                //// get all children of calendarview
                //var children = CalendarViewChildrenHelper.Children(element).OfType<CalendarViewDayItem>();

                //// sort bookings into Bookings dict
                //bookingsDictionary = new Dictionary<DateOnly?, ObservableCollection<BookingDetail>>();

                //foreach (BookingDetail booking in bookings)
                //{
                //    if (bookingsDictionary.ContainsKey(booking.PickupDate) == false)
                //    {
                //        bookingsDictionary.Add(booking.PickupDate, new ObservableCollection<BookingDetail>());
                //    }
                //}

                //foreach (BookingDetail booking in bookings)
                //{
                //    bookingsDictionary[booking.PickupDate].Add(booking);
                //    // Set a density bar color for each of the days bookings.
                //    // It's assumed that there can't be more than 10 bookings in a day. Otherwise,
                //    // further processing is needed to fit within the max of 10 density bars.
                //    if (booking.Status == 0)
                //    {
                //        densityColors.Add(Colors.Green);
                //    }
                //    else
                //    {
                //        densityColors.Add(Colors.Blue);
                //    }
                //}
                
                //foreach (var child in children)
                //{
                //    child.SetDensityColors(densityColors);
                //}
            });
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
        public ICommand RefreshCommand { get; }
    }
}
