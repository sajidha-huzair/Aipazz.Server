using Aipazz.Domian.Calendar;

namespace Aipazz.Application.Calender.Interfaces
{
    public interface ICourtDateFormRepository
    {
        List<CourtDateForm> GetAll();
        // Other CRUD methods will go here later
    }
}