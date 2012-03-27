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
using DailyScores.Models.Parsers;
using DailyScores.Models.Requests;
using Typesafe.Mailgun;

namespace DailyScores.Controllers
{
    public class ScoresController : Controller
    {
        #region Private Properties

        private DailyScoresEntities _repository;
        private DailyScoresEntities Repository
        {
            get
            {
                if (this._repository == null)
                {
                    this._repository = new DailyScoresEntities();
                }

                return this._repository;
            }
        }

        #endregion Private Properties

        //
        // GET: /Scores/
        public ActionResult EmailSubmissions()
        {
            var emailSubmissions = new List<EmailSubmission>();

            try
            {
                emailSubmissions = this.Repository.EmailSubmissions.ToList();
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
            var emailAddress = this.Repository.EmailAddresses.SingleOrDefault(e => e.Address == request.Sender);

            if (emailAddress != null && emailAddress.Player != null)
            {
                this.ParseAndSaveScores(emailAddress.Player, request.Body);
            }

            this.RecordEmailSubmission(request);
        }

        #region Private Methods

        private void ParseAndSaveScores(Player player, string bodyText)
        {
            var dateReponse = new DateParser().Parse(bodyText);
            bool anyChanges = false;

            if (dateReponse.IsSuccess)
            {
                bool hasHidatoEntry = this.Repository.HidatoScores.Any(s => s.Date == dateReponse.Value);
                if (!hasHidatoEntry)
                {
                    var hidatoResponse = new HidatoScoreParser().Parse(bodyText);
                    if (hidatoResponse.IsSuccess)
                    {
                        hidatoResponse.Value.PlayerId = player.PlayerId;
                        this.Repository.HidatoScores.Add(hidatoResponse.Value);
                        anyChanges = true;
                    }
                }

                bool hasJumbleEntry = this.Repository.JumbleScores.Any(s => s.Date == dateReponse.Value);
                if (!hasJumbleEntry)
                {
                    var jumbleResponse = new JumbleScoreParser().Parse(bodyText);
                    if (jumbleResponse.IsSuccess)
                    {
                        jumbleResponse.Value.PlayerId = player.PlayerId;
                        this.Repository.JumbleScores.Add(jumbleResponse.Value);
                        anyChanges = true;
                    }
                }

                if (anyChanges)
                {
                    this.Repository.SaveChanges();
                }
            }
        }

        private void RecordEmailSubmission(EmailRequest request)
        {
            var submission = new EmailSubmission
                             {
                                 From = request.Sender,
                                 To = request.Recipient,
                                 Subject = request.Subject,
                                 Body = request.Body
                             };

            this.Repository.EmailSubmissions.Add(submission);
            this.Repository.SaveChanges();
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

        #endregion Private Methods
    }
}
