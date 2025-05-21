using Aipazz.Application.Calender.Interfaces;
using Aipazz.Domian.Calendar;

namespace Aipazz.Infrastructure.Calendar
{
    public class CourtDateFormRepository : ICourtDateFormRepository
    {
        
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
        
        
        
        private readonly List<CourtDateForm> _courtDates = new();

        public List<CourtDateForm> GetAll()
        {
            return _courtDates;
        }

        // Other methods (Add, Update, Delete) will go here
        
        public Task<CourtDateForm?> GetById(Guid id)
        {
            var courtDate = _courtDates.FirstOrDefault(cd => cd.Id == id);
            return Task.FromResult(courtDate);
        }
    }
}