using Aipazz.Domian.Calender;

namespace Aipazz.Application.Calender.Interfaces
{
    public interface IFilingsDeadlineFormRepository
    {
        List<Domian.Calender.FilingsDeadlineForm> GetAll();
        Task<Domian.Calender.FilingsDeadlineForm?> GetById(Guid id);
        void Add(Domian.Calender.FilingsDeadlineForm form);
        Task<Domian.Calender.FilingsDeadlineForm?> Update(Guid id, Domian.Calender.FilingsDeadlineForm form);
        Task<bool> Delete(Guid id);
    }
}