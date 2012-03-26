using System;

namespace DailyScores.Models
{
    public abstract class BaseEntity
    {
        protected BaseEntity()
        {
            ModifiedDtm = DateTime.Now;
        }

        public DateTime ModifiedDtm { get; set; }        
    }
}