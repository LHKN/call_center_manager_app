using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FireSharp.Interfaces;
using ManagerApp.Model;
using ManagerApp.Services;

namespace ManagerApp.Repository
{
    class BookingRepository : FirebaseConfiguration, IBookingRepository
    {
        public bool UpdateBookingDetail(IFirebaseClient client, BookingDetail booking)
        {
            try
            {
                client.Set("Bookings/" + booking.Id + "/PickupLocationName", booking.PickupLocationName);
                client.Set("Bookings/" + booking.Id + "/DestinationName", booking.DestinationName);
                client.Set("Bookings/" + booking.Id + "/PickupLocationLatitude", booking.PickupLocationLatitude.ToString());
                client.Set("Bookings/" + booking.Id + "/PickupLocationLongitude", booking.PickupLocationLongitude.ToString());
                client.Set("Bookings/" + booking.Id + "/DestinationLatitude", booking.DestinationLatitude.ToString());
                client.Set("Bookings/" + booking.Id + "/DestinationLongitude", booking.DestinationLongitude.ToString());
                client.Set("Bookings/" + booking.Id + "/PickupTime", booking.PickupTime.ToString());
                client.Set("Bookings/" + booking.Id + "/PickupDate", booking.PickupDate.ToString());
                client.Set("Bookings/" + booking.Id + "/Transport", booking.Transport);
                client.Set("Bookings/" + booking.Id + "/Status", booking.Status.ToString());
            }
            catch (Exception ex) when (ex is AggregateException ||
                                      ex is Exception ||
                                      ex is ArgumentNullException)
            {
                App.MainRoot.ShowDialog("Error", ex.Message);  
                return false;
            }

            return true;
        }

        public bool SetBookingDetail(IFirebaseClient client, BookingDetail booking)
        {
            try
            {
                client.Set("Bookings/" + booking.Id + "/PhoneNumber", booking.PhoneNumber);
                client.Set("Bookings/" + booking.Id + "/CustomerRole", booking.CustomerRole.ToString());
                client.Set("Bookings/" + booking.Id + "/CustomerName", booking.CustomerName);
                client.Set("Bookings/" + booking.Id + "/PickupLocationName", booking.PickupLocationName);
                client.Set("Bookings/" + booking.Id + "/DestinationName", booking.DestinationName);
                client.Set("Bookings/" + booking.Id + "/PickupLocationLatitude", booking.PickupLocationLatitude.ToString());
                client.Set("Bookings/" + booking.Id + "/PickupLocationLongitude", booking.PickupLocationLongitude.ToString());
                client.Set("Bookings/" + booking.Id + "/DestinationLatitude", booking.DestinationLatitude.ToString());
                client.Set("Bookings/" + booking.Id + "/DestinationLongitude", booking.DestinationLongitude.ToString());
                client.Set("Bookings/" + booking.Id + "/PickupTime", booking.PickupTime.ToString());
                client.Set("Bookings/" + booking.Id + "/PickupDate", booking.PickupDate.ToString());
                client.Set("Bookings/" + booking.Id + "/Price", booking.Price.ToString());
                //client.Set("Bookings/" + booking.Id + "/Rating", booking.Rating.ToString());
                client.Set("Bookings/" + booking.Id + "/Transport", booking.Transport);
                client.Set("Bookings/" + booking.Id + "/Status", booking.Status.ToString());
            }
            catch (Exception ex) when (ex is AggregateException ||
                                      ex is Exception ||
                                      ex is ArgumentNullException)
            {
                App.MainRoot.ShowDialog("Error", ex.Message);         
                client.Delete("Bookings/" + booking.Id);
                return false;
            }

            return true;
        }

        public bool GetBookingDetail(IFirebaseClient client, ref BookingDetail booking)
        {
            try
            {
                var res = client.Get("Bookings/" + booking.Id);

                BookingDetailTemplate temp = new BookingDetailTemplate();
                temp = res.ResultAs<BookingDetailTemplate>();

                booking.PhoneNumber = temp.PhoneNumber;
                booking.CustomerRole = temp.CustomerRole;
                booking.CustomerName = temp.CustomerName;
                booking.Price = int.Parse(temp.Price);
                booking.Duration = temp.Duration;
                booking.Distance = temp.Distance;
                booking.Status = int.Parse(temp.Status);
                booking.Transport = temp.Transport;
                booking.PickupLocationName = temp.PickupLocationName;
                booking.DestinationName = temp.DestinationName;
                if (temp.PickupLocationLatitude == null)
                {
                    booking.PickupLocationLatitude = 0;
                }
                else booking.PickupLocationLatitude = double.Parse(temp.PickupLocationLatitude);
                if (temp.PickupLocationLongitude == null)
                {
                    booking.PickupLocationLongitude = 0;
                }    
                else booking.PickupLocationLongitude = double.Parse(temp.PickupLocationLongitude);
                if (temp.DestinationLatitude == null)
                {
                    booking.DestinationLatitude = 0;
                }
                else booking.DestinationLatitude = double.Parse(temp.DestinationLatitude);   
                if (temp.DestinationLongitude == null)
                {
                    booking.DestinationLongitude = 0;
                }
                else booking.DestinationLongitude = double.Parse(temp.DestinationLongitude);
                booking.PickupTime = TimeSpan.Parse(temp.PickupTime);
                booking.PickupDate = DateOnly.Parse(temp.PickupDate);
            }
            catch (Exception ex) when (ex is AggregateException ||
                                      ex is Exception ||
                                      ex is ArgumentNullException)
            {
                App.MainRoot.ShowDialog("Error", ex.Message);
                return false;
            }
            return true;
        }

