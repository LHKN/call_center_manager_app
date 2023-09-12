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
    class AddDriverViewModel : ViewModelBase
    {

        private IAccountRepository _accountRepository;
        private ObservableCollection<Gender> genders;
        private ObservableCollection<Model.Type> types;

        private Driver _currentDriver;
        public ICommand SaveCommand { get; private set; }
        public ICommand BanCommand { get; private set; }
        public ICommand BackCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public Driver CurrentDriver { get => _currentDriver; set => _currentDriver = value; }
        public ObservableCollection<Gender> Genders { get => genders; set => genders = value; }
        public ObservableCollection<Model.Type> Types { get => types; set => types = value; }



        public AddDriverViewModel()
        {
            _accountRepository = new AccountRepository();

            //Loaded
            PageLoaded();
            SaveCommand = new RelayCommand(ExecuteSaveCommand);
            BackCommand = new RelayCommand(ExecuteBackCommand);
            CancelCommand = new RelayCommand(ExecuteCancelCommand);
        }

        private void PageLoaded()
        {
            //New instance for Driver
            CurrentDriver = new Driver
            {
                CreatedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                Role = Role.Driver,

            };

            Genders = new ObservableCollection<Gender>();
            {
                Genders.Add(Gender.Male);
                Genders.Add(Gender.Female);
                Genders.Add(Gender.Unspecified);
            }

        }

        public async void ExecuteCancelCommand()
        {
            var confirmed = await App.MainRoot.ShowYesCancelDialog("Cancel adding this Driver?", "OK", "Cancel");

            if (confirmed == true)
            {
                ParentPageNavigation.ViewModel = new ManageDriverViewModel();
            }

        }

        private void ExecuteBackCommand()
        {
            ParentPageNavigation.ViewModel = new ManageDriverViewModel(); //TODO: implement Back method on AccountRepository
        }

        public async void ExecuteSaveCommand()
        {
            try
            {
                bool isSuccess = await _accountRepository.AddDriver(CurrentDriver);
                if (isSuccess == true)
                {
                    await App.MainRoot.ShowDialog("Success", "Driver is added successfully!");
                }
                else
                {
                    await App.MainRoot.ShowDialog("Failed", "Failed to add this Driver!");
                }
            }
            catch (Exception ex) { await App.MainRoot.ShowDialog("Exception", ex.Message); }

            ParentPageNavigation.ViewModel = new ManageDriverViewModel(); //TODO: implement Update/Edit method on AccountRepository

        }
    }
}
