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

        // GET: /Scores/
        public ActionResult Index()
        {
            return this.View();
        }


        // GET: /Scores/EmailSubmissions
        public ActionResult EmailSubmissions()
        {
            var emailSubmissions = new List<EmailSubmission>();

            try
            {
                emailSubmissions = this.Repository.EmailSubmissions
                    .OrderByDescending(e => e.EmailSubmissionId)
                    .Take(10)
                    .ToList();
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
            this.RecordEmailSubmission(request);

            var emailAddress = this.Repository.EmailAddresses.SingleOrDefault(e => e.Address == request.Sender);

            if (emailAddress != null && emailAddress.Player != null)
            {
                this.ParseAndSaveScores(emailAddress.Player, request);
            }
        }

        #region Private Methods

        private void ParseAndSaveScores(Player player, EmailRequest email)
        {
            Response<HidatoScore> hidatoResponse = null;
            Response<JumbleScore> jumbleResponse = null;

            var bodyText = email.Body;
            var dateReponse = new DateParser().Parse(bodyText);
            bool anyChanges = false;

            if (dateReponse.IsSuccess)
            {
                bool hasHidatoEntry = this.Repository.HidatoScores.Any(s => s.Date == dateReponse.Value);
                if (!hasHidatoEntry)
                {
                    hidatoResponse = new HidatoScoreParser().Parse(bodyText);
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
                    jumbleResponse = new JumbleScoreParser().Parse(bodyText);
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

                this.Validate(hidatoResponse, jumbleResponse, email);
            }
        }

        private void Validate(Response<HidatoScore> hidatoResponse, Response<JumbleScore> jumbleResponse, EmailRequest email)
        {
            var builder = new StringBuilder();

            if (hidatoResponse != null && !hidatoResponse.IsSuccess)
            {
                builder.AppendLine(this.BuildErrorReport(hidatoResponse));
            }

            if (jumbleResponse != null && !jumbleResponse.IsSuccess)
            {
                builder.AppendLine(this.BuildErrorReport(jumbleResponse));
            }

            var errorReport = builder.ToString();

            if (!string.IsNullOrEmpty(errorReport))
            {
                this.EmailErrorReport(errorReport, email);
            }
        }

        private string BuildErrorReport<T>(Response<T> response)
        {
            var builder = new StringBuilder();

            if (response.ErrorMessages.Any())
            {
                builder.AppendLine("ERROR MESSAGES:");
                response.ErrorMessages.ForEach(error => builder.AppendLine("  " + error));
            }
            if (response.Exceptions.Any())
            {
                builder.AppendLine("EXCEPTIONS");
                response.Exceptions.ForEach(ex =>
                                                  {
                                                      builder.AppendLine(string.Format("  Message: {0}\r\n  Stack: {1}", ex.Message, ex.StackTrace));
                                                      Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                                                  });
            }

            return builder.ToString();
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

        private void EmailErrorReport(string errorMessages, EmailRequest email)
        {
            var mailgunClient = new MailgunClient("dailyscores.mailgun.org", "key-6ivkuetilj5gtaripxidk04k-1lqr0v6");
            var message = new MailMessage("scores@dailyscores.mailgun.org", "curtistbone@gmail.com")
                          {
                              Subject = "Email Submission/Parse Error",
                              Body = string.Format("FROM: {0}\r\n\r\nBODY:\r\n{1}\r\n\r\nERRORS\r\n{2}", email.Sender, email.Body, errorMessages)
                          };

            mailgunClient.SendMail(message);
        }

        #endregion Private Methods
    }
}
