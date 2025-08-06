using Quartz;
using System.Threading.Tasks;
using Aipazz.Application.Calender.Interface;

namespace AIpazz.Infrastructure.Jobs;

public class ClientMeetingReminderJob : IJob
{
    private readonly ICalenderEmailService _emailService;

    public ClientMeetingReminderJob(ICalenderEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var data = context.MergedJobDataMap;
        
        string recipient = data.GetString("email");
        string subject = data.GetString("subject");
        string body = data.GetString("body");

        await _emailService.sendEmaiToClient(recipient, subject, body);
    }
}
