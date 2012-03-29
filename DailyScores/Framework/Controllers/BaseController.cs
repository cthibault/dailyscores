using System.Web.Mvc;
using DailyScores.Models;

namespace DailyScores.Framework.Controllers
{
    public class BaseController : Controller
    {
        #region IDisposable Members

        protected override void Dispose(bool disposing)
        {
            if (this._repository != null)
            {
                this._repository.Dispose();
            }

            base.Dispose(disposing);
        }

        #endregion IDisposable Members

        #region Protected Properties

        protected DailyScoresEntities Repository
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

        #endregion Protected Properties

        #region Private Fields

        private DailyScoresEntities _repository;

        #endregion Private Fields
    }
}