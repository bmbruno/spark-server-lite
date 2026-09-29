using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SparkServerLite.Infrastructure;
using SparkServerLite.Interfaces;
using SparkServerLite.Models.Analytics;
using SparkServerLite.ViewModels;
using SparkServerLite.ViewModels.Analytics;
using SparkServerLite.ViewModels.Seo;

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
            RobotsViewModel viewModel = new();
            
            // TODO: load robots.txt and populate viewModel
            
            return View(viewModel);
        }
        
        [HttpPost]
        public ActionResult SaveRobots(RobotsViewModel viewModel)
        {
            // TODO: validate robots content
            
            // TODO: save to file
            
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