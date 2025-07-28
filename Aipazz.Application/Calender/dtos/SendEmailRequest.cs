namespace Aipazz.Application.Calender.dtos;

public class SendEmailRequest
{
    
    public List<string> Recipients { get; set; } = [];
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    
}