using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SparkServer.Infrastructure.Repositories;
using SparkServerLite.Infrastructure;
using SparkServerLite.Interfaces;
using SparkServerLite.Models;
using SparkServerLite.ViewModels;
using SparkServerLite.ViewModels.Seo;

namespace SparkServerLite.Controllers
{
    [Authorize]
    public class SeoController : BaseController
    {
        private readonly IWebHostEnvironment _host;
        private readonly SeoManager _seoManager;
        private readonly IBlogRepository<Blog> _blogRepo;
                
        public SeoController(Interfaces.ILogger logger, IWebHostEnvironment host, IAppSettings settings, IAppContent content, IBlogRepository<Blog> blogRepo) : base(settings, content, logger)
        {
            _host = host;
            _blogRepo = blogRepo;
            _seoManager = new SeoManager(settings, host);
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

            try
            {
                viewModel.RobotsContent = _seoManager.LoadRobotsTxtFromDisk();
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Could not load robots.txt: {ex.Message}";
                return RedirectToAction(actionName: "Index", controllerName: "Seo");
            }
            
            return View(viewModel);
        }
        
        [HttpPost]
        public ActionResult SaveRobots(RobotsViewModel viewModel)
        {
            // TODO: validate robots content

            try
            {
                _seoManager.SaveRobotsTxtToDisk(viewModel.RobotsContent);
                TempData["Success"] = "robots.txt updated.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            
            return View(viewName: "Robots", model: viewModel);
        }
        
        public ActionResult Sitemap()
        {
            SitemapViewModel viewModel = new();
            
            viewModel.Sitemap = _seoManager.LoadSitemapFromDisk().ToList();
            
            return View(viewModel);
        }
        
        [HttpPost]
        public ActionResult RebuildSitemap()
        {
            _seoManager.RebuildSitemap(_blogRepo);
            
            TempData["Success"] = "Sitemap rebuilt.";
            
            return RedirectToAction(actionName: "Sitemap", controllerName: "Seo");
        }
    }
}