using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DailyScores.Models
{
    public class EmailRequest
    {
        public string Recipient { get; set; }
        public string Subject { get; set; }
        public string Sender { get; set; }
        public string Body { get; set; }
    }
}