using Aipazz.Application.Calender.Interface;
using Aipazz.Domian.Calender;

namespace AIpazz.Infrastructure.Calender
{
    public class CourtDateFormRepository : ICourtDateFormRepository
    {
        private static readonly List<CourtDateForm> _courtDates = new();

        public Task<List<CourtDateForm>> GetAllCourtDates()
        {
            return Task.FromResult(_courtDates);
        }

        public Task<CourtDateForm> GetCourtDateById(Guid id)
        {
            var result = _courtDates.FirstOrDefault(cd => cd.Id == id);
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