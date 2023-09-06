using System;
using ManagerApp.Services;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp;
using Microsoft.Extensions.Configuration;
using ManagerApp.Model;
using System.Threading.Tasks;

namespace ManagerApp.Repository
{
    class FirebaseConfiguration
    {
        private IFirebaseClient client;
        private IFirebaseConfig ifc;
        private IConfigurationRoot _config;

        public FirebaseConfiguration()
        {
            _config = new ConfigurationBuilder().AddUserSecrets<MainWindow>().Build();

            ifc = new FirebaseConfig()
            {
                AuthSecret = _config.GetSection("Firebase")["AuthSecret"],
                BasePath = "https://transportation-app-297c0-default-rtdb.firebaseio.com/"
            };
        }

        public async Task<IFirebaseClient> TryConnect()
        {
            try
            {
                return new FirebaseClient(ifc);
            }
            catch
            {
                await App.MainRoot.ShowDialog("Can not connect to Firebase", "There was a problem with your internet!");
                return null;
            }
        }
    }
}
