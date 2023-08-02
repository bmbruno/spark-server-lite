using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SparkServerLite.Interfaces;
using SparkServerLite.Models;
using SparkServerLite.ViewModels;
using SparkServerLite.ViewModels.Admin;

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

            var blogs = _blogRepo.GetAll();

            foreach (var blog in blogs)
            {
                viewModel.BlogList.Add(new BlogListItemViewModel()
                {
                    ID = blog.ID,
                    Title = blog.Title,
                    Subtitle = blog.Subtitle,
                    PublishedDate = blog.PublishDate.ToShortDateString(),
                    AuthorName = !String.IsNullOrEmpty(blog.AuthorFullName) ? blog.AuthorFullName : "N/A"
                });
            }

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
