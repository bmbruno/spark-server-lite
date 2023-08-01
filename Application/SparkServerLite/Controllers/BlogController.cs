using Microsoft.AspNetCore.Mvc;
using SparkServerLite.Interfaces;
using SparkServerLite.Mapping;
using SparkServerLite.Models;
using SparkServerLite.ViewModels;

namespace SparkServerLite.Controllers
{
    public class BlogController : BaseController
    {
        private readonly IBlogRepository<Blog> _blogRepo;
        private readonly IBlogTagRepository<BlogTag> _blogTagRepo;

        public BlogController(IBlogRepository<Blog> blogRepo, IBlogTagRepository<BlogTag> blogTagRepo)
        {
            _blogRepo = blogRepo;
            _blogTagRepo = blogTagRepo;
            this.ItemsPerPage = 3;
        }

        [HttpGet("blog/")]
        [HttpGet("blog/{year}")]
        [HttpGet("blog/{year}/{month}")]
        public ActionResult Index(int? year, int? month, int? page)
        {
            this.SetupPaging(page);

            BlogListViewModel viewModel = new BlogListViewModel();
            List<Blog> blogList = new List<Blog>();
            List<BlogTag> tagList = new List<BlogTag>();

            if (year.HasValue && month.HasValue)
            {
                // Blogs list by year + month

                // TODO: determine how to get total number of blogs (for paging information)
                blogList = _blogRepo.GetByDate(year.Value, month.Value, this.Page, this.ItemsPerPage).ToList();
                string monthName = new DateTime(year.Value, month.Value, 1).ToString("MMMM");
                viewModel.Header = $"Blog Posts for {monthName} {year.ToString()}";
            }
            else if (year.HasValue)
            {
                // Blog list by year only
                blogList = _blogRepo.GetByDate(year.Value, null, this.Page, this.ItemsPerPage).ToList();
                viewModel.Header = $"Blog Posts for {year.ToString()}";
            }
            else
            {
                // Default: blog overview (top posts)
                blogList = _blogRepo.GetAll(this.Page, this.ItemsPerPage).ToList();
                viewModel.Header = "Latest Blog Posts";
            }

            // TODO: implement GetAll()
            tagList = _blogTagRepo.GetAll().OrderBy(u => u.Name).ToList();

            // TODO
            viewModel.MapToViewModel(blogList, tagList);

            viewModel.Paging.PageCount = (blogList.Count + (this.ItemsPerPage - 1)) / this.ItemsPerPage;
            viewModel.Paging.CurrentPage = this.Page;

            return View(viewModel);
        }

        [HttpGet("blog/{year}/{month}/{slug}")]
        public ActionResult Post(int year, int month, string slug = "", bool preview = false)
        {
            BlogViewModel viewModel = new BlogViewModel();

            if (String.IsNullOrEmpty(slug))
                return RedirectToAction(actionName: "Index", controllerName: "Blog");

            Blog blog = _blogRepo.Get(year, month, slug);
            IEnumerable<BlogTag> blogTags = _blogTagRepo.GetForBlog(blog.ID);
            viewModel.MapToViewModel(blog, blogTags);
            viewModel.IsPreview = preview;

            // Should this blog post be displayed at all? (Preview flag overrides denied access in some cases)
            bool shouldDisplay = false;

            // Has published date passed?
            if (viewModel.PublishDate <= DateTime.Now)
            {
                shouldDisplay = true;
            }

            // Is preview mode being used with an authenticated user?
            if (preview && User != null && User.Identity.IsAuthenticated)
            {
                shouldDisplay = true;
            }

            if (shouldDisplay)
                return View(viewModel);
            else
                return RedirectToAction(actionName: "Index", controllerName: "Home");
        }
    }
}
