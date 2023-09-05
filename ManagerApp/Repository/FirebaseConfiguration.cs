using System;
using ManagerApp.Services;
using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;
using FireSharp;
using Microsoft.Extensions.Configuration;
using ManagerApp.Model;

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

        public void TryConnect()
        {
            try
            {
                client = new FirebaseClient(ifc);
            }
            catch
            {
                App.MainRoot.ShowDialog("Can not connect to Firebase", "There was a problem with your internet!");
            }
        }

        public void ClientSetBooking(String str, BookingDetail obj)
        {
            var setter = client.Set(str, obj);
        }
    }
}
