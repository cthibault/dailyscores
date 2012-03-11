﻿using System;
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
            //var db = new DailyScoresEntities();
            //var players = db.Players.ToList();

            //this.SendMailTest();

            //return View(players);
            return View(new List<Player>());
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
