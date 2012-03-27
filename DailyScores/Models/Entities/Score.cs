using System;

namespace DailyScores.Models
{
    public abstract class Score : BaseEntity
    {
        //public int ScoreId { get; set; }
        public int TotalScore { get; set; }

        public DateTime Date { get; set; }
        public string DisplayDate
        {
            get { return this.Date.ToShortDateString(); }
        }

        public int TimeInSeconds { get; set; }
        public string DisplayTime
        {
            get { return TimeSpan.FromSeconds(this.TimeInSeconds).ToString(); }
        }

        public int PlayerId { get; set; }
        public Player Player { get; set; }
    }
}