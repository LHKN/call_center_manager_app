using CommunityToolkit.Mvvm.Input;
using ManagerApp.Model;
using ManagerApp.Repository;
using ManagerApp.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ManagerApp.ViewModel
{
    class LoginViewModel: ViewModelBase
    {

        private string _email;
        private string _password;
        private string _errorMessage;
        private bool _isValidData;
        private bool _isRememberAccount;

        IAccountRepository _accountRepository;

        //-> Constructor
        public LoginViewModel()
        {
            _accountRepository = new AccountRepository();
            LoadedCommand = new RelayCommand(PageLoaded);
            LoginCommand = new RelayCommand(ExecuteLoginCommand);
            RememberAccountCommand = new RelayCommand<bool>(ExecuteRememberAccountCommand);
        }

        private async void ExecuteLoginCommand()
        {
            bool isSuccess = await _accountRepository.AccountSignedIn(
                new System.Net.NetworkCredential(Email, Password));

            if (isSuccess)
            {
                Thread.CurrentPrincipal = new GenericPrincipal(
                    new GenericIdentity(Email), null);

                await App.MainRoot.ShowDialog("Notification", "Login successfully!");

                ParentPageNavigation.ViewModel = new HomeViewModel();
            }
            else
            {
                await App.MainRoot.ShowDialog("Error", "Login failed!");
                return;
            }

            if (IsRememberAccount)
            {
                //save to config for local login
                var sysconfig = System.Configuration.ConfigurationManager.OpenExeConfiguration(
                    ConfigurationUserLevel.None);
                sysconfig.AppSettings.Settings["Username"].Value = Email;

                // Encrypt password
                var passwordInBytes = Encoding.UTF8.GetBytes(Password);
                var entropy = new byte[20];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(entropy);
                }

                var cypherText = ProtectedData.Protect(
                    passwordInBytes,
                    entropy,
                    DataProtectionScope.CurrentUser
                );

                var passwordIn64 = Convert.ToBase64String(cypherText);
                var entropyIn64 = Convert.ToBase64String(entropy);

                sysconfig.AppSettings.Settings["Password"].Value = passwordIn64;
                sysconfig.AppSettings.Settings["Entropy"].Value = entropyIn64;

                sysconfig.Save(ConfigurationSaveMode.Full);
                System.Configuration.ConfigurationManager.RefreshSection("appSettings");
            }
        }

        private void PageLoaded()
        {
            //get from local
            string username = System.Configuration.ConfigurationManager.AppSettings["Username"]!;
            string passwordIn64 = System.Configuration.ConfigurationManager.AppSettings["Password"];
            string entropyIn64 = System.Configuration.ConfigurationManager.AppSettings["Entropy"]!;

            if (passwordIn64.Length != 0)
            {
                byte[] entropyInBytes = Convert.FromBase64String(entropyIn64);
                byte[] cypherTextInBytes = Convert.FromBase64String(passwordIn64);

                byte[] passwordInBytes = ProtectedData.Unprotect(cypherTextInBytes,
                    entropyInBytes,
                    DataProtectionScope.CurrentUser
                );

                string password = Encoding.UTF8.GetString(passwordInBytes);

                Email = username;
                Password = password;
            }
        }

        public void ExecuteRememberAccountCommand(bool isChecked)
        {
            IsRememberAccount = isChecked;
        }

        //-> getter, setter
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                SetProperty(ref _errorMessage, value);
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public bool IsValidData
        {
            get => _isValidData;
            set
            {
                SetProperty(ref _isValidData, value);
                OnPropertyChanged(nameof(IsValidData));
            }
        }

        //-> Commands
        public RelayCommand LoginCommand { get; }
        public RelayCommand LoadedCommand { get; }
        public RelayCommand ResetCommand { get; }
        public RelayCommand<bool> RememberAccountCommand { get; }
        public bool IsRememberAccount { get => _isRememberAccount; set => _isRememberAccount = value; }
        public string Email { get => _email; set => _email = value; }
        public string Password { get => _password; set => _password = value; }
    }
}
