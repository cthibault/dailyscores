using System;
using System.Collections.Generic;
using System.Linq;

namespace DailyScores.Models.Parsers
{
    public class HidatoScoreParser : ScoreParser<HidatoScore>
    {
        public override Response<HidatoScore> Parse(string text)
        {
            var response = new Response<HidatoScore>(false);

            if (text.ToUpper().StartsWith("HIDATO"))
            {
                //Hidato: 26360 (2100, 10000, 1080, 2x) 2:00
                var textParts = text.Split(new string[] { " ", "(", ",", ")" }, StringSplitOptions.RemoveEmptyEntries);

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
                                        TimeInSeconds = values[5].Value
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
            }
            else
            {
                response.ErrorMessages.Add("Input Text does not contain a Hidato score");                
            }

            return response;
        }
    }
}