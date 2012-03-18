using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DailyScores.Binders;
using DailyScores.Models;

namespace DailyScores.Controllers
{
    public class ScoresController : Controller
    {
        //
        // GET: /Scores/
        public string Index()
        {
            return "Scores.Index()";
        }

        [HttpPost]
        [ValidateInput(false)]
        public void EmailSubmission([ModelBinder(typeof(JsonFormModelBinder))] EmailRequest request)
        {
            //EmailSubmission submission = this.Request != null && this.Request.Form != null
            //                                 ? this.RequestEmailSubmission() : this.NoRequestEmailSubmission();

            //var repo = new DailyScoresEntities();
            //repo.EmailSubmissions.Add(submission);

            var submission = new EmailSubmission { From = "Test", To = "Test", Subject = "Test", Body = DateTime.Now.ToString() };
            var repo = new DailyScoresEntities();
            repo.EmailSubmissions.Add(submission);
            repo.SaveChanges();
        }

        private EmailSubmission NoRequestEmailSubmission()
        {
            return new EmailSubmission() { Body = "No Request" };
        }

        private EmailSubmission RequestEmailSubmission()
        {
            var from = Request.Form.AllKeys.Contains("From") ? Request.Form["From"] : string.Empty;
            var to = Request.Form.AllKeys.Contains("To") ? Request.Form["To"] : string.Empty;
            var subject = Request.Form.AllKeys.Contains("subject") ? Request.Form["subject"] : string.Empty;
            var strippedText = Request.Form.AllKeys.Contains("stripped-text") ? Request.Form["stripped-text"] : string.Empty;

            return new EmailSubmission
                   {
                       To = to,
                       From = from,
                       Subject = subject,
                       Body = strippedText
                   };
        }
    }
}
