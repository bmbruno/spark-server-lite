using System;
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
        /// <param name="slug">Unique URL slug.</param>
        /// <returns>Object of type T.</returns>
        T Get(string slug);

        /// <summary>
        /// Should retrieve a blog objects from a datastore that are active and published.
        /// </summary>
        /// <returns>Enumerable of blog-type objects.</returns>
        IEnumerable<T> GetAllPublished();

        /// <summary>
        /// Should retrieve blog objects from a datastore for any combination of year + month. Year is a minimum requirement.
        /// </summary>
        /// <param name="year">Year.</param>
        /// <param name="month">Month.</param>
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
        /// <returns>IEnumerable of blog-type objecs.</returns>
        IEnumerable<T> GetByTagID(int tagID);

        /// <summary>
        /// Should retrieve blog-type objects from a datastore based on tag name.
        /// </summary>
        /// <param name="tagName">Name of tag object.</param>
        /// <returns>IEnumerable of blog-type objecs.</returns>
        IEnumerable<T> GetByTagName(string tagName);

        /// <summary>
        /// Should determine if the given URL slug exists in the database.
        /// </summary>
        /// <param name="slug">URL slug</param>
        /// <returns>True/false.</returns>
        public bool SlugExists(string slug);

        /// <summary>
        /// Gets the latest blog post that uses a default hero image.
        /// </summary>
        /// <param name="folderFragment">String fragment that identifies what path default heros use.</param>
        /// <returns>Filename of the latest hero image in use.</returns>
        public string GetLatestBlogBanner(string folderFragment);
    }
}
