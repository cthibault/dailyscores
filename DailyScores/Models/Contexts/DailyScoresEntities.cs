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
        public DbSet<LogGroup> LogGroups { get; set; }
        public DbSet<LogEntry> LogEntries { get; set; }

        public DbSet<Player> Players { get; set; }
        public DbSet<EmailAddress> EmailAddresses { get; set; }

        public DbSet<HidatoScore> HidatoScores { get; set; }
        public DbSet<JumbleScore> JumbleScores { get; set; }

        public DbSet<Season> Seasons { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

        #region Seasons Queries
        public int? GetSeasonId(DateTime date)
        {
            int? seasonId = null;

            try
            {
                var season = this.Seasons.SingleOrDefault(s => s.StartDate <= date && s.EndDate >= date);

                if (season != null)
                {
                    seasonId = season.SeasonId;
                }
            }
            catch (Exception ex)
            {
                //TODO
            }

            return seasonId;
        }
        #endregion
    }
}