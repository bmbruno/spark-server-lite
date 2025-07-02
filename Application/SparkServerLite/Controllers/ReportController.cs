using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SparkServerLite.Infrastructure;
using SparkServerLite.Interfaces;
using SparkServerLite.Models;
using SparkServerLite.ViewModels;
using SparkServerLite.ViewModels.Analytics;


namespace SparkServerLite.Controllers
{
    [Authorize]
    public class ReportController : BaseController
    {
        private readonly IWebHostEnvironment _host;
        private Analytics _analytics;
        
        public ReportController(IAnalyticsRepository<Visit> analyticsRepo, Interfaces.ILogger logger, IWebHostEnvironment host, IAppSettings settings, IAppContent content) : base(settings, content, logger)
        {
            _host = host;
            _analytics = new Analytics(_settings, analyticsRepo, _logger);
        }

        public IActionResult Index()
        {
            BaseViewModel viewModel = new();
            base.Setup(viewModel);
            ViewData["Title"] = "Analytics";

            return View(viewModel);
        }

        public IActionResult Pageviews()
        {
            ReportPageViewsViewModel viewModel = new();
            base.Setup(viewModel);
            ViewData["Title"] = "Report: Page Views";

            // TODO: get report data from analytics repo

            return View(viewModel);

        }

    }
}