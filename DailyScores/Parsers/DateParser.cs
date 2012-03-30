using System;
using DailyScores.Framework;

namespace DailyScores.Parsers
{
    public class DateParser : BaseScoreParser<DateTime>
    {
        private readonly string _header = "DATE";
        public override string Header
        {
            get { return this._header; }
        }

        public override Response<DateTime> Parse(string input)
        {
            var response = new Response<DateTime>(true);

            var dateInput = this.ExtractInput(input);

            if (!string.IsNullOrEmpty(dateInput))
            {
                response = this.ParseDate(dateInput);
            }
            else
            {
                response.Value = DateTime.UtcNow.Date;
            }

            return response;
        }

        private Response<DateTime> ParseDate(string input)
        {
            var response = new Response<DateTime>(true);

            //Date: "Date: 3/26/2012";
            var textParts = input.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            try
            {
                var date = this.GetDate(textParts[1]);

                response.Value = date.HasValue ? date.Value : DateTime.UtcNow.Date;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessages.Add("Unable to parse the Date");
                response.Exceptions.Add(ex);
            }

            return response;
        }

        private DateTime? GetDate(string input)
        {
            DateTime tempDate;
            
            if (!DateTime.TryParse(input, out tempDate))
            {
                return null;
            }

            return tempDate;
        }
    }
}