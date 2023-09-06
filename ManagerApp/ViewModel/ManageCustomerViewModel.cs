using CommunityToolkit.Mvvm.Input;
using ManagerApp.Model;
using ManagerApp.Repository;
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
            CustomerList = new List<Customer> {
                new VIPCustomer {
                    Id = "CUS000001",
                    Name = "Trần Văn Tèo",
                    PhoneNumber = "+84834635633",
                    Location = "89 Hoàng Hoa Thám, Phường 6, Bình Thạnh, Thành phố Hồ Chí Minh, Việt Nam",

                },
                new Customer
                {
                    Id = "CUS000002",
                    Name = "Thái Văn Thiên",
                    PhoneNumber = "+84192355906",
                    Location = "71/1/12 Nguyễn Văn Thương, Phường 25, Bình Thạnh, Thành phố Hồ Chí Minh, Việt Nam",

                },
                new VIPCustomer
                {
                    Id = "CUS000003",
                    Name = "Lê Hoàng Khanh Nguyên",
                    PhoneNumber = "+84331197205",
                    Location = "106 Phạm Viết Chánh, Phường Nguyễn Cư Trinh, Quận 1, Thành phố Hồ Chí Minh, Việt Nam",

                },
                new VIPCustomer
                {
                    Id = "CUS000004",
                    Name = "Lê Hoàng Khanh Nguyên",
                    PhoneNumber = "+84331197205",
                    Location = "106 Phạm Viết Chánh, Phường Nguyễn Cư Trinh, Quận 1, Thành phố Hồ Chí Minh, Việt Nam",

                },
                new VIPCustomer
                {
                    Id = "CUS000005",
                    Name = "Lê Hoàng Khanh Nguyên",
                    PhoneNumber = "+84331197205",
                    Location = "106 Phạm Viết Chánh, Phường Nguyễn Cư Trinh, Quận 1, Thành phố Hồ Chí Minh, Việt Nam",

                },
                new VIPCustomer
                {
                    Id = "CUS000006",
                    Name = "Lê Hoàng Khanh Nguyên",
                    PhoneNumber = "+84331197205",
                    Location = "106 Phạm Viết Chánh, Phường Nguyễn Cư Trinh, Quận 1, Thành phố Hồ Chí Minh, Việt Nam",

                },
                new VIPCustomer
                {
                    Id = "CUS000007",
                    Name = "Lê Hoàng Khanh Nguyên",
                    PhoneNumber = "+84331197205",
                    Location = "106 Phạm Viết Chánh, Phường Nguyễn Cư Trinh, Quận 1, Thành phố Hồ Chí Minh, Việt Nam",

                },
                new VIPCustomer
                {
                    Id = "CUS000008",
                    Name = "Lê Hoàng Khanh Nguyên",
                    PhoneNumber = "+84331197205",
                    Location = "106 Phạm Viết Chánh, Phường Nguyễn Cư Trinh, Quận 1, Thành phố Hồ Chí Minh, Việt Nam",

                }

            };
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
            GoToNextPageCommand = new RelayCommand(ExecuteGoToNextPageCommand);
            GoToPreviousPageCommand = new RelayCommand(ExecuteGoToPreviousPageCommand);
        }

        // execute commands
        public async void ExecuteAddCommand()
        {
            ParentPageNavigation.ViewModel = new AddCustomerViewModel();
        }
        public async void ExecuteGetAllCustomerCommand()
        {
            //await Task.Run(() => { });
            //CustomerList = await _accountRepository.GetAllCustomer(); //TODO: load from Firebase through IAccountRepository
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
        public RelayCommand GoToPreviousPageCommand { get; private set; }
        public RelayCommand GoToNextPageCommand { get; private set; }
        public Customer SelectedCustomer { get => _selectedCustomer; set => _selectedCustomer = value; }
    }
}
