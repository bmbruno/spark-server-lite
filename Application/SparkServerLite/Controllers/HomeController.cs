using Microsoft.AspNetCore.Mvc;
using SparkServerLite.Infrastructure;
using SparkServerLite.Infrastructure.Enums;
using SparkServerLite.Interfaces;
using SparkServerLite.Mapping;
using SparkServerLite.Models;
using SparkServerLite.ViewModels;
using System.Diagnostics;

namespace SparkServerLite.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IBlogRepository<Blog> _blogRepo;
        private readonly Analytics _analytics;

        public HomeController(IBlogRepository<Blog> blogRepo, IAnalyticsRepository<Visit> analyticsRepo, Interfaces.ILogger logger, IAppSettings settings, IAppContent content) : base(settings, content, logger)
        {
            _blogRepo = blogRepo;
            _analytics = new Analytics(_settings, analyticsRepo, _logger);
        }

        public IActionResult Index()
        {
            HomeViewModel viewModel = new HomeViewModel();
            base.Setup(viewModel);

            var blogs = _blogRepo.GetRecent(5);
            viewModel.MapToViewModel(blogs, _settings);
            viewModel.MenuSelection = MainMenu.Home;

            _analytics.RecordVisit(Request.Path, this.UserAgent, this.Referer);

            return View(viewModel);
        }

        public ActionResult About()
        {
            HomeViewModel viewModel = new HomeViewModel();
            base.Setup(viewModel);

            viewModel.MenuSelection = MainMenu.About;

            return View(viewModel);
        }
    }
}