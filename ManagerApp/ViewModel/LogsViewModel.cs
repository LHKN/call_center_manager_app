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
        private LogNotification _log;
        private IBookingRepository _bookingRepository;

        // constructor
        public LogsViewModel()
        {
            _bookingRepository = new BookingRepository();

            var task2 = _bookingRepository.GetLog();
            _log = task2.Result;

            var task = _bookingRepository.GetAllLog();
            _logs = task.Result;
            //RunTask();
        }

        // execute commands
        public async void RunTask()
        {
            await Task.Run(() =>
            {
                var task2 = _bookingRepository.GetLog();
                _log = task2.Result;

                var task = _bookingRepository.GetAllLog();
                _logs = task.Result;
            });
        }

        // getters, setters
        public ObservableCollection<LogNotification> LogList { get => _logs; set => _logs = value; }
        public LogNotification MyLog { get => _log; set => _log = value; }

        // commands
    }
}