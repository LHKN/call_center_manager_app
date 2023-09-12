using CommunityToolkit.Mvvm.Input;
using ManagerApp.Model;
using ManagerApp.Repository;
using ManagerApp.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Services.Store;

namespace ManagerApp.ViewModel
{
    class ManageDriverViewModel : ViewModelBase
    {
        // fields
        private List<Driver> _driverList;
        private List<Driver> _displayDriverList;
        private List<Driver> _resultDriverList;
        private ObservableCollection<Driver> _displayDriverCollection;
        private Driver _selectedDriver;

        private IAccountRepository _accountRepository;
        private string _paginationMessage;

        private int _currentPage;
        private int _itemsPerPage;
        private int _totalItems;
        private int _totalPages;
        private string _currentKeyword = String.Empty;


        // constructor
        public ManageDriverViewModel()
        {
            _accountRepository = new AccountRepository();
            DriverList = new List<Driver>();
            DisplayDriverList = new List<Driver>();
            DisplayDriverCollection = new ObservableCollection<Driver>();
            ResultDriverList = new List<Driver>();
            //Paging info
            {
                CurrentPage = 1;
                ItemsPerPage = 5;
            }

            ExecuteGetAllDriverCommand();
            AddCommand = new RelayCommand(ExecuteAddCommand);
            ImportCommand = new RelayCommand(ExecuteImportCommand);
            ExportCommand = new RelayCommand(ExecutExportCommand);
            GoToNextPageCommand = new RelayCommand(ExecuteGoToNextPageCommand);
            GoToPreviousPageCommand = new RelayCommand(ExecuteGoToPreviousPageCommand);
            ViewCommand = new RelayCommand(ExecuteViewCommand);

        }

        private void ExecutExportCommand()
        {

        }

        private void ExecuteImportCommand()
        {

        }

        // execute commands
        public async void ExecuteAddCommand()
        {
            ParentPageNavigation.ViewModel = new AddDriverViewModel();
        }
        public async void ExecuteGetAllDriverCommand()
        {
            DriverList = await _accountRepository.GetAllDriver(); //TODO: load from Firebase through IAccountRepository
            DriverList.ForEach(item => ResultDriverList.Add(item));
            TotalItems = DriverList.Count;

            UpdateDataSource();
            UpdatePagingInfo();
        }

        public void ExecuteGoToNextPageCommand()
        {
            if (!CanExecuteGoToNextPageCommand()) return;
            CurrentPage += 1;
            UpdateDataSource();
            UpdatePagingInfo();
        }

        public void ExecuteGoToPreviousPageCommand()
        {
            if (!CanExecuteGoToPreviousCommand()) return;
            CurrentPage -= 1;
            UpdateDataSource();
            UpdatePagingInfo();
        }

        public bool CanExecuteGoToNextPageCommand() { return CurrentPage < TotalPages; }
        public bool CanExecuteGoToPreviousCommand() { return CurrentPage > 1; }

        public void UpdatePagingInfo()
        {
            TotalPages = TotalItems / ItemsPerPage +
                  (TotalItems % ItemsPerPage == 0 ? 0 : 1);
            PaginationMessage = $"{DisplayDriverList.Count}/{TotalItems} Drivers";
        }

        public void UpdateDataSource()
        {
            DisplayDriverCollection.Clear();

            DisplayDriverList = ResultDriverList.Skip((CurrentPage - 1) * ItemsPerPage).Take(ItemsPerPage).ToList();
            DisplayDriverList.ForEach(x => DisplayDriverCollection.Add(x));

        }
        //private void ExecuteSearchCommand(string keyword)
        //{
        //    CurrentPage = 1;
        //    //ResultDriverList = _bookRepository.Filter(DriverList, StartPrice, EndPrice, CurrentKeyword, GenreId);
        //    UpdateDataSource();
        //    TotalItems = ResultDriverList.Count;
        //    UpdatePagingInfo();
        //}

        public async void ExecuteViewCommand()
        {
            if (SelectedDriver == null)
            {
                await App.MainRoot.ShowDialog("No selected item", "Please select an item first!");
                return;
            }
            ParentPageNavigation.ViewModel = new ViewDriverViewModel(SelectedDriver);
        }


        // getters, setters
        public List<Driver> DriverList { get => _driverList; set => _driverList = value; }
        public List<Driver> DisplayDriverList { get => _displayDriverList; set => _displayDriverList = value; }
        public ObservableCollection<Driver> DisplayDriverCollection { get => _displayDriverCollection; set => _displayDriverCollection = value; }
        public List<Driver> ResultDriverList { get => _resultDriverList; set => _resultDriverList = value; }
        public string PaginationMessage { get => _paginationMessage; set => _paginationMessage = value; }
        public int CurrentPage { get => _currentPage; set => _currentPage = value; }
        public int TotalItems { get => _totalItems; set => _totalItems = value; }
        public string CurrentKeyword { get => _currentKeyword; set => _currentKeyword = value; }
        public int ItemsPerPage { get => _itemsPerPage; set => _itemsPerPage = value; }
        public int TotalPages { get => _totalPages; set => _totalPages = value; }


        // commands
        public ICommand AddCommand { get; private set; }
        public ICommand GoToPreviousPageCommand { get; private set; }
        public ICommand GoToNextPageCommand { get; private set; }
        public ICommand ViewCommand { get; private set; }
        public ICommand ImportCommand { get; private set; }
        public ICommand ExportCommand { get; private set; }
        public Driver SelectedDriver { get => _selectedDriver; set => _selectedDriver = value; }
    }
}
