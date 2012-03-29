using System;
using System.Linq;
using DailyScores.Framework;

namespace DailyScores.Parsers
{
    public abstract class BaseScoreParser<T>
    {
        public abstract string Header { get; }

        public abstract Response<T> Parse(string text);

        public virtual bool ContainsHeader(string input)
        {
            return input.ToUpper().StartsWith(this.Header);
        }

        protected virtual string ExtractInput(string input)
        {
            var split = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var results = split.Where(this.ContainsHeader).ToList();

            return results.Any() ? results.First() : string.Empty;
        }

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