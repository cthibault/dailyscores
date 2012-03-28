using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DailyScores.Models
{
    public class LogEntry : BaseEntity
    {
        [Key]
        public int EntryId { get; set; }
        public int GroupId { get; set; }
        public string Description { get; set; }
    }

    public class LogGroup : BaseEntity
    {
        [Key]
        public int GroupId { get; set; }
        public string Description { get; set; }

        public List<LogEntry> LogEntries { get; set; }
    }
}