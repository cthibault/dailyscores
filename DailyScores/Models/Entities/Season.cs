using System;

namespace DailyScores.Models
{
    public class Season : BaseEntity
    {
        public int SeasonId { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}