using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SparkServerLite.Infrastructure;
using SparkServerLite.Infrastructure.Enums;
using SparkServerLite.Interfaces;
using SparkServerLite.Mapping;
using SparkServerLite.Models;
using SparkServerLite.ViewModels;
using SparkServerLite.ViewModels.Admin;

namespace SparkServerLite.Controllers
{
    [Authorize]
    public class AdminController : BaseController
    {
        private readonly IBlogRepository<Blog> _blogRepo;
        private readonly IBlogTagRepository<BlogTag> _blogTagRepo;
        private readonly IAuthorRepository<Author> _authorRepo;
        private readonly IWebHostEnvironment _host;
        private readonly IAppSettings _settings;
        
        public AdminController(IBlogRepository<Blog> blogRepo, IBlogTagRepository<BlogTag> blogTagRepo, IAuthorRepository<Author> authorRepo, IWebHostEnvironment host, IAppSettings settings, IAppContent content) : base(settings, content)
        {
            _blogRepo = blogRepo;
            _blogTagRepo = blogTagRepo;
            _authorRepo = authorRepo;
            _settings = settings;
            _host = host;
        }

        public IActionResult Index()
        {
            BaseViewModel viewModel = new();
            base.Setup(viewModel);
            ViewData["Title"] = "Admin";

            return View(viewModel);
        }

        public ActionResult BlogList()
        {
            BlogEditListViewModel viewModel = new();
            base.Setup(viewModel);

            try
            {
                IEnumerable<Blog> blogs = _blogRepo.GetAll();
                viewModel.MapToViewModel(blogs);
            }
            catch (Exception exc)
            {
                // TODO: log this exception
                TempData["Error"] = $"Error loading blogs. Exception: {exc.Message}";
            }

            return View(viewModel);
        }

