namespace DailyScores.Models
{
    public class EmailSubmission : BaseEntity
    {
        public long EmailSubmissionId { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}