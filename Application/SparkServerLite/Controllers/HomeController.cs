﻿using Microsoft.AspNetCore.Mvc;
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
        private readonly IBlogRepository<Blog> _blogRepo;
        private readonly IAppSettings _settings;

        public HomeController(ILogger<HomeController> logger, IBlogRepository<Blog> blogRepo, IAppSettings settings)
        {
            _logger = logger;
            _blogRepo = blogRepo;
            _settings = settings;
        }

        public IActionResult Index()
        {
            HomeViewModel viewModel = new HomeViewModel();

            var blogs = _blogRepo.GetRecent(5);

            viewModel.MapToViewModel(blogs, _settings);
            viewModel.MenuSelection = MainMenu.Home;
            viewModel.Sitename = _settings.Sitename;

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