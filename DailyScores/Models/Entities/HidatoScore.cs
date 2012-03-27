using System.ComponentModel.DataAnnotations;

namespace DailyScores.Models
{
    public class HidatoScore : Score
    {
        [Key]
        public int HidatoId { get; set; }
        public int TileScore { get; set; }
        public int PerfectBonus { get; set; }
        public int TimeBonus { get; set; }
        public int AdvancedMultiplier { get; set; }

        public override string ToString()
        {
            return string.Format("Hidato: {0} ({1}, {2}, {3}, {4}x) {5}",
                                 this.TotalScore, this.TileScore, this.PerfectBonus, this.TimeBonus, this.AdvancedMultiplier, this.TimeInSeconds);
        }
    }
}