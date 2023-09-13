using System;
using ManagerApp.Services;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
using Microsoft.Extensions.Configuration;
using ManagerApp.Model;
using System.Threading.Tasks;
//using Microsoft.WindowsAppSDK.Runtime.Packages;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using FirebaseAdmin.Auth;
using System.Net;

namespace ManagerApp.Repository
{
    class FirebaseConfiguration
    {
        private IFirebaseConfig ifc;
        private IConfigurationRoot _config;

        private IFirebaseAuthClient _authClient;
        private FirestoreDb _firestoreDb;

        public FirebaseConfiguration()
        {
            //FireSharp FirebaseConfig
            {
                _config = new ConfigurationBuilder().AddUserSecrets<MainWindow>().Build();

                ifc = new FirebaseConfig()
                {
                    AuthSecret = _config.GetSection("Firebase")["AuthSecret"],
                    BasePath = "https://transportation-app-297c0-default-rtdb.firebaseio.com/"
                };
            }
            //Firestore config
            {
                string path = _config.GetSection("Firebase:ServiceAccount")["key_file"];
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
                string project = _config.GetSection("Firebase:ServiceAccount")["project_name"];
                FirestoreDb = FirestoreDb.Create(project);
            }

            // Configure Firebase Authentication
            {
                var config = new FirebaseAuthConfig
                {
                    ApiKey = _config.GetSection("Firebase")["APIKey"],
                    AuthDomain = _config.GetSection("Firebase")["AuthDomain"],
                    Providers = new FirebaseAuthProvider[]
                    {
                    // Add and configure individual providers
                    new GoogleProvider().AddScopes("email"),
                    new EmailProvider()
                        // ...
                    },
                    UserRepository = new FileUserRepository("FirebaseSample")
                };

                AuthClient = new FirebaseAuthClient(config);
            }


        }

        protected FirestoreDb FirestoreDb { get => _firestoreDb; set => _firestoreDb = value; }
        protected IFirebaseAuthClient AuthClient { get => _authClient; set => _authClient = value; }

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
