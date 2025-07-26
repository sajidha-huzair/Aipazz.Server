using Aipazz.Domian.Billing;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aipazz.Application.Notification.Services
{
    public interface INotificationService
    {
        Task SendTeamCreatedNotificationAsync(string teamId, string teamName, string ownerName, string ownerId, List<string> memberIds, List<string> memberEmails);
        Task SendTeamAssignmentNotificationAsync(string teamId, string teamName, string memberEmail, string assignedBy);
        Task SendDocumentSharedNotificationAsync(string documentId, string documentName, string teamId, string teamName, string sharedBy);
        Task NotifyLawyerPaymentReceived(Invoice invoice);

    }
}