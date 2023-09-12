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
        }

        public FirestoreDb FirestoreDb { get => _firestoreDb; set => _firestoreDb = value; }

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

        //public async Task<bool> SignUpAccount(Account account)
        //{
        //    try
        //    {
        //        // sign up with email and password
        //        var userCredential = await _authClient.CreateUserWithEmailAndPasswordAsync(account.Username, account.Password, account.Name);
        //        // User successfully signed up.
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle any exceptions that occurred during signup.
        //        await App.MainRoot.ShowDialog("Exception",$"Error signing up: {ex.Message}");
        //        return false;
        //    }
        //}
        //public async Task<bool> SignInAccount(Account account)
        //{
        //    try
        //    {
        //        // sign up with email and password
        //        var userCredential = await _authClient.SignInWithEmailAndPasswordAsync(account.Username, account.Password);
        //        // User successfully signed in.
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle any exceptions that occurred during signup.
        //        Console.WriteLine($"Error signing up: {ex.Message}");
        //        return false;
        //    }
        //}


    }
}
