using Microsoft.AspNetCore.Mvc;
using SparkServerLite.Infrastructure;
using SparkServerLite.Interfaces;
using SparkServerLite.Mapping;
using SparkServerLite.Models;
using SparkServerLite.Models.Analytics;
using SparkServerLite.ViewModels;

namespace SparkServerLite.Controllers
{
    public class BlogController : BaseController
    {
        private readonly IBlogRepository<Blog> _blogRepo;
        private readonly IBlogTagRepository<BlogTag> _blogTagRepo;
        private readonly Analytics _analytics;

        public BlogController(IBlogRepository<Blog> blogRepo, IBlogTagRepository<BlogTag> blogTagRepo, IAnalyticsRepository<Visit> analyticsRepo, Interfaces.ILogger logger, IAppSettings settings, IAppContent content) : base(settings, content, logger)
        {
            _blogRepo = blogRepo;
            _blogTagRepo = blogTagRepo;
            _analytics = new Analytics(_settings, analyticsRepo, _logger);
            this.ItemsPerPage = _settings.BlogItemsPerPage;
        }

        [HttpGet("posts/")]
        [HttpGet("posts/{year}")]
        [HttpGet("posts/{year}/{month}")]
        public ActionResult Index(int? year, int? month, int? page)
        {
            this.SetupPaging(page);

            BlogListViewModel viewModel = new();
            List<Blog> blogList = new();
            List<BlogTag> tagList = new();

            base.Setup(viewModel);

            try
            {
                if (year.HasValue && month.HasValue)
                {
                    // Blogs list by year + month
                    blogList = _blogRepo.GetByDate(year.Value, month.Value).ToList();
                    string monthName = new DateTime(year.Value, month.Value, 1).ToString("MMMM");
                    viewModel.Header = $"Blog Posts for {monthName} {year}";
                }
                else if (year.HasValue)
                {
                    // Blog list by year only
                    blogList = _blogRepo.GetByDate(year.Value, null).ToList();
                    viewModel.Header = $"Blog Posts for {year}";
                }
                else
                {
                    // Default: blog overview (top posts)
                    blogList = _blogRepo.GetAllPublished().ToList();
                    viewModel.Header = "Latest Blog Posts";
                }

                tagList = _blogTagRepo.GetTagsInUse().ToList();

                // Paging
                int totalCount = blogList.Count;
                blogList = blogList.Skip(this.SkipCount).Take(this.ItemsPerPage).ToList();
                viewModel.MapToViewModel(blogList, tagList, _settings);

                viewModel.Paging.PageCount = (totalCount + (this.ItemsPerPage - 1)) / this.ItemsPerPage;
                viewModel.Paging.CurrentPage = this.Page;
            }
            catch (Exception exc)
            {
                // TODO: log this exception
                TempData["Error"] = $"Error loading blog posts. Exception: {exc.Message}";
            }

            _analytics.RecordVisit(Request.Path, this.UserAgent, this.Referer);

            return View(viewModel);
        }

        [HttpGet("posts/{year}/{month}/{slug}")]
        public ActionResult Post(string slug = "", bool preview = false)
        {
            BlogViewModel viewModel = new();
            base.Setup(viewModel);

            if (String.IsNullOrEmpty(slug))
                return RedirectToAction(actionName: "Index", controllerName: "Blog");

            try
            {
                Blog blog = _blogRepo.Get(slug);
                IEnumerable<BlogTag> blogTags = _blogTagRepo.GetForBlog(blog.ID);

                viewModel.MapToViewModel(blog, blogTags, _settings);
                viewModel.IsPreview = preview;
                viewModel.MenuSelection = Infrastructure.Enums.MainMenu.Blog;

                // Should this blog post be displayed at all? (Preview flag overrides denied access in some cases)
                bool shouldDisplay = false;

                // Has published date passed?
                if (viewModel.PublishDate <= DateTime.Now)
                {
                    shouldDisplay = true;
                }

                // Is preview mode being used with an authenticated user?
                if (preview && User.Identity != null && User.Identity.IsAuthenticated)
                {
                    shouldDisplay = true;
                }

                if (shouldDisplay)
                {
                    _analytics.RecordVisit(Request.Path, this.UserAgent, this.Referer);
                    return View(viewModel);
                }
                else
                {
                    return RedirectToAction(actionName: "Index", controllerName: "Home");
                }

            }
            catch (Exception exc)
            {
                // TODO: log this exception
                TempData["Error"] = $"Error loading blog post. Exception: {exc.Message}";
            }

            _analytics.RecordVisit(Request.Path, this.UserAgent, this.Referer);

            return View(viewModel);
        }

        [HttpGet("posts/tag/{tagName}")]
        public ActionResult ListByTag(string tagName, int? page)
        {
            BlogListViewModel viewModel = new();
            base.Setup(viewModel);
            this.SetupPaging(page);
            
            try
            {
                string unencodedTagName = FormatHelper.GetTagNameFromURL(tagName);
                List<Blog> blogList = _blogRepo.GetByTagName(unencodedTagName).ToList();
                List<BlogTag> tagList = _blogTagRepo.GetTagsInUse().ToList();

                viewModel.MapToViewModel(blogList, tagList, _settings);
                viewModel.Header = $"Posts tagged '{unencodedTagName}'";

                // Paging
                int totalItems = blogList.Count;
                viewModel.BlogList = viewModel.BlogList.OrderByDescending(u => u.PublishDate).Skip(this.SkipCount).Take(this.ItemsPerPage).ToList();

                viewModel.Paging.PageCount = (totalItems + this.ItemsPerPage - 1) / this.ItemsPerPage;
                viewModel.Paging.CurrentPage = this.Page;
            }
            catch (Exception exc)
            {
                // TODO: log this exception
                TempData["Error"] = $"Error loading blogs by tag. Exception: {exc.Message}";
            }

            _analytics.RecordVisit(Request.Path, this.UserAgent, this.Referer);

            return View(viewName: "Index", model: viewModel);
        }
    }
}