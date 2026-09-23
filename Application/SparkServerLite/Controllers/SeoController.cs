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
    public class SeoController : BaseController
    {
        private readonly IWebHostEnvironment _host;
                
        public SeoController(Interfaces.ILogger logger, IWebHostEnvironment host, IAppSettings settings, IAppContent content) : base(settings, content, logger)
        {
            _host = host;
        }

        public IActionResult Index()
        {
            BaseViewModel viewModel = new();
            base.Setup(viewModel);
            ViewData["Title"] = "Analytics";

            return View(viewModel);
        }
    }
}