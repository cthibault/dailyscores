using System;
using System.Collections.Generic;
using System.Linq;
using DailyScores.Framework;
using DailyScores.Models;

namespace DailyScores.Parsers
{
    public class HidatoScoreParser : BaseScoreParser<HidatoScore>
    {
        private readonly string _header = "HIDATO";
        public override string Header
        {
            get { return this._header; }
        }

        public override Response<HidatoScore> Parse(string input)
        {
            var response = new Response<HidatoScore>(false);

            var dateResponse = this.ParseDate(input);
            var hidatoInput = this.ExtractInput(input);

            if (dateResponse.IsSuccess && !string.IsNullOrEmpty(hidatoInput))
            {
                response = this.ParseScore(hidatoInput, dateResponse.Value);
            }
            else
            {
                response.ErrorMessages.Add("Unable to parse the Hidato score");
            }

            return response;
        }

        private Response<DateTime> ParseDate(string input)
        {
            var parser = new DateParser();
            return parser.Parse(input);
        }

        private Response<HidatoScore> ParseScore(string input, DateTime date)
        {
            var response = new Response<HidatoScore>(false);

            //Hidato: 26360 (2100, 10000, 1080, 2x) 2:00
            var textParts = input.Split(new string[] { " ", "(", ",", ")" }, StringSplitOptions.RemoveEmptyEntries);

            try
            {
                var values = new List<int?>();
                values.Add(this.GetInteger(textParts[1]));
                values.Add(this.GetInteger(textParts[2]));
                values.Add(this.GetInteger(textParts[3]));
                values.Add(this.GetInteger(textParts[4]));
                values.Add(this.GetInteger(textParts[5].Replace("x", "")));
                values.Add(this.GetTimeInSeconds(textParts[6]));

                if (values.All(x => x.HasValue))
                {
                    var score = new HidatoScore()
                                {
                                    TotalScore = values[0].Value,
                                    TileScore = values[1].Value,
                                    PerfectBonus = values[2].Value,
                                    TimeBonus = values[3].Value,
                                    AdvancedMultiplier = values[4].Value,
                                    TimeInSeconds = values[5].Value,
                                    Date = date
                                };

                    response.Value = score;
                    response.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessages.Add("Unable to parse the Hidato Score");
                response.Exceptions.Add(ex);
            }

            return response;
        }
    }
}