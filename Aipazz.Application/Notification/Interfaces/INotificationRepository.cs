using System.Collections.Generic;
using System.Threading.Tasks;
using Aipazz.Domian.Notification;


namespace Aipazz.Application.Notification.Interfaces
{
    public interface INotificationRepository
    {
        Task<string> CreateNotificationAsync(Aipazz.Domian.Notification.Notification notification);
        Task<List<Aipazz.Domian.Notification.Notification>> GetUserNotificationsAsync(string userId);
        Task AssignNotificationsToUserAsync(string userEmail, string userId);
        Task<List<Aipazz.Domian.Notification.Notification>> GetUnreadNotificationsAsync(string userId);
        Task<bool> MarkAsReadAsync(string notificationId, string userId);
        Task<bool> DeleteNotificationAsync(string notificationId, string userId);
        Task<int> GetUnreadCountAsync(string userId);
    }
}