        public ActionResult BlogEdit(int? ID)
        {
            BlogEditViewModel viewModel = new();
            base.Setup(viewModel);

            if (ID.HasValue)
            {
                //
                // EDIT
                //

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
                viewModel.Markdown = blog.Markdown;
                viewModel.Content = blog.Content;
                viewModel.PublishDate = blog.PublishDate;
                viewModel.AuthorID = blog.AuthorID;
                viewModel.Slug = blog.Slug;
                viewModel.MediaFolder = blog.MediaFolder;
                viewModel.ImagePath = blog.ImagePath;
                viewModel.ImageThumbnailPath = blog.ImageThumbnailPath;

                IEnumerable<BlogTag> blogTags = _blogTagRepo.GetForBlog(blog.ID);
                IEnumerable<int> blogTagIDs = blogTags.Select(t => t.ID);

                viewModel.AuthorSource = FilterData.Authors(_authorRepo, viewModel.AuthorID);
                viewModel.BlogTagSource = FilterData.BlogTags(_blogTagRepo, blogTagIDs);
            }
            else
            {
                //
                // ADD
                //

                viewModel.Mode = EditMode.Add;

                viewModel.ID = 0;
                viewModel.AuthorSource = FilterData.Authors(_authorRepo, null);
                viewModel.BlogTagSource = FilterData.BlogTags(_blogTagRepo, null);
            }

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult BlogUpdate(BlogEditViewModel viewModel)
        {
            base.Setup(viewModel);

            // Check for unique URL
            if (viewModel.Mode == EditMode.Add && !String.IsNullOrEmpty(viewModel.Slug))
            {
                bool existing = _blogRepo.SlugExists(viewModel.Slug.Trim());

                if (existing)
                    ModelState.AddModelError("Slug", "URL slug is already in use!");
            }

            if (ModelState.IsValid)
            {
                if (viewModel.Mode == EditMode.Add)
                {
                    MediaManager media = new(_settings, _host);

                    Blog blog = new();

                    blog.Title = viewModel.Title;
                    blog.Subtitle = viewModel.Subtitle;
                    blog.Markdown = viewModel.Markdown;
                    blog.Content = viewModel.Content;
                    blog.PublishDate = viewModel.PublishDate;
                    blog.AuthorID = viewModel.AuthorID;
                    blog.Slug = viewModel.Slug;
                    blog.ImagePath = viewModel.ImagePath;
                    blog.ImageThumbnailPath = viewModel.ImageThumbnailPath;
                    blog.CreateDate = DateTime.Now;

                    int newBlogID = _blogRepo.Create(blog);
                    _blogTagRepo.UpdateTagsForBlog(newBlogID, viewModel.BlogTags);

                    TempData["Success"] = "Blog created.";
                    return RedirectToAction(actionName: "BlogEdit", controllerName: "Admin", routeValues: new { ID = newBlogID });
                }
                else
                {
                    var blog = _blogRepo.Get(viewModel.ID);

                    if (blog == null)
                    {
                        TempData["Error"] = $"No blog found with ID {viewModel.ID}.";
                        return RedirectToAction(actionName: "Index", controllerName: "Admin");
                    }

                    blog.Title = viewModel.Title;
                    blog.Subtitle = viewModel.Subtitle;
                    blog.Markdown = viewModel.Markdown;
                    blog.Content = viewModel.Content;
                    blog.PublishDate = viewModel.PublishDate;
                    blog.AuthorID = viewModel.AuthorID;
                    blog.Slug = viewModel.Slug;
                    blog.ImagePath = viewModel.ImagePath;
                    blog.ImageThumbnailPath = viewModel.ImageThumbnailPath;

                    _blogRepo.Update(blog);
                    _blogTagRepo.UpdateTagsForBlog(blog.ID, viewModel.BlogTags);

                    TempData["Success"] = "Blog updated.";
                    return RedirectToAction(actionName: "BlogEdit", controllerName: "Admin", routeValues: new { blog.ID });
                }

            }
            else
            {
                TempData["Error"] = "Please correct the errors below.";
            }

            viewModel.AuthorSource = FilterData.Authors(_authorRepo, viewModel.AuthorID);
            viewModel.BlogTagSource = FilterData.BlogTags(_blogTagRepo, viewModel.BlogTags);

            return View("BlogEdit", viewModel);
        }

        public ActionResult BlogDelete(int? ID)
        {
            if (ID.HasValue)
            {
                try
                {
                    _blogRepo.Delete(ID.Value);
                    TempData["Success"] = "Blog deleted.";
                }
                catch (Exception exc)
                {
                    TempData["Error"] = $"Could not delete blog. Exception: ${exc.Message}";
                }
            }
            else
            {
                TempData["Error"] = "ID required to delete blog.";
            }

            return RedirectToAction(actionName: "BlogList", controllerName: "Admin");
        }

        public ActionResult BlogTagList()
        {
            BlogTagListViewModel viewModel = new();
            base.Setup(viewModel);

            try
            {
                IEnumerable<BlogTag> allTags = _blogTagRepo.GetAllTagsWithCount();

                foreach (var tag in allTags)
                {
                    viewModel.BlogTagList.Add(new BlogTagListItemViewModel()
                    {
                        ID = tag.ID,
                        Name = tag.Name,
                        Uses = tag.Uses
                    });
                }
            }
            catch (Exception exc)
            {
                // TODO: log this exception
                TempData["Error"] = $"Error loading blog tags. Exception: {exc.Message}";
            }

            return View(viewModel);
        }

        public ActionResult BlogTagEdit(int? ID)
        {
            BlogTagEditViewModel viewModel = new();
            base.Setup(viewModel);

            if (ID.HasValue)
            {
                // EDIT

                viewModel.Mode = EditMode.Edit;

                var blog = _blogTagRepo.Get(ID.Value);

                if (blog == null)
                {
                    TempData["Error"] = $"No Blog Tag found with ID {ID.Value}.";
                    return RedirectToAction(actionName: "Index", controllerName: "Admin");
                }

                viewModel.ID = blog.ID;
                viewModel.Name = blog.Name;
            }
            else
            {
                // ADD

                viewModel.Mode = EditMode.Add;
            }

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult BlogTagUpdate(BlogTagEditViewModel viewModel)
        {
            viewModel.Name = viewModel.Name.Trim();

            // Check for existing Name
            if (ModelState.IsValid && viewModel.Mode == EditMode.Add)
            {
                bool existing = _blogTagRepo.Exists(viewModel.Name);

                if (existing)
                    ModelState.AddModelError("Name", "Name is not unique!");
            }

            if (ModelState.IsValid)
            {
                if (viewModel.Mode == EditMode.Add)
                {
                    BlogTag blogTag = new();
                    blogTag.Name = viewModel.Name;

                    try
                    {
                        _blogTagRepo.Create(blogTag);

                        TempData["Success"] = $"Blog Tag '{blogTag.Name}' created.";
                        return RedirectToAction(actionName: "BlogTagList", controllerName: "Admin");
                    }
                    catch (Exception exc)
                    {
                        TempData["Error"] = $"Could not create Blog Tag '{viewModel.Name}'. Exception: {exc.Message}";
                    }
                }
                else
                {
                    var blogTag = _blogTagRepo.Get(viewModel.ID);

                    if (blogTag == null)
                    {
                        TempData["Error"] = $"No Blog Tag found with ID {viewModel.ID}.";
                        return RedirectToAction(actionName: "Index", controllerName: "Admin");
                    }

                    blogTag.Name = viewModel.Name;

                    try
                    {
                        _blogTagRepo.Update(blogTag);
                        TempData["Success"] = $"Blog Tag '{blogTag.Name}' updated.";
                    }
                    catch (Exception exc)
                    {
                        TempData["Error"] = $"Could not update blog tag. Exception: ${exc.Message}";
                    }

                    return RedirectToAction(actionName: "BlogTagList", controllerName: "Admin");
                }

            }
            else
            {
                TempData["Error"] = "Please correct the errors below.";
            }

            return View("BlogTagEdit", viewModel);
        }

        public ActionResult BlogTagDelete(int? ID)
        {
            if (ID.HasValue)
            {
                try
                {
                    _blogTagRepo.Delete(ID.Value);
                    TempData["Success"] = $"Blog tag deleted.";
                }
                catch (Exception exc)
                {
                    TempData["Error"] = $"Could not delete blog tag. Exception: ${exc.Message}";
                }
            }
            else
            {
                TempData["Error"] = "ID required to delete blog tag.";
            }

            return RedirectToAction(actionName: "BlogTagList", controllerName: "Admin");
        }

        public ActionResult Media()
        {
            BaseViewModel viewModel = new();
            this.Setup(viewModel);

            return View(viewModel);
        }
    }
}