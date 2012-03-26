using System;

namespace DailyScores.Models
{
    public abstract class Score : BaseEntity
    {
        //public int ScoreId { get; set; }
        public int PlayerId { get; set; }
        public DateTime Date { get; set; }

        public int TotalScore { get; set; }
        public int TimeInSeconds { get; set; }
    }
}