using Microsoft.AspNetCore.Mvc;
using SparkServerLite.Interfaces;
using SparkServerLite.ViewModels;

namespace SparkServerLite.Controllers
{
    public class BaseController : Controller
    {
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

        internal void Setup(BaseViewModel viewModel, IAppSettings appSettings)
        {
            viewModel.Sitename = appSettings.Sitename;
            viewModel.SiteDescription = appSettings.SiteDescription;
            viewModel.SiteURL = appSettings.SiteURL;
        }
    }
}