using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ManagerApp.ViewModel
{
    class AddDriverViewModel : ViewModelBase
    {
        // fields

        // constructor
        public AddDriverViewModel()
        {
            BackCommand = new RelayCommand(ExecuteBackCommand);
        }

        // execute commands
        public async void ExecuteBackCommand()
        {
            ParentPageNavigation.ViewModel = new ManageDriverViewModel();
        }

        // getters, setters

        // commands
        public ICommand BackCommand { get; }

    }
}
