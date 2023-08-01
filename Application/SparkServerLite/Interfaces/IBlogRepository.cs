﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkServerLite.Interfaces
{
    public interface IBlogRepository<T> : IRepositoryBase<T>
    {
        /// <summary>
        /// Should retrieve a blog object from a datastore using the unique URL scheme.
        /// </summary>
        /// <param name="year">Year.</param>
        /// <param name="month">Month.</param>
        /// <param name="slug">Unique URL slug.</param>
        /// <returns>Object of type T.</returns>
        T Get(int year, int month, string slug);

        /// <summary>
        /// Should retrieve blog objects from a datastore for any combination of year + month. Year is a minimum requirement.
        /// </summary>
        /// <param name="year">Year.</param>
        /// <param name="month">Month.</param>
        /// <param name="page">Page value for paging.</param>
        /// <param name="numberToTake">Number of items for paging.</param>
        /// <returns>Object of type T.</returns>
        IEnumerable<T> GetByDate(int year, int? month);

        /// <summary>
        /// Should return an enumerable of blog objects ordered by publish date descending, limited by the numberToLoad.
        /// </summary>
        /// <param name="numberToLoad">Number of blog objects to load.</param>
        /// <returns>Enumerable of blog-type objects.</returns>
        IEnumerable<T> GetRecent(int numberToLoad);

        /// <summary>
        /// Should retrieve blog-type objects from a datastore based on tag ID.
        /// </summary>
        /// <param name="tagID">ID of tag object.</param>
        /// <param name="page">Page value for paging.</param>
        /// <param name="numberToTake">Number of items for paging.</param>
        /// <returns>IEnumerable of blog-type objecs.</returns>
        IEnumerable<T> GetByTagID(int tagID, int? page, int? numberToTake);
    }
}
