using Microsoft.AspNetCore.Mvc;
using SparkServerLite.Models;
using System.Diagnostics;
using System.Reflection.Metadata;

namespace SparkServerLite.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        //private IArticleRepository<Article> _articleRepo;
        //private IBlogRepository<Blog> _blogRepo;

        //public HomeController(ILogger<HomeController> logger, IArticleRepository<Article> articleRepo, IBlogRepository<Blog> blogRepo)
        //{
        //    _logger = logger;
        //    _articleRepo = articleRepo;
        //    _blogRepo = blogRepo;
        //}

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}