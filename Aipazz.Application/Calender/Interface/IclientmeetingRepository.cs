using Aipazz.Domian.Calender;

namespace Aipazz.Application.Calender.Interface;

public interface IclientmeetingRepository
{
    Task<List<ClientMeeting>> GetAllClientMeetings(string userId);
    Task AddClientMeeting(ClientMeeting Meeting);
    Task<ClientMeeting> GetClientMeetingByID(Guid id);
    Task<ClientMeeting> UpdateClientMeeting(ClientMeeting Meeting);
    Task<bool> DeleteClientMeeting(Guid id);  
   
}