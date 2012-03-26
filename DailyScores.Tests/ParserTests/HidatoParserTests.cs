using System;
using System.Collections.Generic;
using System.Linq;
using DailyScores.Models;
using DailyScores.Models.Parsers;
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
            Assert.Contains("Input Text does not contain a Hidato score", output.ErrorMessages);
        }

        [Fact]
        public void UnableToParse_TextDoesNotBeginWithJumble()
        {
            string text = "This is just a normal sentence.";
            
            var parser = new HidatoScoreParser();
            var output = parser.Parse(text);

            Assert.False(output.IsSuccess);
            Assert.Contains("Input Text does not contain a Hidato score", output.ErrorMessages);
        }

        [Fact]
        public void AbleToParse_StrictJumbleScoreFormat()
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
                                    TimeInSeconds = 120
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
        }

        [Fact]
        public void AbleToParse_LooseJumbleScoreFormat_OnlySpaces()
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
                                    TimeInSeconds = 120
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
        }
    }
}