using Aipazz.Application.Calender.Interfaces;
using Aipazz.Domian.Calender;

namespace AIpazz.Infrastructure.Calendar
{
    public class FilingsDeadlineFormRepository : IFilingsDeadlineFormRepository
    {
        private readonly List<FilingsDeadlineForm> _forms = new();

        public FilingsDeadlineFormRepository()
        {
            _forms.Add(new FilingsDeadlineForm
            {
                Id = Guid.NewGuid(),
                Title = "Initial Filing",
                Date = DateTime.UtcNow.AddDays(2),
                Time = "10:00 AM",
                Reminder = "1 day before",
                Description = "File complaint with supporting documents.",
                AssignedMatter = "Civil Case A"
            });
        }

        public List<FilingsDeadlineForm> GetAll()
        {
            return _forms;
        }

        public Task<FilingsDeadlineForm?> GetById(Guid id)
        {
            var form = _forms.FirstOrDefault(f => f.Id == id);
            return Task.FromResult(form);
        }

        public void Add(FilingsDeadlineForm form)
        {
            _forms.Add(form);
        }

        public Task<FilingsDeadlineForm?> Update(Guid id, FilingsDeadlineForm updatedForm)
        {
            var existing = _forms.FirstOrDefault(f => f.Id == id);
            if (existing == null) return Task.FromResult<FilingsDeadlineForm?>(null);

            existing.Title = updatedForm.Title;
            existing.Date = updatedForm.Date;
            existing.Time = updatedForm.Time;
            existing.Reminder = updatedForm.Reminder;
            existing.Description = updatedForm.Description;
            existing.AssignedMatter = updatedForm.AssignedMatter;

            return Task.FromResult(existing);
        }

        public Task<bool> Delete(Guid id)
        {
            var form = _forms.FirstOrDefault(f => f.Id == id);
            if (form == null) return Task.FromResult(false);

            _forms.Remove(form); 
            return Task.FromResult(true);

        }
        
        
    }
}
