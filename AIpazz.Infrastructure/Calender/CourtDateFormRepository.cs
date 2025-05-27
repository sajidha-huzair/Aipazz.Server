using Aipazz.Application.Calender.Interfaces;
using Aipazz.Domian.Calender;

namespace Aipazz.Infrastructure.Calendar
{
    public class CourtDateFormRepository : ICourtDateFormRepository
    {
        private readonly List<CourtDateForm> _courtDates = new List<CourtDateForm>();
        public CourtDateFormRepository()
        {
            _courtDates.Add(new CourtDateForm
            {
                Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                CaseNumber = "ABC123",
                CourtName = "Supreme Court",
                Date = DateTime.UtcNow.AddDays(7),
                Description = "Hearing for civil case"
            });
        }
        

        public List<CourtDateForm> GetAll()
        {
            return _courtDates;
        }
        
        
        public Task<CourtDateForm?> GetById(Guid id)
        {
            var courtDate = _courtDates.FirstOrDefault(cd => cd.Id == id);
            return Task.FromResult(courtDate);
        }
        
        
        public void AddCourtDateForm(CourtDateForm courtDateForm) // 👈 Add this method
        {
            _courtDates.Add(courtDateForm);
        }

        public Task<CourtDateForm?> UpdateCourtDateForm(Guid modelId, CourtDateForm courtDateForm)
        {
            var existing = _courtDates.FirstOrDefault(cd => cd.Id == modelId);
    
            if (existing == null)
            {
                return Task.FromResult<CourtDateForm?>(null);
            }

            existing.CaseNumber = courtDateForm.CaseNumber;
            existing.CourtName = courtDateForm.CourtName;
            existing.Date = courtDateForm.Date;

            return Task.FromResult(existing);
        }

    }
}