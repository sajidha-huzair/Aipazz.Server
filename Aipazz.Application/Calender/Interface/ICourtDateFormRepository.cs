using Aipazz.Domian.Calender;

namespace Aipazz.Application.Calender.Interface
{
    public interface ICourtDateFormRepository
    {
        Task<List<CourtDateForm>> GetAll(string userId);
        Task<CourtDateForm?> GetById(Guid id);
        
        Task<CourtDateForm> AddCourtDateForm(CourtDateForm courtDateForm);
        
        Task<CourtDateForm?> UpdateCourtDateForm(Guid id, CourtDateForm courtDateForm);
        Task<CourtDateForm?> DeleteCourtDateForm(Guid id);

    }
}