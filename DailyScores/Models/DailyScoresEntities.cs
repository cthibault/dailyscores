using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace DailyScores.Models
{
    public class DailyScoresEntities : DbContext
    {
        public DbSet<EmailSubmission> EmailSubmissions { get; set; }

        public DbSet<Player> Players { get; set; }
        public DbSet<EmailAddress> EmailAddresses { get; set; }

        public DbSet<HidatoScore> HidatoScores { get; set; }
        public DbSet<JumbleScore> JumbleScores { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}