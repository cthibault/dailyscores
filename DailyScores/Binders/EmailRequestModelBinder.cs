using System;
using System.Web.Mvc;
using DailyScores.Models;
using DailyScores.Models.Requests;

namespace DailyScores.Binders
{
    public class EmailRequestModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var recipient = bindingContext.ValueProvider.GetValue("recipient");
            var sender = bindingContext.ValueProvider.GetValue("sender");
            var subject = bindingContext.ValueProvider.GetValue("subject");
            var body = bindingContext.ValueProvider.GetValue("body-plain");

            var request = new EmailRequest
                          {
                              Recipient = recipient != null ? recipient.AttemptedValue : string.Empty,
                              Sender = sender != null ? sender.AttemptedValue : string.Empty,
                              Subject = subject != null ? subject.AttemptedValue : string.Empty,
                              Body = body != null ? body.AttemptedValue : string.Empty,
                          };

            return request;
        }
    }
}