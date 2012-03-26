using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DailyScores.Models
{
    public class JumbleScore : Score
    {
        public int JumbleScoreId { get; set; }
        public int WordOneMultiplier { get; set; }
        public int WordTwoMultiplier { get; set; }
        public int WordThreeMultiplier { get; set; }
        public int WordFourMultiplier { get; set; }
        public int WordFiveMultiplier { get; set; }

        public override string ToString()
        {
            return string.Format("Jumble: {0} ({1}x, {2}x, {3}x, {4}x, {5}x) {6}",
                                 this.TotalScore, this.WordOneMultiplier, this.WordTwoMultiplier, this.WordThreeMultiplier, 
                                 this.WordFourMultiplier, this.WordFiveMultiplier, this.TimeInSeconds);
        }
    }
}