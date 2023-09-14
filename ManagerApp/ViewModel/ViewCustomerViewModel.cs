using CommunityToolkit.Mvvm.Input;
using ManagerApp.Model;
using ManagerApp.Repository;
using ManagerApp.Services;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ManagerApp.ViewModel
{
    class ViewCustomerViewModel : ViewModelBase
    {
        private IAccountRepository _accountRepository;
        private Customer _currentCustomer;
        private ObservableCollection<string> _customerSavedLocationCollection;
        public ICommand UpdateCommand { get; private set; }
        public ICommand BanCommand { get; private set; }
        public ICommand BackCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public Customer CurrentCustomer { get => _currentCustomer; set => _currentCustomer = value; }
        public string BanLabel { get => _banLabel; set => _banLabel = value; }
        public string BanTooltip { get => _banTooltip; set => _banTooltip = value; }
        public SymbolIcon BanIcon { get => _banIcon; set => _banIcon = value; }
        public ObservableCollection<string> CustomerSavedLocationCollection { get => _customerSavedLocationCollection; set => _customerSavedLocationCollection = value; }

        private string _banLabel;
        private string _banTooltip;
        private SymbolIcon _banIcon;

        public ViewCustomerViewModel(Customer currentCustomer)
        {
            _accountRepository = new AccountRepository();
            //Get the customer clone instance
            CurrentCustomer = currentCustomer;
            CustomerSavedLocationCollection = new ObservableCollection<string>();
            //Loaded
            PageLoaded();
            UpdateCommand = new RelayCommand(ExecuteUpdateCommand);
            BanCommand = new RelayCommand(ExecuteBanCommand);
            BackCommand = new RelayCommand(ExecuteBackCommand);
            DeleteCommand = new RelayCommand(ExecuteDeleteCommand);
        }

        private void PageLoaded()
        {
            UpdateBanButton();
            CurrentCustomer.Location.ForEach(x => CustomerSavedLocationCollection.Add(x));

        }

        private void UpdateBanButton()
        {
            BanLabel = CurrentCustomer.Status == AccountStatus.None ? "Ban" : "Unban";
            BanTooltip = CurrentCustomer.Status == AccountStatus.None ? "Ban this customer" : "Unban this customer";
            BanIcon = CurrentCustomer.Status == AccountStatus.None ? new SymbolIcon(Symbol.Important) : new SymbolIcon(Symbol.Clear);
        }

        public async void ExecuteDeleteCommand()
        {
            var confirmed = await App.MainRoot.ShowYesCancelDialog("Delete this customer?", "Delete", "Cancel");

            if (confirmed == true)
            {
                var task = await _accountRepository.DeleteCustomer(CurrentCustomer.Id);
                if (task)
                {
                    await App.MainRoot.ShowDialog("Success", "Customer is removed!");
                    ExecuteBackCommand();
                }
                else
                {
                    await App.MainRoot.ShowDialog("Failure", "Removal unsuccessful...");
                }
            }
            ExecuteBackCommand();
        }

        private void ExecuteBackCommand()
        {
            ParentPageNavigation.ViewModel = new ManageCustomerViewModel();
        }

        private async void ExecuteBanCommand()
        {
            //TODO: implement Terminate method on AccountRepository
            
            if (this.CurrentCustomer.Status == AccountStatus.None)
            {
                var confirmed = await App.MainRoot.ShowYesCancelDialog("Ban this user?", "Ban", "Cancel");
                if (confirmed == true)
                {
                    CurrentCustomer.Status = AccountStatus.Restricted;
                    CurrentCustomer.UpdatedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    var task = await _accountRepository.EditCustomer(CurrentCustomer); //TODO: implement Remove/Delete method on AccountRepository
                    if (task)
                    {
                        await App.MainRoot.ShowDialog("Success", "User is banned!");
                        ExecuteBackCommand();
                    }
                    else
                    {
                        await App.MainRoot.ShowDialog("Failure", "Ban user unsuccessful...");
                    }
                }

            }
            else if (this.CurrentCustomer.Status == AccountStatus.Restricted)
            {
                var confirmed = await App.MainRoot.ShowYesCancelDialog("Unban this user?", "Unban", "Cancel");
                if (confirmed == true)
                {
                    CurrentCustomer.Status = AccountStatus.None;
                    CurrentCustomer.UpdatedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    var task = await _accountRepository.EditCustomer(CurrentCustomer);
                    if (task)
                    {
                        await App.MainRoot.ShowDialog("Success", "Unban customer successfully!");
                        ExecuteBackCommand();
                    }
                    else
                    {
                        await App.MainRoot.ShowDialog("Failure", "Unban unsuccessfully...");
                    }
                }
            }

            UpdateBanButton();

        }
        public void ExecuteUpdateCommand()
        {
            ParentPageNavigation.ViewModel = new EditCustomerViewModel(CurrentCustomer);
        }
    }
}
