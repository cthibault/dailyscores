namespace DailyScores.Models.Requests
{
    public class EmailRequest
    {
        public string Recipient { get; set; }
        public string Subject { get; set; }
        public string Sender { get; set; }
        public string Body { get; set; }
    }
}