        public async Task<bool> Add(BookingDetail booking)
        {
            IFirebaseClient client;
            bool result = false;
            int curCount;

            try
            {
                client = await TryConnect();

                var res = client.Get("Bookings/count");
                curCount = int.Parse(res.ResultAs<string>());
                if (curCount == 0) return result;

                result = SetBookingDetail(client, booking);

                client.Set("Bookings/idList/" + curCount, booking.Id.ToString());
                curCount += 1;
                client.Set("Bookings/count", curCount.ToString());

                if (result)
                {
                    await App.MainRoot.ShowDialog("Success", "Add Booking Successfully!");
                }
                else
                {
                    await App.MainRoot.ShowDialog("Error", "Add Booking Failed...");
                }
            }
            catch (Exception ex) when (ex is AggregateException ||
                                      ex is Exception ||
                                      ex is ArgumentNullException)
            {
                await App.MainRoot.ShowDialog("Error", ex.Message);
            }

            return result;
        }

        public async Task<bool> Delete(int id)
        {
            IFirebaseClient client;
            bool result = false;

            try
            {
                client = await TryConnect();

                var res = client.Get("Bookings/count");
                int curCount = int.Parse(res.ResultAs<string>());
                if (curCount == 0) return result;

                client.Delete("Bookings/" + id);

                int idFromList = 0;
                List<int> idList = new List<int>();

                for (int i = 0; i < curCount; i++)
                {
                    res = client.Get("Bookings/idList/" + i);
                    int temp = int.Parse(res.ResultAs<string>());
                    idList.Add(temp);

                    if (temp == id)
                    {
                        idFromList = i;
                    }
                }

                curCount--;
                client.Set("Bookings/count", (curCount).ToString());

                if (idFromList != curCount)
                {
                    client.Set("Bookings/idList/" + idFromList, (idList[curCount]).ToString());
                    client.Delete("Bookings/idList/" + curCount);
                }
                else
                {
                    client.Delete("Bookings/idList/" + idFromList);
                }

                await App.MainRoot.ShowDialog("Success", "Delete Booking Successfully!");
            }
            catch (Exception ex) when (ex is AggregateException ||
                                      ex is Exception ||
                                      ex is ArgumentNullException)
            {
                await App.MainRoot.ShowDialog("Error", ex.Message);
            }

            return result;
        }

        public async Task<bool> Edit(BookingDetail booking)
        {
            IFirebaseClient client;
            bool result = false;

            try
            {
                client = await TryConnect();

                result = UpdateBookingDetail(client, booking);
                if (result)
                {
                    await App.MainRoot.ShowDialog("Success", "Update Booking Successfully!");
                }
                else
                {
                    await App.MainRoot.ShowDialog("Error", "Update Booking Failed...");
                }
            }
            catch (Exception ex) when (ex is AggregateException ||
                                      ex is Exception ||
                                      ex is ArgumentNullException)
            {
                await App.MainRoot.ShowDialog("Error", ex.Message);
            }

            return result;
        }

        public async Task<ObservableCollection<BookingDetail>> GetAll()
        {
            ObservableCollection<BookingDetail> bookings = new ObservableCollection<BookingDetail>();
            IFirebaseClient client;

            try
            {
                client = await TryConnect();

                var res = client.Get("Bookings/count");
                int curCount = int.Parse(res.ResultAs<string>());
                if (curCount == 0) return bookings;

                List<int> idList = new List<int>();

                for (int i = 0; i < curCount; i++)
                {
                    res = client.Get("Bookings/idList/" + i);
                    int temp = int.Parse(res.ResultAs<string>());
                    idList.Add(temp);
                }

                foreach (int id in idList)
                {
                    BookingDetail temp = new BookingDetail();
                    temp.Id = id;
                    bool result = GetBookingDetail(client, ref temp);

                    if (!result)
                    {
                        await App.MainRoot.ShowDialog("Error", "Unable to fetch data... Please check database entries");
                        break;
                    }
                    bookings.Add(temp);
                }

            }
            catch(Exception ex) when (ex is AggregateException ||
                                      ex is Exception ||
                                      ex is ArgumentNullException)
            {
                await App.MainRoot.ShowDialog("Error", ex.Message);
            }

            return bookings;
        }

        public async Task<BookingDetail> GetById(int id)
        {
            BookingDetail temp = new BookingDetail();
            IFirebaseClient client;

            try
            {
                client = await TryConnect();

                temp.Id = id;
                bool result = GetBookingDetail(client, ref temp);

                if (!result)
                {
                    await App.MainRoot.ShowDialog("Error", "Unable to fetch data... Please check database entries");
                    return new BookingDetail();
                }
                return temp;
            }
            catch (Exception ex) when (ex is AggregateException ||
                                      ex is Exception ||
                                      ex is ArgumentNullException)
            {
                await App.MainRoot.ShowDialog("Error", ex.Message);
                return new BookingDetail();
            }
        }

        public async Task<int> GetAvailableId()
        {
            IFirebaseClient client;

            try
            {
                client = await TryConnect();
                var res = client.Get("Bookings/count");
                int curCount = int.Parse(res.ResultAs<string>());
                if (curCount == 0) return 0;

                List<int> idList = new List<int>();

                for (int i = 0; i < curCount; i++)
                {
                    res = client.Get("Bookings/idList/" + i);
                    int temp = int.Parse(res.ResultAs<string>());
                    idList.Add(temp);
                }

                return idList.Max() + 1;
            }
            catch (Exception ex) when (ex is AggregateException ||
                                      ex is Exception ||
                                      ex is ArgumentNullException)
            {
                await App.MainRoot.ShowDialog("Error", ex.Message);
                return -1;
            }

        }
    }
}
