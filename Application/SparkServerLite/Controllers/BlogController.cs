using Microsoft.AspNetCore.Mvc;
using SparkServerLite.Infrastructure;
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
        private readonly IAppSettings _settings;

        public BlogController(IBlogRepository<Blog> blogRepo, IBlogTagRepository<BlogTag> blogTagRepo, IAppSettings settings)
        {
            _blogRepo = blogRepo;
            _blogTagRepo = blogTagRepo;
            _settings = settings;
            this.ItemsPerPage = _settings.BlogItemsPerPage;
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

            base.Setup(viewModel, _settings);

            if (year.HasValue && month.HasValue)
            {
                // Blogs list by year + month
                blogList = _blogRepo.GetByDate(year.Value, month.Value).ToList();
                string monthName = new DateTime(year.Value, month.Value, 1).ToString("MMMM");
                viewModel.Header = $"Blog Posts for {monthName} {year.ToString()}";
            }
            else if (year.HasValue)
            {
                // Blog list by year only
                blogList = _blogRepo.GetByDate(year.Value, null).ToList();
                viewModel.Header = $"Blog Posts for {year.ToString()}";
            }
            else
            {
                // Default: blog overview (top posts)
                blogList = _blogRepo.GetAll().ToList();
                viewModel.Header = "Latest Blog Posts";
            }

            tagList = _blogTagRepo.GetTagsInUse().ToList();

            // Paging
            int totalCount = blogList.Count;
            blogList = blogList.Skip(this.SkipCount).Take(this.ItemsPerPage).ToList();
            viewModel.MapToViewModel(blogList, tagList, _settings);

            viewModel.Paging.PageCount = (totalCount + (this.ItemsPerPage - 1)) / this.ItemsPerPage;
            viewModel.Paging.CurrentPage = this.Page;

            return View(viewModel);
        }

        [HttpGet("blog/{year}/{month}/{slug}")]
        public ActionResult Post(int year, int month, string slug = "", bool preview = false)
        {
            BlogViewModel viewModel = new BlogViewModel();
            base.Setup(viewModel, _settings);

            if (String.IsNullOrEmpty(slug))
                return RedirectToAction(actionName: "Index", controllerName: "Blog");

            Blog blog = _blogRepo.Get(year, month, slug);
            IEnumerable<BlogTag> blogTags = _blogTagRepo.GetForBlog(blog.ID);

            viewModel.MapToViewModel(blog, blogTags, _settings);
            viewModel.IsPreview = preview;
            //viewModel.SiteURL = _settings.SiteURL;
            //viewModel.Sitename = _settings.Sitename;

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

        [HttpGet("blog/tag/{tagName}")]
        public ActionResult ListByTag(string tagName, int? page)
        {
            this.SetupPaging(page);
            
            BlogListViewModel viewModel = new BlogListViewModel();
            List<Blog> blogList = new List<Blog>();
            List<BlogTag> tagList = new List<BlogTag>();

            base.Setup(viewModel, _settings);

            string unencodedTagName = FormatHelper.GetTagNameFromURL(tagName);
            blogList = _blogRepo.GetByTagName(unencodedTagName).ToList();
            tagList = _blogTagRepo.GetTagsInUse().ToList();

            viewModel.MapToViewModel(blogList, tagList, _settings);
            viewModel.Header = $"Posts tagged '{unencodedTagName}'";

            // Paging
            int totalItems = blogList.Count;
            viewModel.BlogList = viewModel.BlogList.OrderByDescending(u => u.PublishDate).Skip(this.SkipCount).Take(this.ItemsPerPage).ToList();

            viewModel.Paging.PageCount = (totalItems + this.ItemsPerPage - 1) / this.ItemsPerPage;
            viewModel.Paging.CurrentPage = this.Page;

            return View(viewName: "Index", model: viewModel);
        }
    }
}