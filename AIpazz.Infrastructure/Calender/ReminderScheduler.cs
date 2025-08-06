using Aipazz.Application.Calender.Interface;
using AIpazz.Infrastructure.Jobs;
using Quartz;

namespace AIpazz.Infrastructure.Calender;

public class ReminderScheduler(IScheduler scheduler) : IReminderScheduler
{
    public async Task ScheduleClientMeetingReminderAsync(Guid meetingId, string email, string title, DateTime meetingDate, TimeSpan reminderOffset)
    {
        var jobData = new JobDataMap
        {
            { "email", email },
            { "subject", "Reminder: Upcoming Client Meeting" },
            { "body", $"Your client meeting \"{title}\" is scheduled for {meetingDate:dddd, MMMM dd}." }
        };

        var job = JobBuilder.Create<ClientMeetingReminderJob>()
            .WithIdentity($"clientMeeting_{meetingId}", "reminders")
            .UsingJobData(jobData)
            .Build();

        var trigger = TriggerBuilder.Create()
            .WithIdentity($"trigger_{meetingId}", "reminders")
            .StartAt(meetingDate.Subtract(reminderOffset)) // e.g., 1 day before
            .Build();

        await scheduler.ScheduleJob(job, trigger);
    }

    public async Task ScheduleCourtDateReminderAsync(DateTime triggerTime, string email, string subject, string body)
    {
        var job = JobBuilder.Create<Jobs.CourtDateReminder>()
            .WithIdentity(Guid.NewGuid().ToString(), "CourtDateReminders")
            .UsingJobData("email", email)
            .UsingJobData("subject", subject)
            .UsingJobData("body", body)
            .Build();

        var trigger = TriggerBuilder.Create()
            .WithIdentity(Guid.NewGuid().ToString(), "CourtDateReminders")
            .StartAt(new DateTimeOffset(triggerTime))
            .Build();

        await  scheduler.ScheduleJob(job, trigger);
    }

    public async Task ScheduleFilingDeadlineReminderAsync(Guid filingId, string email, string title,
        DateTime deadlineDate, TimeSpan reminderOffset)
    {
        var jobData = new JobDataMap
        {
            { "email", email },
            { "subject", "Reminder: Filing Deadline Approaching" },
            { "body", $"Your filing deadline titled \"{title}\" is scheduled for {deadlineDate:dddd, MMMM dd}." }
        };

        var job = JobBuilder.Create<FillingDeadlineReminder>()
            .WithIdentity($"filing_{filingId}", "reminders")
            .UsingJobData(jobData)
            .Build();

        var trigger = TriggerBuilder.Create()
            .WithIdentity($"trigger_{filingId}", "reminders")
            .StartAt(deadlineDate.Subtract(reminderOffset)) // E.g., 1 day before
            .Build();

        await scheduler.ScheduleJob(job, trigger);
    }

    public async Task ScheduleTeamMeetingReminderAsync(DateTime triggerTime, string email, string subject, string body)
    {
        var job = JobBuilder.Create<Jobs.CourtDateReminder>()
            .WithIdentity(Guid.NewGuid().ToString(), "TeamMeetingReminders")
            .UsingJobData("email", email)
            .UsingJobData("subject", subject)
            .UsingJobData("body", body)
            .Build();

        var trigger = TriggerBuilder.Create()
            .WithIdentity(Guid.NewGuid().ToString(), "TeamMeetingReminders")
            .StartAt(new DateTimeOffset(triggerTime))
            .Build();

        await  scheduler.ScheduleJob(job, trigger);
    }
}