using ABI.System;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging.Messages;
using ManagerApp.Model;
using ManagerApp.Model.HTTPResponseTemplate;
using ManagerApp.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.System;

namespace ManagerApp.ViewModel
{
    class LogsViewModel : ViewModelBase
    {
        // fields
        private ObservableCollection<LogNotification> _logs;

        // constructor
        public LogsViewModel(ObservableCollection<LogNotification> log)
        {
            _logs = log;
        }

        // execute commands
        
        // getters, setters
        public ObservableCollection<LogNotification> LogList { get => _logs; set => _logs = value; }

        // commands
    }
}