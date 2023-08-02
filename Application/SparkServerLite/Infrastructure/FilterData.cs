﻿using Microsoft.AspNetCore.Mvc.Rendering;
using SparkServerLite.Interfaces;
using SparkServerLite.Models;

namespace SparkServerLite.Infrastructure
{
    public static class FilterData
    {
        public static List<SelectListItem> Authors(IAuthorRepository<Author> repo, int? selected)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            IEnumerable<Author> sourceList = repo.GetAll().OrderBy(u => u.FirstName).ThenBy(u => u.LastName);

            list.Add(new SelectListItem() { Value = string.Empty, Text = string.Empty });

            foreach (var item in sourceList)
            {
                SelectListItem listItem = new SelectListItem()
                {
                    Value = item.ID.ToString(),
                    Text = string.Format($"{item.FirstName} {item.LastName}")
                };

                if (item.ID == selected)
                {
                    listItem.Selected = true;
                }

                list.Add(listItem);
            }

            return list;
        }

        public static List<SelectListItem> BlogTags(IBlogTagRepository<BlogTag> repo, IEnumerable<int> selected)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            IEnumerable<BlogTag> sourceList = repo.GetAll().OrderBy(u => u.Name);

            foreach (var item in sourceList)
            {
                SelectListItem listItem = new SelectListItem()
                {
                    Value = item.ID.ToString(),
                    Text = item.Name
                };

                // TODO: make sure the selection logic works
                foreach (int selectedID in selected)
                {
                    if (item.ID == selectedID)
                    {
                        listItem.Selected = true;
                    }
                }

                list.Add(listItem);
            }

            return list;
        }
    }
}
