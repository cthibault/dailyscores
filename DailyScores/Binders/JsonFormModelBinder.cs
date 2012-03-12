using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace DailyScores.Binders
{
    public class JsonFormModelBinder : IModelBinder
    {
        #region Implementation of IModelBinder

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpRequestBase request = controllerContext.HttpContext.Request;
            var jsonStringData = request.Form[bindingContext.ModelName];

            return (jsonStringData != null)
                       ? JsonConvert.DeserializeObject(jsonStringData, bindingContext.ModelType)
                       : null;
        }

        #endregion
    }
}