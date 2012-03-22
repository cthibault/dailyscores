using System;
using System.Linq;
using System.Web;

namespace DailyScores.Models.Parsers
{
    public abstract class ScoreParser<T> where T : Score, new()
    {
        public abstract Response<T> Parse(string text);

        protected virtual int? GetInteger(string input)
        {
            int value = 0;

            if (int.TryParse(input, out value))
            {
                return value;
            }

            return null;
        }

        protected virtual int? GetTimeInSeconds(string input)
        {
            var timeparts = input.Split(':').Select(this.GetInteger).ToList();

            while (timeparts.Count < 4)
            {
                timeparts.Insert(0, 0);
            }

            if (timeparts.All(x => x.HasValue))
            {
                var timespan = new TimeSpan(timeparts[0].Value, timeparts[1].Value, timeparts[2].Value, timeparts[3].Value);
                return (int) timespan.TotalSeconds;
            }

            return null;
        }
    }
}