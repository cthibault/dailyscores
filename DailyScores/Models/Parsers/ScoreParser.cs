using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DailyScores.Models.Parsers
{
    public static class ScoreParser
    {
        public static Response<TScore> Parse<TParser, TScore>(string input) 
            where TParser : BaseScoreParser<TScore>, new()
            where TScore : Score, new()
        {
            var parser = new TParser();
            var response = new Response<TScore>(false);

            var extractedInput = ExtractInput<TParser, TScore>(parser, input).ToList();

            if (!extractedInput.Any())
            {
                response.ErrorMessages.Add("No scores to parse");
            }
            else if (extractedInput.Count > 1)
            {
                response.ErrorMessages.Add("Too many scores present to parse");
            }
            else
            {
                response = parser.Parse(extractedInput.First());
            }

            return response;
        }

        private static IEnumerable<string> ExtractInput<TParser, TScore>(TParser parser, string input)
            where TParser : BaseScoreParser<TScore>
            where TScore : Score
        {
            var splitResults = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            return splitResults.Where(parser.ContainsHeader);
        }
    }
}