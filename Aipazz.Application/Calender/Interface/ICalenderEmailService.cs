namespace Aipazz.Application.Calender.Interface;

public interface ICalenderEmailService
{
    
    public Task sendEmaiToClient( string  receptor, string subject, string body);
    
}