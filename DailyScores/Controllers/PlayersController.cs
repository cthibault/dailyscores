using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DailyScores.Framework.Controllers;
using DailyScores.Models;

namespace DailyScores.Controllers
{ 
    public class PlayersController : BaseController
    {
        //
        // GET: /Player/
        public ViewResult Index()
        {
            return View(this.Repository.Players.ToList());
        }

        //
        // GET: /Player/Details/5
        public ViewResult Details(int id)
        {
            Player player = this.Repository.Players.Find(id);

            var currentSeasonId = this.Repository.GetSeasonId(DateTime.UtcNow.Date);

            ViewBag.HidatoScores = this.GetHidatoScores(id, currentSeasonId, 10);
            ViewBag.JumbleScores = this.GetJumbleScores(id, currentSeasonId, 10);
            ViewBag.MissingScores = this.GetMissingScores(id, currentSeasonId, 20);
            ViewBag.EmailSubmissions = this.GetEmailSubmissions(player, 10);
            ViewBag.Statistics = this.GetStatistics(id, currentSeasonId);
            
            return View(player);
        }

        #region Get Scores

        private List<HidatoScore> GetHidatoScores(int id, int? seasonId, int count)
        {
            return this.Repository.HidatoScores
                .Where(s => s.PlayerId == id && (seasonId == null || s.SeasonId == seasonId))
                .OrderByDescending(s => s.Date)
                .Take(count)
                .ToList();
        }

        private List<JumbleScore> GetJumbleScores(int id, int? seasonId, int count)
        {
            return this.Repository.JumbleScores
                .Where(s => s.PlayerId == id && (seasonId == null || s.SeasonId == seasonId))
                .OrderByDescending(s => s.Date)
                .Take(count)
                .ToList();
        }

        #endregion Get Scores

        #region Get Missing Scores

        private List<MissingScore> GetMissingScores(int id, int? seasonId, int count)
        {
            var missingScoresQuery = this.Repository.HidatoScores
                .Where(hs => seasonId == null || hs.SeasonId == seasonId)
                .GroupBy(hs => hs.Date)
                .Where(g => g.All(s => s.PlayerId != id))
                .Select(hs => new MissingScore
                              {
                                  Date = hs.Key,
                                  NumberOfPlayers = hs.Count(),
                                  GameType = "Hidato"
                              });

            missingScoresQuery = missingScoresQuery.Union(
                this.Repository.JumbleScores
                    .Where(js => seasonId == null || js.SeasonId == seasonId)
                    .GroupBy(js => js.Date)
                    .Where(g => g.All(s => s.PlayerId != id))
                    .Select(js => new MissingScore
                                  {
                                      Date = js.Key,
                                      NumberOfPlayers = js.Count(),
                                      GameType = "Jumble"
                                  })
                );

            return missingScoresQuery
                .OrderByDescending(ms => ms.Date)
                .Take(count)
                .ToList();
        }

        #endregion Get Missing Scores

        #region Get Email Submissions

        private List<EmailSubmission> GetEmailSubmissions(Player player, int count)
        {
            return this.Repository.EmailSubmissions.ToList()
                .Where(s => player.EmailAddresses.Any(ea => s.From == ea.Address))
                .OrderByDescending(s => s.EmailSubmissionId)
                .Take(count)
                .ToList();
        }

        #endregion Get Email Submissions

        #region Get Statistics

        private List<Statistics> GetStatistics(int id, int? seasonId)
        {
            var list = new List<Statistics>
                       {
                           this.GetHidatoStatistics(id, seasonId), 
                           this.GetJumbleStatistics(id, seasonId)
                       };

            return list;
        }

        private Statistics GetHidatoStatistics(int id, int? seasonId)
        {
            var hidatoScoresQuery = this.Repository.HidatoScores.Where(s => seasonId == null || s.SeasonId == seasonId);

            var groupedHidatoScores = from score in hidatoScoresQuery
                                      group score by score.Date
                                      into scoreGroup
                                      let list = scoreGroup.OrderByDescending(s => s.TotalScore).ThenBy(s => s.TimeInSeconds)
                                      let best = scoreGroup.OrderByDescending(s => s.TotalScore).ThenBy(s => s.TimeInSeconds).FirstOrDefault()
                                      let tied = scoreGroup.Where(s => s.TotalScore == best.TotalScore && s.TimeInSeconds == best.TimeInSeconds)
                                      select new
                                             {
                                                 Date = scoreGroup.Key, 
                                                 Results = list, 
                                                 Best = best,
                                                 IsTie = tied.Count() > 1 && tied.Any(s => s.PlayerId == id)
                                             };

            var statistics = new Statistics
                             {
                                 Title = "Hidato",
                                 GamesPlayed = groupedHidatoScores.SelectMany(g => g.Results).Count(s => s.PlayerId == id),
                                 Wins = groupedHidatoScores.Count(g => !g.IsTie && g.Best.PlayerId == id),
                                 Losses = groupedHidatoScores.Count(g => !g.IsTie && g.Best.PlayerId != id),
                                 Ties = groupedHidatoScores.Count(g => g.IsTie),
                                 HighScore = groupedHidatoScores.SelectMany(g => g.Results).Where(s => s.PlayerId == id).Max(s => s.TotalScore),
                                 LowScore = groupedHidatoScores.SelectMany(g => g.Results).Where(s => s.PlayerId == id).Min(s => s.TotalScore),
                                 AverageScore = Math.Round(groupedHidatoScores.SelectMany(g => g.Results).Where(s => s.PlayerId == id).Average(s => s.TotalScore), 2)
                             };

            return statistics;
        }

        private Statistics GetJumbleStatistics(int id, int? seasonId)
        {
            var jumbleScoresQuery = this.Repository.JumbleScores.Where(s => seasonId == null || s.SeasonId == seasonId);

            var groupedJumbleScores = from score in jumbleScoresQuery
                                      group score by score.Date
                                      into scoreGroup
                                      let list = scoreGroup.OrderByDescending(s => s.TotalScore).ThenBy(s => s.TimeInSeconds)
                                      let best = scoreGroup.OrderByDescending(s => s.TotalScore).ThenBy(s => s.TimeInSeconds).FirstOrDefault()
                                      let tied = scoreGroup.Where(s => s.TotalScore == best.TotalScore && s.TimeInSeconds == best.TimeInSeconds)
                                      select new
                                             {
                                                 Date = scoreGroup.Key, 
                                                 Results = list, 
                                                 Best = best,
                                                 IsTie = tied.Count() > 1 && tied.Any(s => s.PlayerId == id)
                                             };

            var statistics = new Statistics
                             {
                                 Title = "Jumble",
                                 GamesPlayed = groupedJumbleScores.SelectMany(g => g.Results).Count(s => s.PlayerId == id),
                                 Wins = groupedJumbleScores.Count(g => !g.IsTie && g.Best.PlayerId == id),
                                 Losses = groupedJumbleScores.Count(g => !g.IsTie && g.Best.PlayerId != id),
                                 Ties = groupedJumbleScores.Count(g => g.IsTie),
                                 HighScore = groupedJumbleScores.SelectMany(g => g.Results).Where(s => s.PlayerId == id).Max(s => s.TotalScore),
                                 LowScore = groupedJumbleScores.SelectMany(g => g.Results).Where(s => s.PlayerId == id).Min(s => s.TotalScore),
                                 AverageScore = Math.Round(groupedJumbleScores.SelectMany(g => g.Results).Where(s => s.PlayerId == id).Average(s => s.TotalScore), 2)
                             };

            return statistics;
        }

        #endregion Get Statistics
    }
}