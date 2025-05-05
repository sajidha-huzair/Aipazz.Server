namespace Aipazz.Domian.Calender;

public class CourtDateForm
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CourtDate { get; set; }
    public string CaseNumber { get; set; }
    public string CourtName { get; set; }
    public string Description { get; set; }
}