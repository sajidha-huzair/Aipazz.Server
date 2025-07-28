using Aipazz.Domain.Calender;
using FilingsDeadlineFormEntity = Aipazz.Domain.Calender.FilingsDeadlineForm;

namespace Aipazz.Application.Calender.Interfaces
{
    public interface IFilingsDeadlineFormRepository
    {
        Task<List<FilingsDeadlineFormEntity>> GetAll(string userId);
        Task<FilingsDeadlineFormEntity?> GetById(Guid id);
        Task Add(FilingsDeadlineFormEntity form);
        Task<FilingsDeadlineFormEntity?> Update(Guid id, FilingsDeadlineFormEntity form);
        Task<bool> Delete(Guid id);
    }
}