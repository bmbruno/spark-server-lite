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
            json.Status = JsonStatus.OK;

            markdown = WebUtility.UrlDecode(markdown);

            try
            {
                json.Data = FormatHelper.MarkdownToHTML(markdown, _settings.SiteURL);
            }
            catch (Exception exc)
            {
                json.Status = JsonStatus.EXCEPTION;
                json.Message = exc.ToString();
            }

            return Json(json);
        }

        public JsonResult BlogMedia(int blogID)
        {
            JsonPayload json = new JsonPayload();

            MediaManager manager = new MediaManager(_settings);

            // TODO: load blog and get MediaFolder value

            manager.GetMediaForBlog("2023/3f5c2a0993da");

            return Json(json);
        }

    }
}