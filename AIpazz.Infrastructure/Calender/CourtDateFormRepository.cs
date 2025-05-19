using Aipazz.Application.Calender.Interfaces;
using Aipazz.Domian.Calendar;

namespace Aipazz.Infrastructure.Calendar
{
    public class CourtDateFormRepository : ICourtDateFormRepository
    {
        private readonly List<CourtDateForm> _courtDates = new();

        public List<CourtDateForm> GetAll()
        {
            return _courtDates;
        }

        // Other methods (Add, Update, Delete) will go here
    }
}