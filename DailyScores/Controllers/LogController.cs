using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DailyScores.Models;

namespace DailyScores.Controllers
{
    public class LogController : Controller
    {
        //
        // GET: /Log/

        public ActionResult Index()
        {
            var logGroups = new List<LogGroup>();

            try
            {
                logGroups = this.Repository.LogGroups.Include("LogEntries").OrderByDescending(g => g.GroupId).Take(10).ToList();
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return View(logGroups);
        }


        #region Private Properties

        private DailyScoresEntities _repository;
        private DailyScoresEntities Repository
        {
            get
            {
                if (this._repository == null)
                {
                    this._repository = new DailyScoresEntities();
                }

                return this._repository;
            }
        }

        #endregion Private Properties
    }
}
