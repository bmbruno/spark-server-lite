using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkServerLite.Interfaces
{
    public interface IBlogTagRepository<T> : IRepositoryBase<T>
    {
        /// <summary>
        /// Should determine if a BlogTag exists in the database based on the name (lowercase comparison).
        /// </summary>
        /// <param name="name">Name of the tag.</param>
        /// <returns>True/false.</returns>
        public bool Exists(string name);

        /// <summary>
        /// Should return a list of blog tag objects that are currently tied to blogs.
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetTagsInUse();

        /// <summary>
        /// Should return a list of blog tag objects for the given blog ID.
        /// </summary>
        /// <param name="blogID">ID of the blog.</param>
        /// <returns>IEnumerable of type T.</returns>
        IEnumerable<T> GetForBlog(int blogID);

        /// <summary>
        /// SHould clear and set all blog tags for a given blog ID.
        /// </summary>
        /// <param name="blogID">ID of the blog to update related tags.</param>
        /// <param name="newTagIDList">List of actively-selected blog tag IDs.</param>
        void UpdateTagsForBlog(int blogID, IEnumerable<int> newTagIDList);

        /// <summary>
        /// Should return a list of blog tag objects and a count of their uses across all blogs.
        /// </summary>
        /// <returns>IEnumerable of type T.</returns>
        IEnumerable<T> GetAllTagsWithCount();
    }
}
