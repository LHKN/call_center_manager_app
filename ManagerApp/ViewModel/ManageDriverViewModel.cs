using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ManagerApp.ViewModel
{
    class ManageDriverViewModel : ViewModelBase
    {
        // fields

        // constructor
        public ManageDriverViewModel() {

            AddCommand = new RelayCommand(ExecuteAddCommand);

        }

        // execute commands
        public async void ExecuteAddCommand()
        {
            ParentPageNavigation.ViewModel = new AddDriverViewModel();
        }

        // getters, setters

        // commands
        public ICommand AddCommand { get; }

    }
}
