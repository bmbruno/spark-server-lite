﻿namespace SparkServerLite.ViewModels
{
    public class BlogTagViewModel
    {
        public int BlogTagID { get; set; }

        public string BlogTagName { get; set; }

        public BlogTagViewModel()
        {
            BlogTagName = string.Empty;
        }
    }
}
