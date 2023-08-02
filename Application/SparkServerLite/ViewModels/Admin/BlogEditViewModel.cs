﻿using Microsoft.AspNetCore.Mvc.Rendering;
using SparkServerLite.Infrastructure.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SparkServerLite.ViewModels.Admin
{
    public class BlogEditViewModel : BaseViewModel
    {
        public EditMode Mode { get; set; }
        public int ID { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Subtitle")]
        public string Subtitle { get; set; }

        [Display(Name = "Source (Markdown)")]
        public string ContentMarkdown { get; set; }

        [Required]
        [Display(Name = "Body")]
        public string Content { get; set; }

        [Required]
        [Display(Name = "Publish Date")]
        public DateTime? PublishDate { get; set; }

        [Required]
        [Display(Name = "Author")]
        public int AuthorID { get; set; }

        public bool Active { get; set; }

        public DateTime CreateDate { get; set; }

        [Required]
        [Display(Name = "Slug")]
        public string Slug { get; set; }

        public string ImagePath { get; set; }

        public string ImageThumbnailPath { get; set; }

        public List<SelectListItem> AuthorSource { get; set; }

        [Display(Name = "Blog Tags")]
        public List<int> BlogTags { get; set; }

        public List<SelectListItem> BlogTagSource { get; set; }

        public string BlogURL { get; set; }

        public BlogEditViewModel()
        {
            AuthorSource = new List<SelectListItem>();
            BlogTags = new List<int>();
            BlogTagSource = new List<SelectListItem>();
        }
    }
}
