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
            ViewData["Title"] = "SEO";

            return View(viewModel);
        }

        public ActionResult Robots()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult SaveRobots()
        {
            return View(viewName: "Robots");
        }
        
        public ActionResult Sitemap()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult RebuildSitmap()
        {
            return View(viewName: "Sitemap");
        }
    }
}