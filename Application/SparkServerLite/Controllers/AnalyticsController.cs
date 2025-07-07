using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SparkServerLite.Infrastructure;
using SparkServerLite.Interfaces;
using SparkServerLite.Models.Analytics;
using SparkServerLite.ViewModels;
using SparkServerLite.ViewModels.Analytics;

namespace SparkServerLite.Controllers
{
    [Authorize]
    public class AnalyticsController : BaseController
    {
        private readonly IAnalyticsRepository<Visit> _analyticsRepo;
        private readonly IWebHostEnvironment _host;
        private Analytics _analytics;
                
        public AnalyticsController(IAnalyticsRepository<Visit> analyticsRepo, Interfaces.ILogger logger, IWebHostEnvironment host, IAppSettings settings, IAppContent content) : base(settings, content, logger)
        {
            _host = host;
            _analyticsRepo = analyticsRepo;
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

            viewModel.ReportData = _analyticsRepo.ReportPageViews();

            return View(viewModel);

        }

        public IActionResult VisitsByMonth()
        {            
            ReportVisitsMonthViewModel viewModel = new();
            base.Setup(viewModel);
            ViewData["Title"] = "Report: Visits by Month";

            viewModel.ReportData = _analyticsRepo.ReportVisitsByMonth();

            return View(viewModel);
        }

    }
}