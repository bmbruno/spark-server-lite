using Microsoft.AspNetCore.Mvc;
using SparkServerLite.Interfaces;
using SparkServerLite.Models;
using System.Diagnostics;

namespace SparkServerLite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IBlogRepository<Blog> _blogRepo;
        private IAuthorRepository<Author> _authorRepo;

        public HomeController(ILogger<HomeController> logger, IBlogRepository<Blog> blogRepo, IAuthorRepository<Author> authorRepo)
        {
            _logger = logger;
            _blogRepo = blogRepo;
            _authorRepo = authorRepo;

            var test = _authorRepo.Get(1);
            var test1 = _authorRepo.GetAll();
        }

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