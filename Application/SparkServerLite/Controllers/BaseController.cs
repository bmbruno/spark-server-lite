using Microsoft.AspNetCore.Mvc;
using SparkServerLite.Infrastructure;
using SparkServerLite.Interfaces;
using SparkServerLite.ViewModels;

namespace SparkServerLite.Controllers
{
    public class BaseController : Controller
    {
        private IAppContent _appContent { get; set; }

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
        /// <param name="appSettings">IAppSettings instance.</param>
        internal void Setup(BaseViewModel viewModel, IAppSettings appSettings)
        {
            viewModel.Sitename = _appContent.SiteName;
            viewModel.SiteDescription = _appContent.SiteDescription;
            viewModel.SiteURL = appSettings.SiteURL;
        }

        public BaseController(IAppContent content)
        {
            _appContent = content;
        }
    }
}