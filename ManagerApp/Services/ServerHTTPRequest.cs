using Google.Rpc;
using ManagerApp.Model;
using ManagerApp.Model.HTTPResponseTemplate;
using ManagerApp.Repository;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

namespace ManagerApp.Services
{
    public class ServerHTTPRequest
    {
        const string PATH_CALC_REQUEST = "Send Path Calculation Request";
        const string PATH_DETAIL_REQUEST = "Send Path Direction Detail Request";
        const string CLIENT_REQUEST = "Send Client Pickup Request";

        private HttpClient client = new HttpClient();

        private BookingDetail booking;
        private IConfigurationRoot _config;
        private string domain;
        private Dictionary<string, string> content;
        private LogNotification log;
        private IBookingRepository _bookingRepository;

        public ServerHTTPRequest(string indicator, BookingDetail currentBooking)
        {
            booking = currentBooking;
            _config = new ConfigurationBuilder().AddUserSecrets<MainWindow>().Build();
            _bookingRepository = new BookingRepository();
            log = new LogNotification();

            domain = _config.GetSection("Server")["domain"];
            if (domain == null) return;

            log.Title = indicator;

            switch (indicator)
            {
                case PATH_CALC_REQUEST:
                    {
                        SendPathCalculationRequest();
                        break;
                    }
                case PATH_DETAIL_REQUEST:
                    {
                        SendPathDetailRequest();
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
            string vehicle;
            
            if (booking.Transport.Equals("Motorbike")) //?
            {
                vehicle = "motor";
            }
            else vehicle = "car";

            string duration = booking.Duration;
            string distance = booking.Distance;
            string cost = booking.Price.ToString();
            string time = booking.PickupDate.ToString() + " " + booking.PickupTime.ToString();

            content = new Dictionary<string, string>
            {
                { "idToken", "" },
                { "systemKey", _config.GetSection("Firebase")["FcmToken"] },
                { "userId", _config.GetSection("Firebase")["AdminId"] },
                { "name", booking.CustomerName },
                { "startAddress", booking.PickupLocationName },
                { "endAddress", booking.DestinationName },
                { "phone", booking.PhoneNumber },
                { "vehicle", vehicle }
            };

            // send request from app to server
            try
            {
                HttpRequestMessage request = new HttpRequestMessage();
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(domain + "pickup/client/new_request");
                request.Headers.Add("ngrok-skip-browser-warning", "any value");
                request.Content = new FormUrlEncodedContent(content);

                HttpResponseMessage response = await client.SendAsync(request);

                log.Message = DateTime.Now.ToString();
                log.StatusCode = response.StatusCode.ToString();
                await _bookingRepository.AddLog(log);

                // Check if the request was successful (HTTP status code 200)
                if (response.IsSuccessStatusCode)
                {
                    // Parse the JSON response
                    string json = await response.Content.ReadAsStringAsync();

                    //get data
                    if (json != "")
                    {
                        //newResponse = JsonConvert.DeserializeObject<PatchCalculationResponse>(json);

                        //if (booking.Transport.Equals("Motorbike"))
                        //{
                        //    booking.Duration = newResponse.PathsCost[1].Duration.ToString();
                        //    booking.Distance = newResponse.PathsCost[1].Distance.ToString();
                        //    booking.Price = (int)Math.Round(newResponse.PathsCost[1].Cost);
                        //}
                        //else
                        //{
                        //    booking.Duration = newResponse.PathsCost[0].Duration.ToString();
                        //    booking.Distance = newResponse.PathsCost[0].Distance.ToString();
                        //    booking.Price = (int)Math.Round(newResponse.PathsCost[0].Cost);
                        //}
                    }
                    else
                        throw new Exception();
                }
                else
                {
                    //await App.MainRoot.ShowDialog("Error", "HTTP Status Code " + (int)response.StatusCode);
                }

            }
            catch (Exception ex)
            {
                //await App.MainRoot.ShowDialog("Error", ex.Message);
            }
        }

        public async void SendPathCalculationRequest()
        {
            content = new Dictionary<string, string>
            {
                { "idToken", "" },
                { "systemKey", _config.GetSection("Firebase")["FcmToken"] },
                { "userId", _config.GetSection("Firebase")["AdminId"] },
                { "startAddress", booking.PickupLocationName },
                { "endAddress", booking.DestinationName }
            };

            // send request from app to server
            try
            {
                HttpRequestMessage request = new HttpRequestMessage();
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(domain + "pickup/cost");
                request.Headers.Add("ngrok-skip-browser-warning", "any value");
                request.Content = new FormUrlEncodedContent(content);

                HttpResponseMessage response = await client.SendAsync(request);

                log.Message = DateTime.Now.ToString();
                log.StatusCode = response.StatusCode.ToString();
                await _bookingRepository.AddLog(log);
                //PatchCalculationResponse newResponse;

                // Check if the request was successful (HTTP status code 200)
                if (response.IsSuccessStatusCode)
                {
                    // Parse the JSON response
                    string json = await response.Content.ReadAsStringAsync();

                    //get data
                    if (json != "")
                    {
                        //newResponse = JsonConvert.DeserializeObject<PatchCalculationResponse>(json);

                        //if (booking.Transport.Equals("Motorbike"))
                        //{
                        //    booking.Duration = newResponse.PathsCost[1].Duration.ToString();
                        //    booking.Distance = newResponse.PathsCost[1].Distance.ToString();
                        //    booking.Price = (int)Math.Round(newResponse.PathsCost[1].Cost);
                        //}
                        //else
                        //{
                        //    booking.Duration = newResponse.PathsCost[0].Duration.ToString();
                        //    booking.Distance = newResponse.PathsCost[0].Distance.ToString();
                        //    booking.Price = (int)Math.Round(newResponse.PathsCost[0].Cost);
                        //}

                        booking.Price = 135000;
                    }
                    else
                        throw new Exception();
                }
                else
                {
                    //await App.MainRoot.ShowDialog("Error", "HTTP Status Code " + (int)response.StatusCode);
                }

            }
            catch (Exception ex)
            {
                //await App.MainRoot.ShowDialog("Error", ex.Message);
            }
        }

        public async void SendPathDetailRequest()
        {
            content = new Dictionary<string, string>
            {
                { "idToken", "" },
                { "systemKey", _config.GetSection("Firebase")["FcmToken"] },
                { "userId", _config.GetSection("Firebase")["AdminId"] },
                { "startAddress", booking.PickupLocationName },
                { "endAddress", booking.DestinationName }
            };

            // send request from app to server
            try
            {
                HttpRequestMessage request = new HttpRequestMessage();
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(domain + "pickup/direction/v1");
                request.Headers.Add("ngrok-skip-browser-warning", "any value");
                request.Content = new FormUrlEncodedContent(content);

                HttpResponseMessage response = await client.SendAsync(request);

                log.Message = DateTime.Now.ToString();
                log.StatusCode = response.StatusCode.ToString();
                await _bookingRepository.AddLog(log);

                // Check if the request was successful (HTTP status code 200)
                if (response.IsSuccessStatusCode)
                {
                    // Parse the JSON response
                    string json = await response.Content.ReadAsStringAsync();

                    //get data
                    if (json != "")
                    {
                        //newResponse = JsonConvert.DeserializeObject<PatchCalculationResponse>(json);

                        //if (booking.Transport.Equals("Motorbike"))
                        //{
                        //    booking.Duration = newResponse.PathsCost[1].Duration.ToString();
                        //    booking.Distance = newResponse.PathsCost[1].Distance.ToString();
                        //    booking.Price = (int)Math.Round(newResponse.PathsCost[1].Cost);
                        //}
                        //else
                        //{
                        //    booking.Duration = newResponse.PathsCost[0].Duration.ToString();
                        //    booking.Distance = newResponse.PathsCost[0].Distance.ToString();
                        //    booking.Price = (int)Math.Round(newResponse.PathsCost[0].Cost);
                        //}

                        booking.Price = 135000;
                    }
                    else
                        throw new Exception();
                }
                else
                {
                    //await App.MainRoot.ShowDialog("Error", "HTTP Status Code " + (int)response.StatusCode);
                }

            }
            catch (Exception ex)
            {
                //await App.MainRoot.ShowDialog("Error", ex.Message);
            }
        }
    }
}
