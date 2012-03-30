using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DailyScores.Models;
using DailyScores.Parsers;
using Xunit;

namespace DailyScores.Tests
{
    public class JumbleParserTests
    {
        [Fact]
        public void UnableToParse_EmptyText()
        {
            string text = string.Empty;

            var parser = new JumbleScoreParser();
            var output = parser.Parse(text);

            Assert.False(output.IsSuccess);
            Assert.Contains("Unable to parse the Jumble score", output.ErrorMessages);
        }

        [Fact]
        public void UnableToParse_NoLineBeginsWithJumble()
        {
            string text = "This is just a normal sentence.";
            
            var parser = new JumbleScoreParser();
            var output = parser.Parse(text);

            Assert.False(output.IsSuccess);
            Assert.Contains("Unable to parse the Jumble score", output.ErrorMessages);
        }

        [Fact]
        public void AbleToParse_StrictJumbleScoreFormat()
        {
            string text = "Jumble: 1234 (5x, 4x, 3x, 2x, 1x) 2:34";

            var parser = new JumbleScoreParser();
            var output = parser.Parse(text);

            var expectedValue = new JumbleScore
                                {
                                    TotalScore = 1234,
                                    WordOneMultiplier = 5,
                                    WordTwoMultiplier = 4,
                                    WordThreeMultiplier = 3,
                                    WordFourMultiplier = 2,
                                    WordFiveMultiplier = 1,
                                    TimeInSeconds = 154,
                                    Date = DateTime.UtcNow.Date
                                };

            Assert.True(output.IsSuccess);
            Assert.Empty(output.ErrorMessages);
            Assert.Empty(output.Exceptions);

            Assert.Equal(expectedValue.TotalScore, output.Value.TotalScore);
            Assert.Equal(expectedValue.WordOneMultiplier, output.Value.WordOneMultiplier);
            Assert.Equal(expectedValue.WordTwoMultiplier, output.Value.WordTwoMultiplier);
            Assert.Equal(expectedValue.WordThreeMultiplier, output.Value.WordThreeMultiplier);
            Assert.Equal(expectedValue.WordFourMultiplier, output.Value.WordFourMultiplier);
            Assert.Equal(expectedValue.WordFiveMultiplier, output.Value.WordFiveMultiplier);
            Assert.Equal(expectedValue.TimeInSeconds, output.Value.TimeInSeconds);
            Assert.Equal(expectedValue.Date, output.Value.Date);
        }

        [Fact]
        public void AbleToParse_StrictJumbleScoreFormatWithDate()
        {
            string text = "Date: 1/14/1985";
            text += "\r\nJumble: 1234 (5x, 4x, 3x, 2x, 1x) 2:34";

            var parser = new JumbleScoreParser();
            var output = parser.Parse(text);

            var expectedValue = new JumbleScore
                                {
                                    TotalScore = 1234,
                                    WordOneMultiplier = 5,
                                    WordTwoMultiplier = 4,
                                    WordThreeMultiplier = 3,
                                    WordFourMultiplier = 2,
                                    WordFiveMultiplier = 1,
                                    TimeInSeconds = 154,
                                    Date = new DateTime(1985, 1, 14)
                                };

            Assert.True(output.IsSuccess);
            Assert.Empty(output.ErrorMessages);
            Assert.Empty(output.Exceptions);

            Assert.Equal(expectedValue.TotalScore, output.Value.TotalScore);
            Assert.Equal(expectedValue.WordOneMultiplier, output.Value.WordOneMultiplier);
            Assert.Equal(expectedValue.WordTwoMultiplier, output.Value.WordTwoMultiplier);
            Assert.Equal(expectedValue.WordThreeMultiplier, output.Value.WordThreeMultiplier);
            Assert.Equal(expectedValue.WordFourMultiplier, output.Value.WordFourMultiplier);
            Assert.Equal(expectedValue.WordFiveMultiplier, output.Value.WordFiveMultiplier);
            Assert.Equal(expectedValue.TimeInSeconds, output.Value.TimeInSeconds);
            Assert.Equal(expectedValue.Date, output.Value.Date);
        }

        [Fact]
        public void AbleToParse_LooseJumbleScoreFormat_OnlySpaces()
        {
            string text = "Jumble 1234 5x 4x 3x 2x 1x 2:34";

            var parser = new JumbleScoreParser();
            var output = parser.Parse(text);

            var expectedValue = new JumbleScore
                                {
                                    TotalScore = 1234,
                                    WordOneMultiplier = 5,
                                    WordTwoMultiplier = 4,
                                    WordThreeMultiplier = 3,
                                    WordFourMultiplier = 2,
                                    WordFiveMultiplier = 1,
                                    TimeInSeconds = 154,
                                    Date = DateTime.UtcNow.Date
                                };

            Assert.True(output.IsSuccess);
            Assert.Empty(output.ErrorMessages);
            Assert.Empty(output.Exceptions);

            Assert.Equal(expectedValue.TotalScore, output.Value.TotalScore);
            Assert.Equal(expectedValue.WordOneMultiplier, output.Value.WordOneMultiplier);
            Assert.Equal(expectedValue.WordTwoMultiplier, output.Value.WordTwoMultiplier);
            Assert.Equal(expectedValue.WordThreeMultiplier, output.Value.WordThreeMultiplier);
            Assert.Equal(expectedValue.WordFourMultiplier, output.Value.WordFourMultiplier);
            Assert.Equal(expectedValue.WordFiveMultiplier, output.Value.WordFiveMultiplier);
            Assert.Equal(expectedValue.TimeInSeconds, output.Value.TimeInSeconds);
            Assert.Equal(expectedValue.Date, output.Value.Date);
        }

        [Fact]
        public void AbleToParse_ParseFirstJumbleScore()
        {
            string text = "Jumble: 1234 (5x, 4x, 3x, 2x, 1x) 2:34";
            text += "\r\nJumble: 1 (2x, 3x, 4x, 5x, 6x) 6:00";

            var parser = new JumbleScoreParser();
            var output = parser.Parse(text);

            var expectedValue = new JumbleScore
                                {
                                    TotalScore = 1234,
                                    WordOneMultiplier = 5,
                                    WordTwoMultiplier = 4,
                                    WordThreeMultiplier = 3,
                                    WordFourMultiplier = 2,
                                    WordFiveMultiplier = 1,
                                    TimeInSeconds = 154,
                                    Date = DateTime.UtcNow.Date
                                };

            Assert.True(output.IsSuccess);
            Assert.Empty(output.ErrorMessages);
            Assert.Empty(output.Exceptions);

            Assert.Equal(expectedValue.TotalScore, output.Value.TotalScore);
            Assert.Equal(expectedValue.WordOneMultiplier, output.Value.WordOneMultiplier);
            Assert.Equal(expectedValue.WordTwoMultiplier, output.Value.WordTwoMultiplier);
            Assert.Equal(expectedValue.WordThreeMultiplier, output.Value.WordThreeMultiplier);
            Assert.Equal(expectedValue.WordFourMultiplier, output.Value.WordFourMultiplier);
            Assert.Equal(expectedValue.WordFiveMultiplier, output.Value.WordFiveMultiplier);
            Assert.Equal(expectedValue.TimeInSeconds, output.Value.TimeInSeconds);
            Assert.Equal(expectedValue.Date, output.Value.Date);
        }
    }
}