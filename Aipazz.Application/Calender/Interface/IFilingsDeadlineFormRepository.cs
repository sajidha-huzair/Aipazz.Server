using Aipazz.Domian.Calender;

namespace Aipazz.Application.Calender.Interfaces
{
    public interface IFilingsDeadlineFormRepository
    {
        Task<List<Aipazz.Domian.Calender.FilingsDeadlineForm>> GetAll();
        Task<Aipazz.Domian.Calender.FilingsDeadlineForm?> GetById(Guid id);
        Task Add(Aipazz.Domian.Calender.FilingsDeadlineForm form);
        Task<Aipazz.Domian.Calender.FilingsDeadlineForm?> Update(Guid id, Aipazz.Domian.Calender.FilingsDeadlineForm form);
        Task<bool> Delete(Guid id);
    }
}