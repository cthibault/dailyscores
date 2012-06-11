using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DailyScores.Models
{
    public class Statistics
    {
        public string Title { get; set; }

        public int GamesPlayed { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Ties { get; set; }
        public int HighScore { get; set; }
        public int LowScore { get; set; }
        public double AverageScore { get; set; }
        public DateTime Range { get; set; }
    }
}