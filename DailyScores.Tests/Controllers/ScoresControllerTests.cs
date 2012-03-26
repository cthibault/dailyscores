using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DailyScores.Controllers;
using DailyScores.Models.Requests;
using Xunit;

namespace DailyScores.Tests.Controllers
{
    public class ScoresControllerTests
    {
        [Fact]
        public void EmailSubmissionTest()
        {
            var request = new EmailRequest
                          {
                              Sender = "curtistbone@gmail.com",
                              Recipient = "scores@dailyscores.mailgun.org",
                              Subject = "Unit Test",
                              Body = "This is the body of the email"
                          };

            var controller = new ScoresController();
            //controller.EmailSubmission(request);
        }
    }
}
