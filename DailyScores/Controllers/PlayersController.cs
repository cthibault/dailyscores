using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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

            ViewBag.HidatoScores = this.Repository.HidatoScores
                .Where(s => s.PlayerId == id)
                .OrderByDescending(s => s.HidatoId)
                .Take(10)
                .ToList();

            ViewBag.JumbleScores = this.Repository.JumbleScores
                .Where(s => s.PlayerId == id)
                .OrderByDescending(s => s.JumbleId)
                .Take(10)
                .ToList();

            var missingScoresQuery = this.Repository.HidatoScores
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
                    .GroupBy(js => js.Date)
                    .Where(g => g.All(s => s.PlayerId != id))
                    .Select(js => new MissingScore
                                  {
                                      Date = js.Key,
                                      NumberOfPlayers = js.Count(),
                                      GameType = "Jumble"
                                  })
                );

            ViewBag.MissingScores = missingScoresQuery
                .OrderByDescending(ms => ms.Date)
                .Take(20);
            
            ViewBag.EmailSubmissions = this.Repository.EmailSubmissions.ToList()
                .Where(s => player.EmailAddresses.Any(ea => s.From == ea.Address))
                .OrderByDescending(s => s.EmailSubmissionId)
                .Take(10)
                .ToList();

            return View(player);
        }
    }
}