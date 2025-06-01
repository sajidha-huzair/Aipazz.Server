using Aipazz.Domian.Calender;

namespace Aipazz.Application.Calender.Interface
{
    public interface ITeamMeetingFormRepository
    {
        List<Domian.Calender.TeamMeetingForm> GetAll();
        Task<Domian.Calender.TeamMeetingForm?> GetById(Guid id);
        void Add(Domian.Calender.TeamMeetingForm form);
        Task<Domian.Calender.TeamMeetingForm?> Update(Guid id, Domian.Calender.TeamMeetingForm form);
        Task<bool> Delete(Guid id);


        
    }
}