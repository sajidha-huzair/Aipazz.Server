namespace Aipazz.Application.Calender.Interface;

public interface IReminderScheduler
{
    Task ScheduleClientMeetingReminderAsync(Guid meetingId, string email, string title, DateTime meetingDate, TimeSpan reminderOffset);
    
    Task ScheduleCourtDateReminderAsync(DateTime triggerTime, string email, string subject, string body);

    public Task ScheduleFilingDeadlineReminderAsync(Guid filingId, string email, string title,
        DateTime deadlineDate, TimeSpan reminderOffset);

    Task ScheduleTeamMeetingReminderAsync(DateTime triggerTime, string email, string subject, string body);
}