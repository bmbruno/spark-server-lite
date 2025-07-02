using Microsoft.AspNetCore.Mvc;
using SparkServerLite.Infrastructure;
using SparkServerLite.Interfaces;
using SparkServerLite.ViewModels;

namespace SparkServerLite.Controllers
{
    public class BaseController : Controller
    {
        protected IAppContent _content { get; set; }

        protected IAppSettings _settings { get; set; }

        protected Interfaces.ILogger _logger { get; set; }

        internal int Page { get; set; }

        internal int ItemsPerPage { get; set; }

        internal int SkipCount { get { return (this.Page - 1) * this.ItemsPerPage; } }

        internal string UserAgent { get { return Request.Headers["User-Agent"]; } }

        internal string Referer { get { return Request.Headers["Referer"]; } }

        internal void SetupPaging(int? page)
        {
            if (page.HasValue)
                this.Page = page.Value;
            else
                this.Page = 1;
        }

        /// <summary>
        /// Assigns site-wide variables on the BaseViewModel class.
        /// </summary>
        /// <param name="viewModel">Current view model that derives from BaseViewModel.</param>
        internal void Setup(BaseViewModel viewModel)
        {
            viewModel.AppContent = _content;
            viewModel.SiteURL = _settings.SiteURL;
        }

        public BaseController(IAppSettings settings, IAppContent content, Interfaces.ILogger logger)
        {
            _settings = settings;
            _content = content;
            _logger = logger;
        }
    }
}