using Aipazz.Domian.Calender;

namespace Aipazz.Application.Calender.Interfaces
{
    public interface ITeamMeetingFormRepository
    {
        List<TeamMeetingForm> GetAll();
        Task<TeamMeetingForm?> GetById(Guid id);
        void Add(TeamMeetingForm form);
        Task<TeamMeetingForm?> Update(Guid id, TeamMeetingForm form);
        Task<bool> Delete(Guid id);
    }
}