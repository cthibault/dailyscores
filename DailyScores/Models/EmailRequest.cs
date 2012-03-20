using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DailyScores.Models
{
    public class EmailRequest
    {
        [AllowHtml]
        public string Subject { get; set; }
    }
}