namespace Aipazz.Domian.Calender;

public class CourtDateForm
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string CaseTitle { get; set; }
    public DateOnly CourtDate { get; set; }
    public TimeOnly Time { get; set; }
    public string CourtLocation { get; set; }
    public string JudgeName { get; set; }
    public string Notes { get; set; }
}