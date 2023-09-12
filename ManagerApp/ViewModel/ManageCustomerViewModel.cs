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
    class ManageCustomerViewModel : ViewModelBase
    {
        // fields
        private List<Customer> _customerList;
        private List<Customer> _displayCustomerList;
        private List<Customer> _resultCustomerList;
        private ObservableCollection<Customer> _displayCustomerCollection;
        private Customer _selectedCustomer;

        private IAccountRepository _accountRepository;
        private string _paginationMessage;

        private int _currentPage;
        private int _itemsPerPage;
        private int _totalItems;
        private int _totalPages;
        private string _currentKeyword = String.Empty;


        // constructor
        public ManageCustomerViewModel()
        {
            _accountRepository = new AccountRepository();
            CustomerList = new List<Customer>();
            DisplayCustomerList = new List<Customer>();
            DisplayCustomerCollection = new ObservableCollection<Customer>();
            ResultCustomerList= new List<Customer>(); 
            //Paging info
            {
                CurrentPage = 1;
                ItemsPerPage = 5;
            }

            ExecuteGetAllCustomerCommand();
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
            ParentPageNavigation.ViewModel = new AddCustomerViewModel();
        }
        public async void ExecuteGetAllCustomerCommand()
        {
            CustomerList = await _accountRepository.GetAllCustomer(); //TODO: load from Firebase through IAccountRepository
            CustomerList.ForEach(item => ResultCustomerList.Add(item));           
            TotalItems = CustomerList.Count;

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
            PaginationMessage = $"{DisplayCustomerList.Count}/{TotalItems} customers";
        }

        public void UpdateDataSource()
        {
            DisplayCustomerCollection.Clear();

            DisplayCustomerList = ResultCustomerList.Skip((CurrentPage - 1) * ItemsPerPage).Take(ItemsPerPage).ToList();
            DisplayCustomerList.ForEach(x => DisplayCustomerCollection.Add(x));

        }
        //private void ExecuteSearchCommand(string keyword)
        //{
        //    CurrentPage = 1;
        //    //ResultCustomerList = _bookRepository.Filter(CustomerList, StartPrice, EndPrice, CurrentKeyword, GenreId);
        //    UpdateDataSource();
        //    TotalItems = ResultCustomerList.Count;
        //    UpdatePagingInfo();
        //}

        public async void ExecuteViewCommand()
        {
            if (SelectedCustomer == null)
            {
                await App.MainRoot.ShowDialog("No selected item", "Please select an item first!");
                return;
            }
            ParentPageNavigation.ViewModel = new ViewCustomerViewModel(SelectedCustomer);
        }


        // getters, setters
        public List<Customer> CustomerList { get => _customerList; set => _customerList = value; }
        public List<Customer> DisplayCustomerList { get => _displayCustomerList; set => _displayCustomerList = value; }
        public ObservableCollection<Customer> DisplayCustomerCollection { get => _displayCustomerCollection; set => _displayCustomerCollection = value; }
        public List<Customer> ResultCustomerList { get => _resultCustomerList; set => _resultCustomerList = value; }
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
        public Customer SelectedCustomer { get => _selectedCustomer; set => _selectedCustomer = value; }
    }
}
