using System;
using System.Threading.Tasks;
using System.Net.Http;
using MapControl;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace ManagerApp.Services
{
    public class LocationHTTPRequest
    {
        public LocationHTTPRequest() { }

        public async Task<GeocodingResponse> RetrieveData(string address)
        {
            IConfigurationRoot _config = new ConfigurationBuilder().AddUserSecrets<MainWindow>().Build();

            // Replace 'YOUR_BING_MAPS_API_KEY' with your actual Bing Maps API key
            string apiKey = _config.GetSection("BingMap")["APIKey"];
            //// Address you want to geocode
            //string address = "1600 Amphitheatre Parkway, Mountain View, CA";

            // Bing Maps Geocoding API endpoint
            string url = $"https://dev.virtualearth.net/REST/v1/Locations?q={address}&key={apiKey}";


            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    // Send the GET request to the Bing Maps API
                    HttpResponseMessage response = await httpClient.GetAsync(url);

                    // Check if the request was successful (HTTP status code 200)
                    if (response.IsSuccessStatusCode)
                    {
                        // Parse the JSON response
                        string json = await response.Content.ReadAsStringAsync();                       

                        // Deserialize the JSON
                        GeocodingResponse geocodingResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<GeocodingResponse>(json);

                        // Check the response for results
                        if (geocodingResponse != null && geocodingResponse.resourceSets.Length > 0)
                        {
                            Resource[] resources = geocodingResponse.resourceSets[0].resources;
                            if (resources.Length > 0)
                            {
                                // Print the first result
                                Console.WriteLine("Location found:");
                                Console.WriteLine("Formatted Address: " + resources[0].address.formattedAddress);
                                Console.WriteLine("Coordinates: " + resources[0].point.coordinates[0] + ", " + resources[0].point.coordinates[1]);
                                
                            }
                            else
                            {
                                //await App.MainRoot.ShowDialog("Search", "No results found.");
                            }
                        }
                        else
                        {
                            //await App.MainRoot.ShowDialog("Search", "No results found.");
                        }
                        return geocodingResponse;
                    }
                    else
                    {
                        //await App.MainRoot.ShowDialog("Search", "Error: HTTP Status Code " + response.StatusCode.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                //await App.MainRoot.ShowDialog("Search", "An error occurred: " + ex.Message);
            }
            return null;
        }
    }

    // Classes for deserialization
    public class GeocodingResponse
    {
        public ResourceSet[] resourceSets { get; set; }
    }

    public class ResourceSet
    {
        public Resource[] resources { get; set; }
    }

    public class Resource
    {
        public Address address { get; set; }
        public Point point { get; set; }
    }

    public class Address
    {
        public string formattedAddress { get; set; }
    }

    public class Point
    {
        public double[] coordinates { get; set; }
    }
}
