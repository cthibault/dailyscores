using System;
using System.Web.Mvc;

namespace DailyScores.Framework.ModelBinders
{
    public class DateTimeModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (value != null && value.AttemptedValue != null && !string.IsNullOrEmpty(value.AttemptedValue))
            {
                return TimeZoneManager.ToUtcTime(((DateTime?) value.ConvertTo(typeof (DateTime))).Value);
            }

            return null;
        }
    }
}