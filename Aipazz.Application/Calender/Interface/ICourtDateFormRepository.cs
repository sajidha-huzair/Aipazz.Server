using Aipazz.Domian.Calender;

namespace Aipazz.Application.Calender.Interfaces
{
    public interface ICourtDateFormRepository
    {
        Task<List<Aipazz.Domian.Calender.CourtDateForm>> GetAll();
        Task<Aipazz.Domian.Calender.CourtDateForm?> GetById(Guid id);
        
        Task AddCourtDateForm(Aipazz.Domian.Calender.CourtDateForm courtDateForm);
        
        Task<Aipazz.Domian.Calender.CourtDateForm> UpdateCourtDateForm(Guid modelId,Aipazz.Domian.Calender.CourtDateForm courtDateForm);
        Task<bool> DeleteCourtDateForm(Guid id);

    }
}