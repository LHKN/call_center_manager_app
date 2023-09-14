using ManagerApp.Model.HTTPResponseTemplate;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ManagerApp.Model
{
    interface IBookingRepository
    {
        Task<int> GetAvailableId();
        Task<bool> Add(BookingDetail booking);
        Task<bool> Edit(BookingDetail booking);
        Task<bool> Delete(int id);
        Task<BookingDetail> GetById(int id);
        Task<ObservableCollection<BookingDetail>> GetAll();

        Task AddLog(LogNotification log);
        Task<ObservableCollection<LogNotification>> GetAllLog();
        Task<LogNotification> GetLog();
    }
}
