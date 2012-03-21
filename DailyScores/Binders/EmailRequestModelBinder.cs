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
            
            request.Sender = bindingContext.ValueProvider.GetValue("sender").ToString();
            request.Subject = bindingContext.ValueProvider.GetValue("subject").ToString();
            request.Body = bindingContext.ValueProvider.GetValue("body-plain").ToString();

            return request;
        }
    }
}