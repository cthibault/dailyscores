using System;
using System.Web;

namespace DailyScores.Framework
{
    public class TimeZoneManager
    {
        private const string USER_TIMEZONE_CONTEXT_KEY = "USER_TIMEZONE";

        public static void UpdateTimeZone(HttpRequest request)
        {
            var cookie = request.Cookies["TimeZoneOffset"];
            if (cookie != null)
            {
                var offset = new TimeSpan(0, int.Parse(cookie.Value), 0);
                UserTimeZone = TimeZoneInfo.CreateCustomTimeZone("Browser", offset, "Browser", "Browser");
            }
        }

        public static DateTime GetUserTime(DateTime value)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(value, UserTimeZone);
        }

        public static DateTime ToUtcTime(DateTime value)
        {
            return TimeZoneInfo.ConvertTimeToUtc(value, UserTimeZone);
        }

        public static TimeZoneInfo UserTimeZone
        {
            get
            {
                var contextValue = HttpContext.Current.Items[USER_TIMEZONE_CONTEXT_KEY];
                return (contextValue == null) ? TimeZoneInfo.Local : (TimeZoneInfo) contextValue;
            }
            set { HttpContext.Current.Items[USER_TIMEZONE_CONTEXT_KEY] = value; }
        }
    }
}