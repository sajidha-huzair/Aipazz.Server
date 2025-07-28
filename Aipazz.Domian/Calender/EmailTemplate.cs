namespace Aipazz.Domian.Calender;

public static class EmailTemplate
{
    public static string WelcomeBody(string title, DateOnly date, TimeOnly time, string meetingLink, string location)
    {
        return $@"
Dear Sir/Madam,

We hope this message finds you well.

This is to formally inform you that a meeting has been scheduled regarding **{title}**.

📅 **Date**: {date:dddd, MMMM dd, yyyy}  
⏰ **Time**: {time:hh:mm tt}  
📍 **Location**: {location}  
🔗 **Join Online**: {meetingLink}

Please ensure your availability at the specified date and time. If you are joining remotely, use the provided link to access the meeting.

If you have any questions or require further details, feel free to contact us.

Kind regards,  
**The AI PAZZ Legal Team**
";
    }
}