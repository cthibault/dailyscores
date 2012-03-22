using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using DailyScores.Models;
using Typesafe.Mailgun;

namespace DailyScores.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            var players = new List<Player>();

            try
            {
                var db = new DailyScoresEntities();
                players = db.Players.ToList();
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }
            

            return View(players);
        }
    }
}
