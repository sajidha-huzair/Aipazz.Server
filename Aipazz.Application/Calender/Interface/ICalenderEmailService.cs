namespace Aipazz.Application.Calender.Interface;

public interface IEmailService
{
    
    public Task sendEmaiToClient( string  receptor, string subject, string body);
    
}