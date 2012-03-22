using System;
using System.Web.Mvc;
using DailyScores.Models;

namespace DailyScores.Binders
{
    public class EmailRequestModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var request = new EmailRequest();
            
            request.Recipient = bindingContext.ValueProvider.GetValue("recipient").AttemptedValue;
            request.Sender = bindingContext.ValueProvider.GetValue("sender").AttemptedValue;
            request.Subject = bindingContext.ValueProvider.GetValue("subject").AttemptedValue;
            request.Body = bindingContext.ValueProvider.GetValue("body-plain").AttemptedValue;

            return request;
        }
    }
}