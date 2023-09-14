using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FireSharp.Interfaces;
using FireSharp.Response;
using ManagerApp.Model;
using ManagerApp.Model.HTTPResponseTemplate;
using ManagerApp.Services;
using Windows.Media.Protection.PlayReady;

namespace ManagerApp.Repository
{
    class BookingRepository : FirebaseConfiguration, IBookingRepository
    {
        const string CLIENT_REQUEST = "Send Client Pickup Request";

        public bool UpdateBookingDetail(IFirebaseClient client, BookingDetail booking)
        {
            try
            {
                BookingDetailTemplate temp = new BookingDetailTemplate();

                temp.PhoneNumber = booking.PhoneNumber;
                temp.CustomerRole = booking.CustomerRole;
                temp.CustomerName = booking.CustomerName;
                temp.Price = booking.Price.ToString();
                temp.Duration = booking.Duration;
                temp.Distance = booking.Distance;
                temp.Status = booking.Status.ToString();
                temp.Transport = booking.Transport;
                temp.PickupLocationName = booking.PickupLocationName;
                temp.DestinationName = booking.DestinationName;
                temp.PickupLocationLatitude = booking.PickupLocationLatitude.ToString();
                temp.PickupLocationLongitude = booking.PickupLocationLongitude.ToString();
                temp.DestinationLatitude = booking.DestinationLatitude.ToString();
                temp.DestinationLongitude = booking.DestinationLongitude.ToString();
                temp.PickupTime = booking.PickupTime.ToString();
                temp.PickupDate = booking.PickupDate.ToString();

                client.Update("Bookings/" + booking.Id, temp);
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
                BookingDetailTemplate temp = new BookingDetailTemplate();

                temp.Id = booking.Id.ToString();
                temp.PhoneNumber = booking.PhoneNumber;
                temp.CustomerRole = booking.CustomerRole;
                temp.CustomerName = booking.CustomerName;
                temp.Price = booking.Price.ToString();
                temp.Duration = booking.Duration;
                temp.Distance = booking.Distance;
                temp.Status = booking.Status.ToString();
                temp.Transport = booking.Transport;
                temp.PickupLocationName = booking.PickupLocationName;
                temp.DestinationName = booking.DestinationName;
                temp.PickupLocationLatitude = booking.PickupLocationLatitude.ToString();
                temp.PickupLocationLongitude = booking.PickupLocationLongitude.ToString();
                temp.DestinationLatitude = booking.DestinationLatitude.ToString();
                temp.DestinationLongitude = booking.DestinationLongitude.ToString();
                temp.PickupTime = booking.PickupTime.ToString();
                temp.PickupDate = booking.PickupDate.ToString();

                client.Set("Bookings/" + booking.Id, temp);
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

                    DateTime getDate = ((DateOnly)temp.PickupDate).ToDateTime(TimeOnly.MinValue);
                    TimeSpan getTime = (TimeSpan)temp.PickupTime;

                    if (getDate.CompareTo(DateTime.Now.Date) == 0)
                    {
                        if (getTime.TotalHours.Equals(DateTime.Now.Hour))
                        {
                            new ServerHTTPRequest(CLIENT_REQUEST, temp);
                        }
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

        public async Task AddLog(LogNotification log)
        {
            IFirebaseClient client;

            try
            {
                client = await TryConnect();

                var countRes = client.Get("Logs/count");
                int curCount = int.Parse(countRes.ResultAs<string>());

                client.Set("Logs/" + curCount, log);
                client.Set("Logs/count", (curCount + 1).ToString());
            }
            catch (Exception ex) when (ex is AggregateException ||
                                      ex is Exception ||
                                      ex is ArgumentNullException)
            {
                await App.MainRoot.ShowDialog("Error", ex.Message);
            }

        }

        public async Task<ObservableCollection<LogNotification>> GetAllLog()
        {
            ObservableCollection<LogNotification> logs = new ObservableCollection<LogNotification>();
            IFirebaseClient client;

            try
            {
                client = await TryConnect();

                var countRes = client.Get("Logs/count");
                int curCount = int.Parse(countRes.ResultAs<string>());
                if (curCount == 0) return logs;

                FirebaseResponse res;
                LogNotification temp;

                for (int i = 0; i < curCount; i++)
                {
                    res = client.Get("Logs/" + i);
                    temp = res.ResultAs<LogNotification>();
                    logs.Add(temp);
                }                

            }
            catch (Exception ex) when (ex is AggregateException ||
                                      ex is Exception ||
                                      ex is ArgumentNullException)
            {
                await App.MainRoot.ShowDialog("Error", ex.Message);
            }

            return logs;
        }

        public async Task<LogNotification> GetLog()
        {
            IFirebaseClient client;
            LogNotification temp = new LogNotification();

            try
            {
                client = await TryConnect();
                var res = client.Get("Logs/0");

                temp = res.ResultAs<LogNotification>();   
            }
            catch (Exception ex) when (ex is AggregateException ||
                                      ex is Exception ||
                                      ex is ArgumentNullException)
            {
                await App.MainRoot.ShowDialog("Error", ex.Message);
            }

            return temp;
        }
    }
}
