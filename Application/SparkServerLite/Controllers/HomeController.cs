using Microsoft.AspNetCore.Mvc;
using SparkServerLite.Interfaces;
using SparkServerLite.Models;
using SparkServerLite.ViewModels;
using System.Diagnostics;

namespace SparkServerLite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IBlogRepository<Blog> _blogRepo;

        public HomeController(ILogger<HomeController> logger, IBlogRepository<Blog> blogRepo)
        {
            _logger = logger;
            _blogRepo = blogRepo;
        }

        public IActionResult Index()
        {
            HomeViewModel viewModel = new HomeViewModel();

            var blogs = _blogRepo.GetRecent(5);

            viewModel.MapToViewModel(blogs);
            viewModel.MenuSelection = Infrastructure.Enums.MainMenu.Home;

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