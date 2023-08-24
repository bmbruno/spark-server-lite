using Microsoft.AspNetCore.Mvc;
using SparkServerLite.Infrastructure;
using SparkServerLite.Infrastructure.Enums;
using SparkServerLite.Interfaces;
using SparkServerLite.Models;
using System.Net;

namespace SparkServerLite.Controllers
{
    public class ApiController : Controller
    {
        private readonly IBlogRepository<Blog> _blogRepo;
        private readonly IBlogTagRepository<BlogTag> _blogTagRepo;
        private readonly IAuthorRepository<Author> _authorRepo;
        private readonly IAppSettings _settings;

        public ApiController(IBlogRepository<Blog> blogRepo, IBlogTagRepository<BlogTag> blogTagRepo, IAuthorRepository<Author> authorRepo, IAppSettings settings)
        {
            _blogRepo = blogRepo;
            _blogTagRepo = blogTagRepo;
            _authorRepo = authorRepo;
            _settings = settings;
        }

        [HttpPost]
        public JsonResult MarkdownToHTML(string markdown)
        {
            JsonPayload json = new JsonPayload();
            json.Status = JsonStatus.OK.ToString();

            markdown = WebUtility.UrlDecode(markdown);

            try
            {
                json.Data = FormatHelper.MarkdownToHTML(markdown, _settings.SiteURL);
            }
            catch (Exception exc)
            {
                json.Status = JsonStatus.EXCEPTION.ToString();
                json.Message = exc.ToString();
            }

            return Json(json);
        }

        public JsonResult BlogMedia(int blogID)
        {
            JsonPayload json = new JsonPayload();

            MediaManager manager = new MediaManager(_settings);

            // Load blog and validate data
            Blog blog = _blogRepo.Get(blogID);

            if (blog == null)
            {
                json.Status = JsonStatus.ERROR.ToString();
                json.Message = $"No blog found for ID {blogID}.";
                return Json(json);
            }

            if (String.IsNullOrEmpty(blog.MediaFolder))
            {
                json.Status = JsonStatus.ERROR.ToString();
                json.Message = $"No media folder stored for Blog ID {blogID}.";
                return Json(json);
            }

            // Load list of files from media folder
            List<MediaItem> list = manager.GetMediaForBlog("2023/3f5c2a0993da");

            // Blank out server path for client-side data
            foreach (MediaItem item in list)
            {
                item.ServerPath = null;
            }

            json.Status = JsonStatus.OK.ToString();
            json.Message = string.Empty;
            json.Data = list;

            return Json(json);
        }
    }
}