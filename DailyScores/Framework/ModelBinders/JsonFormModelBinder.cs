using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace DailyScores.Framework.ModelBinders
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