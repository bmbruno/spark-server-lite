using Microsoft.AspNetCore.Mvc;
using SparkServerLite.Infrastructure;
using SparkServerLite.Interfaces;
using SparkServerLite.ViewModels;

namespace SparkServerLite.Controllers
{
    public class BaseController : Controller
    {
        private IAppContent _content { get; set; }

        private IAppSettings _settings { get; set; }

        internal int Page { get; set; }

        internal int ItemsPerPage { get; set; }

        internal int SkipCount { get { return (this.Page - 1) * this.ItemsPerPage; } }

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
            viewModel.Sitename = _content.SiteName;
            viewModel.SiteDescription = _content.SiteDescription;
            viewModel.SiteURL = _settings.SiteURL;
        }

        public BaseController(IAppSettings settings, IAppContent content)
        {
            _settings = settings;
            _content = content;
        }
    }
}