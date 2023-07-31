﻿using Microsoft.AspNetCore.Mvc;
using SparkServerLite.Interfaces;
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
            this.ItemsPerPage = 10;
        }

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
                blogList = _blogRepo.GetByDate(year.Value, month.Value).OrderByDescending(u => u.PublishDate).ToList();
                string monthName = new DateTime(year.Value, month.Value, 1).ToString("MMMM");
                viewModel.Header = $"Blog Posts for {monthName} {year.ToString()}";
            }
            else if (year.HasValue)
            {
                // Blog list by year only
                blogList = _blogRepo.GetByDate(year.Value, null).OrderByDescending(u => u.PublishDate).ToList();
                viewModel.Header = $"Blog Posts for {year.ToString()}";
            }
            else
            {
                // Default: blog overview (top posts)

                // TODO
                // blogList = _blogRepo.Get(u => u.Active && u.PublishDate <= DateTime.Now).OrderByDescending(u => u.PublishDate).ToList();
                viewModel.Header = "Latest Blog Posts";
            }

            tagList = _blogTagRepo.GetAll().OrderBy(u => u.Name).ToList();

            int totalItems = blogList.Count;
            blogList = blogList.Skip(this.SkipCount).Take(this.ItemsPerPage).ToList();

            // TODO
            // viewModel.MapToViewModel(blogList, tagList);

            viewModel.Paging.PageCount = (totalItems + this.ItemsPerPage - 1) / this.ItemsPerPage;
            viewModel.Paging.CurrentPage = this.Page;

            return View(viewName: $"IndexOverview", model: viewModel);
        }
    }
}