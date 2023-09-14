using ManagerApp.Model;
using ManagerApp.Model.HTTPResponseTemplate;
using ManagerApp.Repository;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ManagerApp.ViewModel
{
    class LogsViewModel : ViewModelBase
    {
        // fields
        private ObservableCollection<LogNotification> _logs;
        private IBookingRepository _bookingRepository;

        // constructor
        public LogsViewModel()
        {
            _bookingRepository = new BookingRepository();

            var task2 = _bookingRepository.GetLog();
            MyLog = task2.Result;

            Task.Run(() =>
            {
                while (true)
                {
                    var task = _bookingRepository.GetAllLog();
                    _logs = task.Result;
                    Task.Delay(System.TimeSpan.FromMinutes(1));
                }
            });
        }

        // execute commands

        // getters, setters
        public ObservableCollection<LogNotification> LogList { get => _logs; set => _logs = value; }
        public LogNotification MyLog { get; set; }

        // commands
    }
}