using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DailyScores.Models
{
    public class Player
    {
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public List<EmailAddress> EmailAddresses { get; set; } 
    }
}