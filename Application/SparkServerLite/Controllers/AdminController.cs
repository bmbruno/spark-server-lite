using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SparkServerLite.Interfaces;
using SparkServerLite.Models;
using SparkServerLite.ViewModels;

namespace SparkServerLite.Controllers
{
    public class AdminController : Controller
    {
        private IBlogRepository<Blog> _blogRepo;
        private IBlogTagRepository<BlogTag> _blogTagRepo;
        private IAuthorRepository<Author> _authorRepo;

        public AdminController(IBlogRepository<Blog> blogRepo, IBlogTagRepository<BlogTag> blogTagRepo, IAuthorRepository<Author> authorRepo)
        {
            _blogRepo = blogRepo;
            _blogTagRepo = blogTagRepo;
            _authorRepo = authorRepo;
        }

        public IActionResult Index()
        {
            

            return View();
        }

        public ActionResult BlogList()
        {
            BlogEditListViewModel viewModel = new BlogEditListViewModel();

            return View(viewModel);
        }

        public ActionResult BlogEdit(int? ID)
        {
            return View();
        }

        public ActionResult BlogUpdate(BlogEditViewModel viewModel)
        {
            return View();
        }

        public ActionResult BlogDelete(int? ID)
        {
            return View();
        }

        public ActionResult BlogTagList()
        {
            return View();
        }

        public ActionResult BlogTagEdit(int? ID)
        {
            return View();
        }

        public ActionResult BlogTagUpdate(BlogTagEditViewModel viewModel)
        {
            return View();
        }

        public ActionResult BlogTagDelete(int? ID)
        {
            return View();
        }
    }
}
