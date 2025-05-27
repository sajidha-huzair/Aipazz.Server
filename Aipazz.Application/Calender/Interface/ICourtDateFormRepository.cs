using Aipazz.Domian.Calender;

namespace Aipazz.Application.Calender.Interfaces
{
    public interface ICourtDateFormRepository
    {
        List<Domian.Calender.CourtDateForm> GetAll();
        Task<Domian.Calender.CourtDateForm?> GetById(Guid id);
        
        void AddCourtDateForm(Domian.Calender.CourtDateForm courtDateForm);
        
        Task<Domian.Calender.CourtDateForm> UpdateCourtDateForm(Guid modelId,Domian.Calender.CourtDateForm courtDateForm);
    }
}