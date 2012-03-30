using System;

namespace DailyScores.Models
{
    public abstract class BaseEntity
    {
        protected BaseEntity()
        {
            ModifiedDtm = DateTime.UtcNow;
        }

        public DateTime ModifiedDtm { get; set; }        
    }
}