using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DailyScores.Models
{
    public class MissingScore
    {
        public DateTime Date { get; set; }
        public string DisplayDate
        {
            get { return this.Date.ToShortDateString(); }
        }

        public int NumberOfPlayers { get; set; }

        public string GameType { get; set; }
    }
}