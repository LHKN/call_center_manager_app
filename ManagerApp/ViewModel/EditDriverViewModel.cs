using CommunityToolkit.Mvvm.Input;
using ManagerApp.Model;
using ManagerApp.Repository;
using ManagerApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ManagerApp.ViewModel
{
    class EditDriverViewModel : ViewModelBase
    {
        private IAccountRepository _accountRepository;
        private ObservableCollection<Gender> genders;

        private Driver _currentDriver;
        public ICommand SaveCommand { get; private set; }
        public ICommand BanCommand { get; private set; }
        public ICommand BackCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public Driver CurrentDriver { get => _currentDriver; set => _currentDriver = value; }
        public ObservableCollection<Gender> Genders { get => genders; set => genders = value; }

        public EditDriverViewModel(Driver currentDriver)
        {
            _accountRepository = new AccountRepository();
            //Get the Driver clone instance
            CurrentDriver = currentDriver;

            Genders = new ObservableCollection<Gender>();
            {
                Genders.Add(Gender.Male);
                Genders.Add(Gender.Female);
                Genders.Add(Gender.Unspecified);
            }
            //Loaded
            //PageLoaded();
            SaveCommand = new RelayCommand(ExecuteSaveCommand);
            BackCommand = new RelayCommand(ExecuteBackCommand);
            CancelCommand = new RelayCommand(ExecuteCancelCommand);
        }

        public async void ExecuteCancelCommand()
        {
            var confirmed = await App.MainRoot.ShowYesCancelDialog("Cancel editing?", "OK", "Cancel");

            if (confirmed == true)
            {
                ParentPageNavigation.ViewModel = new ViewDriverViewModel(CurrentDriver);
            }
        }

        private void ExecuteBackCommand()
        {
            ParentPageNavigation.ViewModel = new ViewDriverViewModel(CurrentDriver);
        }

        public async void ExecuteSaveCommand()
        {
            try
            {
                bool isSuccess = await _accountRepository.EditDriver(CurrentDriver);
                if (isSuccess == true)
                {
                    await App.MainRoot.ShowDialog("Success", "Driver updated successfully!");
                }
                else
                {
                    await App.MainRoot.ShowDialog("Failed", "Failed to update this Driver!");
                }
            }
            catch (Exception ex) { await App.MainRoot.ShowDialog("Exception", ex.Message); }

        }
    }
}
