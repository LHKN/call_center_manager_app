﻿using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ManagerApp.ViewModel
{
    partial class ManageCustomerViewModel : ViewModelBase
    {
        // fields

        // constructor
        public ManageCustomerViewModel()
        {
            AddCommand = new RelayCommand(ExecuteAddCommand);
        }

        // execute commands
        public async void ExecuteAddCommand()
        {
            ParentPageNavigation.ViewModel = new AddCustomerViewModel();
        }

        // getters, setters

        // commands
        public ICommand AddCommand { get; }

    }
}
