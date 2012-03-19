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

        public ActionResult Math(double x, double y)
        {
            this.ViewBag.X = x;
            this.ViewBag.Y = y;

            return View();
        }

        //private void SendMailTest()
        //{
        //    var mailgunClient = new MailgunClient("dailyscores.mailgun.org", "key-6ivkuetilj5gtaripxidk04k-1lqr0v6");
        //    var message = new MailMessage("submit@dailyscores.mailgun.org", "dailyscores_ml@dailyscores.mailgun.org")
        //                  {
        //                      Subject = "Test Subject",
        //                      Body = "Test message for the email body"
        //                  };

        //    mailgunClient.SendMail(message);
        //}
    }
}
