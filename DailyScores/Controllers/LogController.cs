using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DailyScores.Models;

namespace DailyScores.Controllers
{
    public class LogController : BaseController
    {
        //
        // GET: /Log/

        public ActionResult Index()
        {
            var logGroups = this.Repository.LogGroups
                .Include("LogEntries")
                .OrderByDescending(g => g.GroupId)
                .Take(10)
                .ToList();

            return View(logGroups);
        }
    }
}
