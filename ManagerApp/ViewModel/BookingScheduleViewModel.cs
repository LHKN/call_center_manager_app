using CommunityToolkit.Mvvm.Input;
using ManagerApp.Model;
using ManagerApp.Repository;
using ManagerApp.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ManagerApp.ViewModel
{
    partial class BookingScheduleViewModel : ViewModelBase
    {
        // fields
        private DateOnly? _selectedDate;
        private ObservableCollection<BookingDetail> bookings;
        private ObservableCollection<BookingDetail> sortedBookings;
        private BookingDetail _selectedBooking;
        private string _warning = "Updating";

        private IBookingRepository _bookingRepository;

        // constructor
        public BookingScheduleViewModel() {
            // initial data
            bookings = new ObservableCollection<BookingDetail>();

            _bookingRepository = new BookingRepository();
        
            ExecuteRefreshCommand();

            sortedBookings = new ObservableCollection<BookingDetail>();
            _selectedDate = DateOnly.FromDateTime(DateTimeOffset.Now.Date);

            AddCommand = new RelayCommand(ExecuteAddCommand);
            ViewCommand = new RelayCommand(ExecuteViewCommand);
            RefreshCommand = new RelayCommand(ExecuteRefreshCommand);
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

        public async void ExecuteRefreshCommand()
        {
            Warning = "Updating";
            await Task.Run(() =>
            {
                _bookingRepository = new BookingRepository();
                var task = _bookingRepository.GetAll();
                bookings = task.Result;
            });
            OnPropertyChanged(nameof(BookingList));
            Warning = "Finished";
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
        public ICommand RefreshCommand { get; }
    }
}
