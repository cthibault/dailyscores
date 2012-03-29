using System;
using System.Collections.Generic;
using System.Linq;

namespace DailyScores.Models.Parsers
{
    public class JumbleScoreParser : BaseScoreParser<JumbleScore>
    {
        private readonly string _header = "JUMBLE";
        public override string Header
        {
            get { return this._header; }
        }

        public override Response<JumbleScore> Parse(string input)
        {
            var response = new Response<JumbleScore>(false);

            var dateResponse = this.ParseDate(input);
            var jumbleInput = this.ExtractInput(input);

            if (dateResponse.IsSuccess && !string.IsNullOrEmpty(jumbleInput))
            {
                response = this.ParseScore(jumbleInput, dateResponse.Value);
            }
            else
            {
                response.ErrorMessages.Add("Unable to parse the Jumble score");                
            }

            return response;
        }

        private Response<DateTime> ParseDate(string input)
        {
            var parser = new DateParser();
            return parser.Parse(input);
        }

        private Response<JumbleScore> ParseScore(string input, DateTime date)
        {
            var response = new Response<JumbleScore>(false);


            //Jumble: 1234 (5x, 4x, 3x, 2x, 1x) 2:34
            var textParts = input.Split(new string[] { " ", "(", ",", ")" }, StringSplitOptions.RemoveEmptyEntries);

            try
            {
                var values = new List<int?>();
                values.Add(this.GetInteger(textParts[1].Replace(",", "")));
                values.Add(this.GetInteger(textParts[2].Replace("x", "")));
                values.Add(this.GetInteger(textParts[3].Replace("x", "")));
                values.Add(this.GetInteger(textParts[4].Replace("x", "")));
                values.Add(this.GetInteger(textParts[5].Replace("x", "")));
                values.Add(this.GetInteger(textParts[6].Replace("x", "")));
                values.Add(this.GetTimeInSeconds(textParts[7]));

                if (values.All(x => x.HasValue))
                {
                    var score = new JumbleScore
                                {
                                    TotalScore = values[0].Value,
                                    WordOneMultiplier = values[1].Value,
                                    WordTwoMultiplier = values[2].Value,
                                    WordThreeMultiplier = values[3].Value,
                                    WordFourMultiplier = values[4].Value,
                                    WordFiveMultiplier = values[5].Value,
                                    TimeInSeconds = values[6].Value,
                                    Date = date
                                };

                    response.Value = score;
                    response.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessages.Add("Unable to parse the Jumble Score");
                response.Exceptions.Add(ex);
            }

            return response;
        }
    }
}