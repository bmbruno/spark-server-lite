using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SparkServerLite.Infrastructure;
using SparkServerLite.Infrastructure.Enums;
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
            //Blog blog = _blogRepo.Get(2);
            //blog.Title = "A MODIFIED TITLE";
            //_blogRepo.Update(blog);

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
            BlogEditViewModel viewModel = new BlogEditViewModel();

            if (ID.HasValue)
            {
                // EDIT

                viewModel.Mode = EditMode.Edit;

                var blog = _blogRepo.Get(ID: ID.Value);

                if (blog == null)
                {
                    TempData["Error"] = $"No blog found with ID {ID.Value}.";
                    return RedirectToAction(actionName: "Index", controllerName: "Admin");
                }

                viewModel.ID = blog.ID;
                viewModel.Title = blog.Title;
                viewModel.Subtitle = blog.Subtitle;
                viewModel.Content = blog.Content;
                viewModel.PublishDate = blog.PublishDate;
                viewModel.AuthorID = blog.AuthorID;
                viewModel.Slug = blog.Slug;
                viewModel.ImagePath = blog.ImagePath;
                viewModel.ImageThumbnailPath = blog.ImageThumbnailPath;
                viewModel.BlogURL = $"/blog/{blog.PublishDate.Year}/{blog.PublishDate.Month}/{blog.Slug}";

                IEnumerable<BlogTag> blogTags = _blogTagRepo.GetForBlog(blog.ID);
                IEnumerable<int> blogTagIDs = blogTags.Select(t => t.ID);

                viewModel.AuthorSource = FilterData.Authors(_authorRepo, viewModel.AuthorID);
                viewModel.BlogTagSource = FilterData.BlogTags(_blogTagRepo, blogTagIDs);
            }
            else
            {
                // ADD

                viewModel.Mode = EditMode.Add;

                viewModel.ID = 0;
                viewModel.AuthorSource = FilterData.Authors(_authorRepo, null);
                viewModel.BlogTagSource = FilterData.BlogTags(_blogTagRepo, viewModel.BlogTags);
            }

            return View(viewModel);
        }

        [HttpPost]
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

        [HttpPost]
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