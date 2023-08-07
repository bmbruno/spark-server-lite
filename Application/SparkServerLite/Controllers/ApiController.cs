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
    public class Apiontroller : Controller
    {
        private IBlogRepository<Blog> _blogRepo;
        private IBlogTagRepository<BlogTag> _blogTagRepo;
        private IAuthorRepository<Author> _authorRepo;

        public Apiontroller(IBlogRepository<Blog> blogRepo, IBlogTagRepository<BlogTag> blogTagRepo, IAuthorRepository<Author> authorRepo)
        {
            _blogRepo = blogRepo;
            _blogTagRepo = blogTagRepo;
            _authorRepo = authorRepo;
        }

        public JsonResult MarkdownToHTML(string markdown)
        {
            JsonPayload json = new JsonPayload();

            return Json(json);
        }

    }
}