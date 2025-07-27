using Aipazz.Application.Notification.Interfaces;
using System.Security.Claims;

namespace Aipazz.API.Controllers
{
    public class NotificationUserAssignmentMiddleware
    {
        private readonly RequestDelegate _next;

        public NotificationUserAssignmentMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var notificationRepository = context.RequestServices.GetRequiredService<INotificationRepository>();

                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userEmail = context.User.FindFirst(ClaimTypes.Email)?.Value;

                if (!string.IsNullOrWhiteSpace(userId) && !string.IsNullOrWhiteSpace(userEmail))
                {
                    await notificationRepository.AssignNotificationsToUserAsync(userEmail, userId);
                }
            }

            await _next(context);
        }
    }

}
