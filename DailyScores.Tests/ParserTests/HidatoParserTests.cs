using System;
using System.Collections.Generic;
using System.Linq;
using DailyScores.Models;
using DailyScores.Parsers;
using Xunit;

namespace DailyScores.Tests
{
    public class HidatoParserTests
    {
        [Fact]
        public void UnableToParse_EmptyText()
        {
            string text = string.Empty;

            var parser = new HidatoScoreParser();
            var output = parser.Parse(text);

            Assert.False(output.IsSuccess);
            Assert.Contains("Unable to parse the Hidato score", output.ErrorMessages);
        }

        [Fact]
        public void UnableToParse_NoLineBeginsWithHidato()
        {
            string text = "This is just a normal sentence.";
            
            var parser = new HidatoScoreParser();
            var output = parser.Parse(text);

            Assert.False(output.IsSuccess);
            Assert.Contains("Unable to parse the Hidato score", output.ErrorMessages);
        }

        [Fact]
        public void AbleToParse_StrictHidatoScoreFormat()
        {
            string text = "Hidato: 26360 (2100, 10000, 1080, 2x) 2:00";

            var parser = new HidatoScoreParser();
            var output = parser.Parse(text);

            var expectedValue = new HidatoScore
                                {
                                    TotalScore = 26360,
                                    TileScore = 2100,
                                    PerfectBonus = 10000,
                                    TimeBonus = 1080,
                                    AdvancedMultiplier = 2,
                                    TimeInSeconds = 120,
                                    Date = DateTime.Now.Date
                                };

            Assert.True(output.IsSuccess);
            Assert.Empty(output.ErrorMessages);
            Assert.Empty(output.Exceptions);

            Assert.Equal(expectedValue.TotalScore, output.Value.TotalScore);
            Assert.Equal(expectedValue.TileScore, output.Value.TileScore);
            Assert.Equal(expectedValue.PerfectBonus, output.Value.PerfectBonus);
            Assert.Equal(expectedValue.TimeBonus, output.Value.TimeBonus);
            Assert.Equal(expectedValue.AdvancedMultiplier, output.Value.AdvancedMultiplier);
            Assert.Equal(expectedValue.TimeInSeconds, output.Value.TimeInSeconds);
            Assert.Equal(expectedValue.Date, output.Value.Date);
        }

        [Fact]
        public void AbleToParse_StrictHidatoScoreFormatWithDate()
        {
            string text = "Date: 1/14/1985";
            text += "\r\nHidato: 26360 (2100, 10000, 1080, 2x) 2:00";

            var parser = new HidatoScoreParser();
            var output = parser.Parse(text);

            var expectedValue = new HidatoScore
                                {
                                    TotalScore = 26360,
                                    TileScore = 2100,
                                    PerfectBonus = 10000,
                                    TimeBonus = 1080,
                                    AdvancedMultiplier = 2,
                                    TimeInSeconds = 120,
                                    Date = new DateTime(1985, 1, 14)
                                };

            Assert.True(output.IsSuccess);
            Assert.Empty(output.ErrorMessages);
            Assert.Empty(output.Exceptions);

            Assert.Equal(expectedValue.TotalScore, output.Value.TotalScore);
            Assert.Equal(expectedValue.TileScore, output.Value.TileScore);
            Assert.Equal(expectedValue.PerfectBonus, output.Value.PerfectBonus);
            Assert.Equal(expectedValue.TimeBonus, output.Value.TimeBonus);
            Assert.Equal(expectedValue.AdvancedMultiplier, output.Value.AdvancedMultiplier);
            Assert.Equal(expectedValue.TimeInSeconds, output.Value.TimeInSeconds);
            Assert.Equal(expectedValue.Date, output.Value.Date);
        }

        [Fact]
        public void AbleToParse_LooseHidatoScoreFormat_OnlySpaces()
        {
            string text = "Hidato: 26360 2100 10000 1080 2x 2:00";

            var parser = new HidatoScoreParser();
            var output = parser.Parse(text);

            var expectedValue = new HidatoScore
                                {
                                    TotalScore = 26360,
                                    TileScore = 2100,
                                    PerfectBonus = 10000,
                                    TimeBonus = 1080,
                                    AdvancedMultiplier = 2,
                                    TimeInSeconds = 120,
                                    Date = DateTime.Now.Date
                                };

            Assert.True(output.IsSuccess);
            Assert.Empty(output.ErrorMessages);
            Assert.Empty(output.Exceptions);

            Assert.Equal(expectedValue.TotalScore, output.Value.TotalScore);
            Assert.Equal(expectedValue.TileScore, output.Value.TileScore);
            Assert.Equal(expectedValue.PerfectBonus, output.Value.PerfectBonus);
            Assert.Equal(expectedValue.TimeBonus, output.Value.TimeBonus);
            Assert.Equal(expectedValue.AdvancedMultiplier, output.Value.AdvancedMultiplier);
            Assert.Equal(expectedValue.TimeInSeconds, output.Value.TimeInSeconds);
            Assert.Equal(expectedValue.Date, output.Value.Date);
        }

        [Fact]
        public void AbleToParse_ParseFirstHidatoScore()
        {
            string text = "Hidato: 26360 (2100, 10000, 1080, 2x) 2:00";
            text += "\r\nHidato: 1 (2, 3, 4, 5x) 6:00";

            var parser = new HidatoScoreParser();
            var output = parser.Parse(text);

            var expectedValue = new HidatoScore
                                {
                                    TotalScore = 26360,
                                    TileScore = 2100,
                                    PerfectBonus = 10000,
                                    TimeBonus = 1080,
                                    AdvancedMultiplier = 2,
                                    TimeInSeconds = 120,
                                    Date = DateTime.Now.Date
                                };

            Assert.True(output.IsSuccess);
            Assert.Empty(output.ErrorMessages);
            Assert.Empty(output.Exceptions);

            Assert.Equal(expectedValue.TotalScore, output.Value.TotalScore);
            Assert.Equal(expectedValue.TileScore, output.Value.TileScore);
            Assert.Equal(expectedValue.PerfectBonus, output.Value.PerfectBonus);
            Assert.Equal(expectedValue.TimeBonus, output.Value.TimeBonus);
            Assert.Equal(expectedValue.AdvancedMultiplier, output.Value.AdvancedMultiplier);
            Assert.Equal(expectedValue.TimeInSeconds, output.Value.TimeInSeconds);
            Assert.Equal(expectedValue.Date, output.Value.Date);
        }
    }
}