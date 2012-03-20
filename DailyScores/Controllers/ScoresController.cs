using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DailyScores.Binders;
using DailyScores.Models;
using Typesafe.Mailgun;

namespace DailyScores.Controllers
{
    public class ScoresController : Controller
    {
        //
        // GET: /Scores/
        public ActionResult Index()
        {
            var emailSubmissions = new List<EmailSubmission>();

            try
            {
                var db = new DailyScoresEntities();
                emailSubmissions = db.EmailSubmissions.ToList();
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }


            return this.View(emailSubmissions);
        }

        [ValidateInput(false)]
        [AcceptVerbs(HttpVerbs.Post)]
        public void EmailSubmission(EmailRequest request)
        {
            var builder = new StringBuilder();

            if (request != null)
            {
                builder.AppendLine("Subject -> " + request.Subject);
            }
            else
            {
                builder.AppendLine("Request is null");
            }

            SendMailTest(builder.ToString());
        }

        private void RecordEmailSubmission()
        {
            //var body = new StringBuilder();
            //body.AppendLine(DateTime.Now.ToString());
            //body.AppendLine(string.Format("request param is null: {0}", request == null));
            //body.AppendLine(string.Format("Request.Form prop is null: {0}", this.Request == null || this.Request.Form == null));

            //if (request != null)
            //{
            //    body.AppendLine("request collection:");
            //    foreach (var key in request.AllKeys)
            //    {
            //        body.AppendLine(string.Format("  {0} -- {1}", key, request[key]));
            //    }
            //}

            //string bodyText = body.ToString();
            //this.SendMailTest(bodyText);
            //var submission = new EmailSubmission
            //                 {
            //                     From = "Test",
            //                     To = "Test",
            //                     Subject = "Test",
            //                     Body = bodyText
            //                 };

            //var repo = new DailyScoresEntities();
            //repo.EmailSubmissions.Add(submission);
            //repo.SaveChanges();
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

        private void SendMailTest(string body)
        {
            var mailgunClient = new MailgunClient("dailyscores.mailgun.org", "key-6ivkuetilj5gtaripxidk04k-1lqr0v6");
            var message = new MailMessage("scores@dailyscores.mailgun.org", "curtistbone@gmail.com")
                          {
                              Subject = "Email Submission Response",
                              Body = body
                          };

            mailgunClient.SendMail(message);
        }
    }
}
