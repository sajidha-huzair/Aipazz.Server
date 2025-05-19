namespace Aipazz.Domian.Calendar;

    public class CourtDateForm
    {
        public Guid Id { get; set; }
        public string CaseTitle { get; set; }
        public DateTime CourtDate { get; set; }
        public string CourtLocation { get; set; }
        public string JudgeName { get; set; }
        public string Description { get; set; }
    }
