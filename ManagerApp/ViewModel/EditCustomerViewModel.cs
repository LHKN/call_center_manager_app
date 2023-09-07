using CommunityToolkit.Mvvm.Input;
using ManagerApp.Model;
using ManagerApp.Repository;
using ManagerApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ManagerApp.ViewModel
{
    class EditCustomerViewModel: ViewModelBase
    {
        private IAccountRepository _accountRepository;
        private Customer _currentCustomer;
        public ICommand UpdateCommand { get; private set; }
        public ICommand TerminateCommand { get; private set; }
        public ICommand BackCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public Customer CurrentCustomer { get => _currentCustomer; set => _currentCustomer = value; }

        public EditCustomerViewModel(Customer currentCustomer)
        {
            _accountRepository = new AccountRepository();
            //Get the customer clone instance
            CurrentCustomer = currentCustomer;
            //Loaded
            //PageLoaded();
            UpdateCommand = new RelayCommand(ExecuteUpdateCommand);
            TerminateCommand = new RelayCommand(ExecuteTerminateCommand);
            BackCommand = new RelayCommand(ExecuteBackCommand);
            DeleteCommand = new RelayCommand(ExecuteDeleteCommand);
        }

        public async void ExecuteDeleteCommand()
        {
            var confirmed = await App.MainRoot.ShowYesCancelDialog("Delete this item?", "Delete", "Cancel");

            if (confirmed == true)
            {
                //var task = await _accountRepository.Remove(CurrentCustomer.Id); //TODO: implement Remove/Delete method on AccountRepository
                //if (task)
                //{
                //    await App.MainRoot.ShowDialog("Success", "Customer is removed!");
                //    ExecuteBackCommand();
                //}
                //else
                //{
                //    await App.MainRoot.ShowDialog("Failure", "Removal unsuccessful...");
                //}
            }
        }

        private void ExecuteBackCommand()
        {
            ParentPageNavigation.ViewModel = new ManageCustomerViewModel(); //TODO: implement Back method on AccountRepository
        }

        private async void ExecuteTerminateCommand()
        {
            //TODO: implement Terminate method on AccountRepository
            var confirmed = await App.MainRoot.ShowYesCancelDialog("Terminate this user?", "Ban", "Cancel");

            if (confirmed == true)
            {
                //var task = await _accountRepository.Remove(CurrentCustomer.Id); //TODO: implement Remove/Delete method on AccountRepository
                //if (task)
                //{
                //    await App.MainRoot.ShowDialog("Success", "Customer is banned!");
                //    ExecuteBackCommand();
                //}
                //else
                //{
                //    await App.MainRoot.ShowDialog("Failure", "Removal unsuccessful...");
                //}
            }
        }
        public void ExecuteUpdateCommand()
        {
            ParentPageNavigation.ViewModel = new EditCustomerViewModel(CurrentCustomer); //TODO: implement Update/Edit method on AccountRepository
        }
    }
}
