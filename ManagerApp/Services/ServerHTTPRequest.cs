using ManagerApp.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Json;

namespace ManagerApp.Services
{
    public class ServerHTTPRequest
    {
        const string PATH_REQUEST = "Send Path Calculation Request";
        const string CLIENT_REQUEST = "Send Client Pickup Request";
        
        private BookingDetail booking;
        private IConfigurationRoot _config;
        private string domain;

        public ServerHTTPRequest(string indicator, ref BookingDetail currentBooking) { 
            booking = currentBooking;

            switch(indicator)
            {
                case PATH_REQUEST:
                    {
                        SendPathCalculationRequest();
                        break;
                    }
                case CLIENT_REQUEST:
                    {
                        SendClientPickupRequest();
                        break;
                    }
            }
        }


        public async void SendClientPickupRequest()
        {
            domain = _config.GetSection("Server")["domain"];
            if (domain == null) return;

            BookingDetail booking = new BookingDetail();

            string idToken = "dasd54a6sd56as124324";
            string systemKey = _config.GetSection("Firebase")["FcmToken"];
            string userId = _config.GetSection("Firebase")["AdminId"];
            string name = booking.CustomerName;
            string startAddress = booking.PickupLocationName;
            string endAddress = booking.DestinationName;
            string phone = booking.PhoneNumber;
            string vehicle = booking.Transport; // ?
            string duration = booking.Duration;
            string distance = booking.Distance;
            string cost = booking.Price.ToString();
            string time = booking.PickupDate.ToString() + " " + booking.PickupTime.ToString();

            // send request from app to server
            try
            {
                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage();
                request.Method = HttpMethod.Post;
                client.DefaultRequestHeaders.Add("ngrok-skip-browser-warning", "any value");

                request.Content = JsonContent.Create(new
                {
                    idToken,
                    systemKey,
                    userId,
                    name,
                    startAddress,
                    endAddress,
                    phone,
                    vehicle,
                    duration,
                    distance,
                    cost,
                    time,
                });

                using (HttpClient httpClient = new HttpClient())
                {
                    // Send the GET request to the Bing Maps API
                    HttpResponseMessage response = await httpClient.GetAsync(domain, HttpCompletionOption.ResponseContentRead);

                    // Check if the request was successful (HTTP status code 200)
                    if (response.IsSuccessStatusCode)
                    {
                        // Parse the JSON response
                        string json = await response.Content.ReadAsStringAsync();

                        //get data
                        // json 
                    }
                    else
                    {
                        Console.WriteLine("Error: HTTP Status Code " + (int)response.StatusCode);
                    }
                }
            }
            catch (System.Exception ex)
            {
                await App.MainRoot.ShowDialog("Error", ex.Message);
            }
        }

        public async void SendPathCalculationRequest()
        {
            domain = _config.GetSection("Server")["domain"];
            if (domain == null) return;

            BookingDetail booking = new BookingDetail();

            string idToken = "dasd54a6sd56as1d56a";
            string systemKey = _config.GetSection("Firebase")["FcmToken"];
            string userId = _config.GetSection("Firebase")["AdminId"];
            string startAddress = booking.PickupLocationName;
            string endAddress = booking.DestinationName;

            // send request from app to server
            try
            {
                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage();
                request.Method = HttpMethod.Post;
                client.DefaultRequestHeaders.Add("ngrok-skip-browser-warning", "any value");

                request.Content = JsonContent.Create(new
                {
                    idToken,
                    systemKey,
                    userId,
                    startAddress,
                    endAddress,
                });

                using (HttpClient httpClient = new HttpClient())
                {
                    // Send the GET request to the Bing Maps API
                    HttpResponseMessage response = await httpClient.GetAsync(domain, HttpCompletionOption.ResponseContentRead);

                    // Check if the request was successful (HTTP status code 200)
                    if (response.IsSuccessStatusCode)
                    {
                        // Parse the JSON response
                        string json = await response.Content.ReadAsStringAsync();

                        //get data
                    }
                    else
                    {
                        Console.WriteLine("Error: HTTP Status Code " + (int)response.StatusCode);
                    }
                }
            }
            catch (System.Exception ex)
            {
                await App.MainRoot.ShowDialog("Error", ex.Message);
            }
        }

    }
}
