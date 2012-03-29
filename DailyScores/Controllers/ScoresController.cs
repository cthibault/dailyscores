using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DailyScores.Framework;
using DailyScores.Framework.Controllers;
using DailyScores.Framework.Requests;
using DailyScores.Models;
using DailyScores.Parsers;
using Typesafe.Mailgun;

namespace DailyScores.Controllers
{
    public class ScoresController : BaseController
    {
        // GET: /Scores/
        public ActionResult Index()
        {
            var hidatoScores = this.Repository.HidatoScores
                    .Include("Player")
                    .OrderByDescending(s => s.HidatoId)
                    .Take(10)
                    .ToList();

            var jumbleScores = this.Repository.JumbleScores
                    .Include("Player")
                    .OrderByDescending(s => s.JumbleId)
                    .Take(10)
                    .ToList();


            this.ViewBag.HidatoScores = hidatoScores;
            this.ViewBag.JumbleScores = jumbleScores;

            return this.View();
        }


        // GET: /Scores/EmailSubmissions
        public ActionResult EmailSubmissions()
        {
            var emailSubmissions = this.Repository.EmailSubmissions
                    .OrderByDescending(e => e.EmailSubmissionId)
                    .Take(10)
                    .ToList();

            return this.View(emailSubmissions);
        }


        [ValidateInput(false)]
        [AcceptVerbs(HttpVerbs.Post)]
        public void EmailSubmission(EmailRequest request)
        {
            var submissionId = this.RecordEmailSubmission(request);

            var emailAddress = this.Repository.EmailAddresses.SingleOrDefault(e => e.Address == request.Sender);

            if (emailAddress != null && emailAddress.Player != null)
            {
                this.ParseAndSaveScores(emailAddress.Player, request);
            }
            else
            {
                //TODO: REMOVE
                this.AddToLogGroup("Unable to find PLAYER", "Submission Id: " + submissionId);
            }

            //TODO: REMOVE
            this.SaveAndCommitLogGroup();
        }


        #region Private Methods

        private void AddToLogGroup(params string[] messages)
        {
            if (messages.Any())
            {
                var builder = new StringBuilder();

                foreach (var message in messages)
                {
                    builder.AppendLine(message);
                }

                this.LogGroup.LogEntries.Add(new LogEntry
                                             {
                                                 Description = builder.ToString(),
                                             });
            }
        }

        private void SaveAndCommitLogGroup()
        {
            if (this.LogGroup.LogEntries.Any())
            {
                this.Repository.LogGroups.Add(this.LogGroup);
                this.Repository.SaveChanges();
            }
        }

        private void ParseAndSaveScores(Player player, EmailRequest email)
        {
            Response<HidatoScore> hidatoResponse = null;
            Response<JumbleScore> jumbleResponse = null;

            bool anyChanges = false;
            var bodyText = email.Body;
            var dateReponse = new DateParser().Parse(bodyText);

            //TODO: REMOVE
            this.AddToLogGroup("Parsing Date Successful = " + dateReponse.IsSuccess);


            if (dateReponse.IsSuccess)
            {
                bool hasHidatoEntry = this.Repository.HidatoScores.Any(s => s.PlayerId == player.PlayerId && s.Date == dateReponse.Value);
                //TODO: REMOVE
                this.AddToLogGroup("Hidato Score already exists = " + hasHidatoEntry);
                if (!hasHidatoEntry)
                {
                    hidatoResponse = new HidatoScoreParser().Parse(bodyText);
                    //TODO: REMOVE
                    this.AddToLogGroup("Parsing Hidato Score: Success = " + hidatoResponse.IsSuccess);
                    if (hidatoResponse.IsSuccess)
                    {
                        hidatoResponse.Value.PlayerId = player.PlayerId;
                        this.Repository.HidatoScores.Add(hidatoResponse.Value);
                        anyChanges = true;
                    }
                }

                bool hasJumbleEntry = this.Repository.JumbleScores.Any(s => s.PlayerId == player.PlayerId && s.Date == dateReponse.Value);
                //TODO: REMOVE
                this.AddToLogGroup("Jumble Score already exists = " + hasJumbleEntry);
                if (!hasJumbleEntry)
                {
                    jumbleResponse = new JumbleScoreParser().Parse(bodyText);
                    //TODO: REMOVE
                    this.AddToLogGroup("Parsing Jumble Score: Success = " + jumbleResponse.IsSuccess);
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
                //TODO: REMOVE
                this.AddToLogGroup(builder.ToString());
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

        private long RecordEmailSubmission(EmailRequest request)
        {
            //TODO: REMOVE
            this.AddToLogGroup("Begin saving EMAIL SUBMISSION");

            var body = request.Body.Replace(Environment.NewLine, "[newline]");

            var submission = new EmailSubmission
                             {
                                 From = request.Sender,
                                 To = request.Recipient,
                                 Subject = request.Subject,
                                 Body = body
                             };

            this.Repository.EmailSubmissions.Add(submission);
            this.Repository.SaveChanges();

            //TODO: REMOVE
            this.AddToLogGroup("Saved EMAIL SUBMISSION: " + submission.EmailSubmissionId);

            return submission.EmailSubmissionId;
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

        #region Private Properties

        private LogGroup _logGroup;
        private LogGroup LogGroup
        {
            get
            {
                if (this._logGroup == null)
                {
                    this._logGroup = new LogGroup { Description = "Scores Controller" };
                    this._logGroup.LogEntries = new List<LogEntry>();
                }

                return this._logGroup;
            }
            set { this._logGroup = value; }
        }

        #endregion Private Properties
    }
}
