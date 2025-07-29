using Aipazz.Domian.Calender;

namespace Aipazz.Application.Calender.Interface
{
    public interface ITeamMeetingFormRepository
    {
        Task<List<Aipazz.Domian.Calender.TeamMeetingForm>> GetAll(string userId);
        Task<Aipazz.Domian.Calender.TeamMeetingForm?> GetById(Guid id);
        Task Add(Aipazz.Domian.Calender.TeamMeetingForm form);
        Task<Aipazz.Domian.Calender.TeamMeetingForm?> Update(Guid id, Aipazz.Domian.Calender.TeamMeetingForm form);
        Task<bool> Delete(Guid id);


        
    }
}