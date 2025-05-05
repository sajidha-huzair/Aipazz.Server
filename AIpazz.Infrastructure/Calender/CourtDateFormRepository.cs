using Aipazz.Application.Calender.Interface;
using Aipazz.Domian.Calender; // ✅ Make sure the namespace is spelled correctly in your project!

namespace AIpazz.Infrastructure.Calender
{
    public class CourtDateFormRepository : ICourtDateFormRepository
    {
        // ✅ Only one declaration of _courtDates
        private readonly List<CourtDateForm> _courtDates = new()
        {
            new CourtDateForm
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                CourtDate = DateTime.Today.AddDays(3),
                CaseNumber = "CASE-001",
                CourtName = "Supreme Court",
                Description = "Hearing of primary case."
            }
        };

        public Task<List<CourtDateForm>> GetAllCourtDates()
        {
            return Task.FromResult(_courtDates);
        }

        public Task<CourtDateForm> GetCourtDateFormById(Guid id)
        {
            var result = _courtDates.FirstOrDefault(c => c.Id == id);
            return Task.FromResult(result);
        }

        public Task AddCourtDate(CourtDateForm courtDateForm)
        {
            _courtDates.Add(courtDateForm);
            return Task.CompletedTask;
        }

        public Task UpdateCourtDate(CourtDateForm courtDateForm)
        {
            var existing = _courtDates.FirstOrDefault(cd => cd.Id == courtDateForm.Id);
            if (existing != null)
            {
                _courtDates.Remove(existing);
                _courtDates.Add(courtDateForm);
            }
            return Task.CompletedTask;
        }

        public Task DeleteCourtDate(Guid id)
        {
            var toDelete = _courtDates.FirstOrDefault(cd => cd.Id == id);
            if (toDelete != null)
            {
                _courtDates.Remove(toDelete);
            }
            return Task.CompletedTask;
        }
    }
}