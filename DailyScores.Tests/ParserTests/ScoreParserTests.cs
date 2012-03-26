using System;
using System.Text;
using DailyScores.Models;
using DailyScores.Models.Parsers;
using Xunit;

namespace DailyScores.Tests
{
    public class ScoreParserTests
    {
        [Fact]
        public void AbleToParse_SingleLineInput_ScorePresent()
        {
            string input = "Hidato: 26360 (2100, 10000, 1080, 2x) 2:00";
            var output = ScoreParser.Parse<HidatoScoreParser, HidatoScore>(input);

            var expectedValue = new HidatoScore
                                {
                                    TotalScore = 26360,
                                    TileScore = 2100,
                                    PerfectBonus = 10000,
                                    TimeBonus = 1080,
                                    AdvancedMultiplier = 2,
                                    TimeInSeconds = 120,
                                    Date = DateTime.Now.Date,
                                };

            Assert.True(output.IsSuccess);
            Assert.Empty(output.ErrorMessages);
            Assert.Empty(output.Exceptions);

            Assert.Equal(expectedValue.Date, DateTime.Now.Date);
        }

        [Fact]
        public void UnableToParse_SingleLineInput_ScoreNotPresent()
        {
            string input = "Hidato: 26360 (2100, 10000, 1080, 2x) 2:00";
            var output = ScoreParser.Parse<JumbleScoreParser, JumbleScore>(input);

            Assert.False(output.IsSuccess);
        }

        [Fact]
        public void AbleToParse_MultiLineInput_ScoreFirstLine()
        {
            var inputBuilder = new StringBuilder();
            inputBuilder.AppendLine("Hidato: 26360 (2100, 10000, 1080, 2x) 2:00");
            inputBuilder.AppendLine("Jumble: 1234 (5x, 4x, 3x, 2x, 1x) 2:34");
            var output = ScoreParser.Parse<HidatoScoreParser, HidatoScore>(inputBuilder.ToString());

            var expectedValue = new HidatoScore
                                {
                                    TotalScore = 26360,
                                    TileScore = 2100,
                                    PerfectBonus = 10000,
                                    TimeBonus = 1080,
                                    AdvancedMultiplier = 2,
                                    TimeInSeconds = 120,
                                    Date = DateTime.Now.Date,
                                };

            Assert.True(output.IsSuccess);
            Assert.Empty(output.ErrorMessages);
            Assert.Empty(output.Exceptions);

            Assert.Equal(expectedValue.Date, DateTime.Now.Date);
        }

        [Fact]
        public void AbleToParse_MultiLineInput_ScoreNotFirstLine()
        {
            var inputBuilder = new StringBuilder();
            inputBuilder.AppendLine("Hidato: 26360 (2100, 10000, 1080, 2x) 2:00");
            inputBuilder.AppendLine("Jumble: 1234 (5x, 4x, 3x, 2x, 1x) 2:34");
            var output = ScoreParser.Parse<HidatoScoreParser, HidatoScore>(inputBuilder.ToString());

            var expectedValue = new JumbleScore
                                {
                                    TotalScore = 1234,
                                    WordOneMultiplier = 5,
                                    WordTwoMultiplier = 4,
                                    WordThreeMultiplier = 3,
                                    WordFourMultiplier = 2,
                                    WordFiveMultiplier = 1,
                                    TimeInSeconds = 154,
                                    Date = DateTime.Now.Date,
                                };

            Assert.True(output.IsSuccess);
            Assert.Empty(output.ErrorMessages);
            Assert.Empty(output.Exceptions);

            Assert.Equal(expectedValue.Date, DateTime.Now.Date);
        }
    }
}