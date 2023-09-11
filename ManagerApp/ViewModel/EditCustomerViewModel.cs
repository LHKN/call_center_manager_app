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
    class EditCustomerViewModel: ViewModelBase
    {
        private IAccountRepository _accountRepository;
        private ObservableCollection<Gender> genders;
        private ObservableCollection<Model.Type> types;

        private Customer _currentCustomer;
        public ICommand SaveCommand { get; private set; }
        public ICommand BanCommand { get; private set; }
        public ICommand BackCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public Customer CurrentCustomer { get => _currentCustomer; set => _currentCustomer = value; }
        public ObservableCollection<Gender> Genders { get => genders; set => genders = value; }
        public ObservableCollection<Model.Type> Types { get => types; set => types = value; }

        public EditCustomerViewModel(Customer currentCustomer)
        {
            _accountRepository = new AccountRepository();
            //Get the customer clone instance
            CurrentCustomer = currentCustomer;

            Genders = new ObservableCollection<Gender>();
            {
                Genders.Add(Gender.Male);
                Genders.Add(Gender.Female);
                Genders.Add(Gender.Unspecified);
            }

            Types = new ObservableCollection<Model.Type>();
            {
                Types.Add(Model.Type.Standard);
                Types.Add(Model.Type.VIP);
                Types.Add(Model.Type.NotUsingApp);
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
                ParentPageNavigation.ViewModel = new ViewCustomerViewModel(CurrentCustomer);
            }
        }

        private void ExecuteBackCommand()
        {
            ParentPageNavigation.ViewModel = new ViewCustomerViewModel(CurrentCustomer);
        }

        public async void ExecuteSaveCommand()
        {
            try
            {
                bool isSuccess = await _accountRepository.EditCustomer(CurrentCustomer);
                if (isSuccess == true)
                {
                    await App.MainRoot.ShowDialog("Success", "Customer updated successfully!");
                }
                else
                {
                    await App.MainRoot.ShowDialog("Failed", "Failed to update this customer!");
                }
            }
            catch (Exception ex) { await App.MainRoot.ShowDialog("Exception", ex.Message); }

        }
    }
}
