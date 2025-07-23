namespace Aipazz.Domian.Matters
{
    public class MatterTask
    {
        public string? id { get; set; }
        public string? MatterId { get; set; } // Links task to a matter
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? AssignedTo { get; set; }

    }
}