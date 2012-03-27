using System;

namespace DailyScores.Models
{
    public abstract class BaseEntity
    {
        protected BaseEntity()
        {
            ModifiedDtm = DateTime.Now.Date;
        }

        public DateTime ModifiedDtm { get; set; }        
    }
}