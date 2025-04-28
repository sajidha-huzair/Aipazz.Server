using Aipazz.Domian.Calender;

namespace Aipazz.Application.Calender.Interface;

public interface IclientmeetingRepository
{
    Task<List<ClientMeeting>> GetAllClientMeetings();
    Task AddClientMeeting(ClientMeeting Meeting);
    Task<ClientMeeting> GetClientMeetingByID(int id);
  
   
}