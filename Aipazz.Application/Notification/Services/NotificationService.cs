using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aipazz.Application.Notification.Interfaces;
using Aipazz.Application.Notification.Services;
using Aipazz.Domian.Notification;

namespace Aipazz.Application.Notification.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task SendTeamCreatedNotificationAsync(string teamId, string teamName, string ownerName, string ownerId, List<string> memberIds)
        {
            var notifications = new List<Aipazz.Domian.Notification.Notification>();

            // Notification for the owner
            notifications.Add(new Aipazz.Domian.Notification.Notification
            {
                id = Guid.NewGuid().ToString(),
                UserId = ownerId,
                Type = "TeamCreated",
                Title = "Team Created Successfully",
                Message = $"You have successfully created the team '{teamName}'.",
                RelatedEntityId = teamId,
                RelatedEntityType = "Team",
                CreatedBy = ownerName,
                ActionUrl = $"/teams/{teamId}"
            });

            // Notifications for team members
            foreach (var memberId in memberIds)
            {
                if (memberId != ownerId) // Don't send duplicate notification to owner
                {
                    notifications.Add(new Aipazz.Domian.Notification.Notification
                    {
                        id = Guid.NewGuid().ToString(),
                        UserId = memberId,
                        Type = "TeamAssignment",
                        Title = "Added to New Team",
                        Message = $"You have been added to the team '{teamName}' by {ownerName}.",
                        RelatedEntityId = teamId,
                        RelatedEntityType = "Team",
                        CreatedBy = ownerName,
                        ActionUrl = $"/teams/{teamId}"
                    });
                }
            }

            // Save all notifications
            foreach (var notification in notifications)
            {
                await _notificationRepository.CreateNotificationAsync(notification);
            }
        }

        public async Task SendTeamAssignmentNotificationAsync(string teamId, string teamName, string memberEmail, string assignedBy)
        {
            // Implementation for future use
            throw new NotImplementedException("Team assignment notifications will be implemented next");
        }

        public async Task SendDocumentSharedNotificationAsync(string documentId, string documentName, string teamId, string teamName, string sharedBy)
        {
            // Implementation for future use
            throw new NotImplementedException("Document shared notifications will be implemented next");
        }
    }
}