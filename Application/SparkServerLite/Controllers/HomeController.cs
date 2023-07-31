using Microsoft.AspNetCore.Mvc;
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

            return View(viewModel);
        }

        public ActionResult About()
        {
            HomeViewModel viewModel = new HomeViewModel();
            viewModel.MenuSelection = MainMenu.About;

            return View(viewModel);
        }
    }
}