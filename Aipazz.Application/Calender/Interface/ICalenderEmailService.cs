namespace Aipazz.Application.Calender.Interface;

public interface ICalenderEmailService
{
    
    public Task sendEmaiToClient( string  receptor, string subject, string body);

    Task SendCourtDateEmailToClientAsync(
        string clientEmail,
        string title,
        string? courtType,
        string? stage,
        DateTime courtDate,
        TimeSpan reminder,
        string? note);

}