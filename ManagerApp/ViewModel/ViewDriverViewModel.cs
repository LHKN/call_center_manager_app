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
    class ViewDriverViewModel : ViewModelBase
    {
        private IAccountRepository _accountRepository;
        private Driver _currentDriver;
        private ObservableCollection<string> _DriverSavedLocationCollection;
        public ICommand UpdateCommand { get; private set; }
        public ICommand BanCommand { get; private set; }
        public ICommand BackCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public Driver CurrentDriver { get => _currentDriver; set => _currentDriver = value; }
        public string BanLabel { get => _banLabel; set => _banLabel = value; }
        public string BanTooltip { get => _banTooltip; set => _banTooltip = value; }
        public SymbolIcon BanIcon { get => _banIcon; set => _banIcon = value; }
        public ObservableCollection<string> DriverSavedLocationCollection { get => _DriverSavedLocationCollection; set => _DriverSavedLocationCollection = value; }

        private string _banLabel;
        private string _banTooltip;
        private SymbolIcon _banIcon;

        public ViewDriverViewModel(Driver currentDriver)
        {
            _accountRepository = new AccountRepository();
            //Get the Driver clone instance
            CurrentDriver = currentDriver;
            DriverSavedLocationCollection = new ObservableCollection<string>();
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
        }

        private void UpdateBanButton()
        {
            BanLabel = CurrentDriver.Status != Driver.DriverStatus.Restricted ? "Ban" : "Unban";
            BanTooltip = CurrentDriver.Status != Driver.DriverStatus.Restricted ? "Ban this Driver" : "Unban this Driver";
            BanIcon = CurrentDriver.Status != Driver.DriverStatus.Restricted ? new SymbolIcon(Symbol.Important) : new SymbolIcon(Symbol.Clear);
        }

        public async void ExecuteDeleteCommand()
        {
            var confirmed = await App.MainRoot.ShowYesCancelDialog("Delete this Driver?", "Delete", "Cancel");

            if (confirmed == true)
            {
                var task = await _accountRepository.DeleteDriver(CurrentDriver.Id);
                if (task)
                {
                    await App.MainRoot.ShowDialog("Success", "Driver is removed!");
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
            ParentPageNavigation.ViewModel = new ManageDriverViewModel();
        }

        private async void ExecuteBanCommand()
        {
            //TODO: implement Terminate method on AccountRepository

            if (this.CurrentDriver.Status != Driver.DriverStatus.Restricted)
            {
                var confirmed = await App.MainRoot.ShowYesCancelDialog("Ban this user?", "Ban", "Cancel");
                if (confirmed == true)
                {
                    CurrentDriver.Status = Driver.DriverStatus.Restricted;
                    CurrentDriver.UpdatedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    var task = await _accountRepository.EditDriver(CurrentDriver); //TODO: implement Remove/Delete method on AccountRepository
                    if (task)
                    {
                        await App.MainRoot.ShowDialog("Success", "User status is banned!");
                        ExecuteBackCommand();
                    }
                    else
                    {
                        await App.MainRoot.ShowDialog("Failure", "Ban unsuccessful...");
                    }
                }

            }
            else if (this.CurrentDriver.Status == Driver.DriverStatus.Restricted)
            {
                var confirmed = await App.MainRoot.ShowYesCancelDialog("Unban this user?", "Unban", "Cancel");
                if (confirmed == true)
                {
                    CurrentDriver.Status = Driver.DriverStatus.Offline;
                    CurrentDriver.UpdatedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    var task = await _accountRepository.EditDriver(CurrentDriver);
                    if (task)
                    {
                        await App.MainRoot.ShowDialog("Success", "Unban Driver successfully!");
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
            ParentPageNavigation.ViewModel = new EditDriverViewModel(CurrentDriver);
        }
    }
}
