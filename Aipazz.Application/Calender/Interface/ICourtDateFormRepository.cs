using Aipazz.Domian.Calender;

namespace Aipazz.Application.Calender.Interface
{
    public interface ICourtDateFormRepository
    {
        Task<List<CourtDateForm>> GetAllCourtDates();
        Task<CourtDateForm> GetCourtDateFormById(Guid id);
        Task AddCourtDate(CourtDateForm courtDateForm);
        Task UpdateCourtDate(CourtDateForm courtDateForm);
        Task DeleteCourtDate(Guid id);
    }
}