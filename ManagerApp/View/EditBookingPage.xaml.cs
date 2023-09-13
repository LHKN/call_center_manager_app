// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using ManagerApp.Model;
using ManagerApp.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ManagerApp.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditBookingPage : Page
    {
        private List<string> pickupFormattedAddresses;
        private List<string> destinationFormattedAddresses;
        private List<Services.Point> pickupCoordinatesList;
        private List<Services.Point> destinationCoordinatesList;

        public EditBookingPage()
        {
            this.InitializeComponent();
            datePicker.SelectedDate = DateTimeOffset.Now;
            timePicker.SelectedTime = DateTime.Now.TimeOfDay;

        }

        private async void PickupAutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            string userInput = sender.Text;
            LocationHTTPRequest request = new LocationHTTPRequest();
            // Call a method to fetch suggestions based on the userInput.
            GeocodingResponse response = await request.RetrieveData(userInput);
            pickupFormattedAddresses = new List<string>();
            pickupCoordinatesList = new List<Services.Point>();


            if (response != null)
            {
                // Extract the data
                foreach (var resourceSet in response.resourceSets)
                {
                    foreach (var resource in resourceSet.resources)
                    {
                        // Extract formattedAddress and coordinates
                        pickupFormattedAddresses.Add(resource.address.formattedAddress);
                        pickupCoordinatesList.Add(resource.point);
                    }
                }
            }
            else { pickupFormattedAddresses.Add("No records found"!); }

            sender.ItemsSource = pickupFormattedAddresses;

        }
        private async void DestinationAutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            string userInput = sender.Text;
            LocationHTTPRequest request = new LocationHTTPRequest();
            // Call a method to fetch suggestions based on the userInput.
            GeocodingResponse response = await request.RetrieveData(userInput);
            destinationFormattedAddresses = new List<string>();
            destinationCoordinatesList = new List<Services.Point>();


            if (response != null)
            {
                // Extract the data
                foreach (var resourceSet in response.resourceSets)
                {
                    foreach (var resource in resourceSet.resources)
                    {
                        // Extract formattedAddress and coordinates
                        destinationFormattedAddresses.Add(resource.address.formattedAddress);
                        destinationCoordinatesList.Add(resource.point);
                    }
                }
            }
            else { pickupFormattedAddresses.Add("No records found"!); }

            sender.ItemsSource = destinationFormattedAddresses;
        }

        private void PickupAutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            PickupSuggestBox.Text = args.SelectedItem.ToString();
            var booking = Booking.ItemsSource as BookingDetail;
            booking.PickupLocationName = args.SelectedItem.ToString();
            Services.Point point = pickupCoordinatesList[pickupFormattedAddresses.IndexOf(booking.PickupLocationName)];
            booking.PickupLocationLatitude = point.coordinates[0];
            booking.PickupLocationLongitude = point.coordinates[1];

        }

        private void DestinationAutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            DestinationSuggestBox.Text = args.SelectedItem.ToString();
            var booking = Booking.ItemsSource as BookingDetail;
            booking.DestinationName = args.SelectedItem.ToString();
            Services.Point point = destinationCoordinatesList[destinationFormattedAddresses.IndexOf(booking.DestinationName)];
            booking.DestinationLatitude = point.coordinates[0];
            booking.DestinationLongitude = point.coordinates[1];

        }
    }
}

