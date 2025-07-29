using Aipazz.Application.Calender.Interface;
using Quartz;

namespace AIpazz.Infrastructure.Jobs;

public class CourtDateReminder(ICalenderEmailService  emailService) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var data = context.MergedJobDataMap;
        
        string recipient = data.GetString("email");
        string subject = data.GetString("subject");
        string body = data.GetString("body");

        await emailService.sendEmaiToClient(recipient, subject, body);
    }
}