using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DailyScores.Framework.DataProviders
{
    public class DateTimeMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        public override ModelMetadata GetMetadataForProperty(Func<object> modelAccessor, Type containerType, string propertyName)
        {
            var propertyDescriptor = this.GetTypeDescriptor(containerType).GetProperties().Find(propertyName, true);

            //DateTime: For DateTime model accessors, perform the conversion to the users time zone
            if (propertyDescriptor.PropertyType == typeof(DateTime))
            {
                var value = DateTime.MinValue;
                var rawValue = modelAccessor.Invoke();
                if (rawValue != null)
                {
                    value = TimeZoneManager.GetUserTime((DateTime) rawValue);
                }

                return this.GetMetadataForProperty(() => value, containerType, propertyDescriptor);
            }

            //DateTime?: For DateTime? model accessors, perform the conversion to the users time zone
            if (propertyDescriptor.PropertyType == typeof(DateTime?))
            {
                return this.GetMetadataForProperty(() =>
                                                   {
                                                       var dt = (DateTime?) modelAccessor.Invoke();
                                                       return !dt.HasValue ? dt : TimeZoneManager.GetUserTime(dt.Value);
                                                   }, containerType, propertyDescriptor);
            }

            //Everything else
            return this.GetMetadataForProperty(modelAccessor, containerType, propertyDescriptor);
        }
    }
}