using Microsoft.Data.Sqlite;
using SparkServerLite.Infrastructure;
using SparkServerLite.Interfaces;
using SparkServerLite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SparkServer.Infrastructure.Repositories
{
    public class BlogRepository : IBlogRepository<Blog>
    {
        public Blog Get(int ID)
        {
            //Blog item;

            //using (var db = new SparkServerEntities())
            //{
            //    item = db.Blog.Include(u => u.Author).Include(u => u.BlogsTags).FirstOrDefault(u => u.ID == ID);
            //}

            //return item;

            return new Blog();
        }

        public IEnumerable<Blog> Get(Expression<Func<Blog, bool>> whereClause)
        {
            // CALLING: ArticleRepo.Get(x => x.Title == "abcdef");
            // USING: db.Articles.Where(whereClause);

            //List<Blog> results;

            //using (var db = new SparkServerEntities())
            //{
            //    results = db.Blog.Where(whereClause).Include(u => u.Author).Include(u => u.BlogsTags).ToList();
            //}

            //return results;

            return new List<Blog>();
        }

        public IEnumerable<Blog> GetAll(int? page, int? numberToTake)
        {
            List<Blog> blogList = new List<Blog>();

            using (var conn = new SqliteConnection(Database.SQLiteConnectionString))
            {
                SqliteCommand command = conn.CreateCommand();
                command.CommandText = @"
                    SELECT
	                    Blogs.*,
	                    Authors.ID AS 'AuthorID',
	                    Authors.FirstName || ' ' || Authors.LastName AS 'AuthorFullName'
                    FROM
	                    Blogs
	                    INNER JOIN Authors ON Authors.ID = Blogs.AuthorID
                    WHERE
	                    Blogs.Active = 1
	                    AND PublishDate <= datetime('now')
                    ORDER BY
	                    PublishDate DESC ";


                if (page.HasValue && numberToTake.HasValue)
                {
                    command.CommandText += "LIMIT $numToTake OFFSET $pageSkip";
                    command.Parameters.AddWithValue("$numToTake", numberToTake);
                    command.Parameters.AddWithValue("$pageSkip", (page - 1) * numberToTake);
                }

                conn.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        blogList.Add(new Blog()
                        {

                            ID = Database.GetID(reader["ID"]),
                            Title = Database.GetString(reader["Title"]),
                            Subtitle = Database.GetString(reader["Subtitle"]),
                            Content = Database.GetString(reader["Content"]),
                            ImagePath = Database.GetString(reader["ImagePath"]),
                            ImageThumbnailPath = Database.GetString(reader["ImageThumbnailPath"]),
                            Slug = Database.GetString(reader["Slug"]),
                            PublishDate = Database.GetDateTime(reader["PublishDate"]).Value,
                            AuthorID = Database.GetID(reader["AuthorID"]),
                            AuthorFullName = Database.GetString(reader["AuthorFullName"])
                        });
                    }
                }

                conn.Close();
            }

            return blogList;
        }

        public IEnumerable<Blog> GetAll()
        {
            //List<Blog> results;

            //using (var db = new SparkServerEntities())
            //{
            //    results = db.Blog.Where(u => u.Active).Include(u => u.Author).Include(u => u.BlogsTags).ToList();
            //}

            //return results;

            return new List<Blog>();
        }

        public int Create(Blog newItem)
        {
            //using (var db = new SparkServerEntities())
            //{
            //    db.Blog.Add(newItem);
            //    db.SaveChanges();
            //}

            //return newItem.ID;

            return 0;
        }

        public void Update(Blog updateItem)
        {
            //using (var db = new SparkServerEntities())
            //{
            //    db.Blog.Attach(updateItem);

            //    var entry = db.Entry(updateItem);
            //    entry.Property(e => e.Title).IsModified = true;
            //    entry.Property(e => e.Subtitle).IsModified = true;
            //    entry.Property(e => e.Body).IsModified = true;
            //    entry.Property(e => e.PublishDate).IsModified = true;
            //    entry.Property(e => e.AuthorID).IsModified = true;
            //    entry.Property(e => e.UniqueURL).IsModified = true;
            //    entry.Property(e => e.ImagePath).IsModified = true;
            //    entry.Property(e => e.ImageThumbnailPath).IsModified = true;

            //    db.SaveChanges();
            //}

            return;
        }

        public void Delete(int ID)
        {
            //using (var db = new SparkServerEntities())
            //{
            //    Blog toDelete = db.Blog.FirstOrDefault(u => u.ID == ID);

            //    if (toDelete == null)
            //        throw new Exception($"Could not find Blog with ID of {ID}");
            //    toDelete.Active = false;

            //    db.SaveChanges();
            //}

            return;
        }

        public Blog Get(int year, int month, string uniqueURL)
        {
            //// TODO: This probably needs to be finished (year, month are not being used!)

            //Blog item = null;

            //using (var db = new SparkServerEntities())
            //{
            //    item = db.Blog.FirstOrDefault(u => u.UniqueURL == uniqueURL);

            //    db.Entry(item).Reference(la => la.Author).Load();
            //    db.Entry(item).Collection(la => la.BlogsTags).Load();
            //}

            //return item;

            return new Blog();
        }

        public IEnumerable<Blog> GetByDate(int year, int? month)
        {
            List<Blog> blogList = new List<Blog>();

            using (var conn = new SqliteConnection(Database.SQLiteConnectionString))
            {
                SqliteCommand command = conn.CreateCommand();
                command.CommandText = @"
                    SELECT
	                    Blogs.*,
	                    Authors.ID AS 'AuthorID',
	                    Authors.FirstName || ' ' || Authors.LastName AS 'AuthorFullName'
                    FROM
                        Blogs
                        INNER JOIN Authors ON Authors.ID = Blogs.AuthorID
                    WHERE
	                    Blogs.Active = 1
	                    AND PublishDate <= datetime('now')
	                    AND PublishDate >= $startDate
	                    AND PublishDate <= $endDate
                    ORDER BY
                        PublishDate DESC";

                // TODO: build start/end dates based on month/year input params
                string startDate, endDate = string.Empty;

                if (month.HasValue)
                {
                    startDate = new DateTime(year, month.Value, 1).ToString(Formats.SQLiteDate);
                    endDate = new DateTime(year, month.Value, DateTime.DaysInMonth(year, month.Value)).ToString(Formats.SQLiteDate);
                }
                else
                {
                    startDate = new DateTime(year, 1, 1).ToString(Formats.SQLiteDate);
                    endDate = new DateTime(year, 12, DateTime.DaysInMonth(year, 12)).ToString(Formats.SQLiteDate);
                }

                command.Parameters.AddWithValue("$startDate", startDate);
                command.Parameters.AddWithValue("$endDate", endDate);
                conn.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        blogList.Add(new Blog()
                        {

                            ID = Database.GetID(reader["ID"]),
                            Title = Database.GetString(reader["Title"]),
                            Subtitle = Database.GetString(reader["Subtitle"]),
                            Content = Database.GetString(reader["Content"]),
                            ImagePath = Database.GetString(reader["ImagePath"]),
                            ImageThumbnailPath = Database.GetString(reader["ImageThumbnailPath"]),
                            Slug = Database.GetString(reader["Slug"]),
                            PublishDate = Database.GetDateTime(reader["PublishDate"]).Value,
                            AuthorID = Database.GetID(reader["AuthorID"]),
                            AuthorFullName = Database.GetString(reader["AuthorFullName"])
                        });
                    }
                }

                conn.Close();
            }

            return blogList;
        }

        public IEnumerable<Blog> GetRecent(int numberToLoad)
        {
            List<Blog> blogList = new List<Blog>();

            using (var conn = new SqliteConnection(Database.SQLiteConnectionString))
            {
                SqliteCommand command = conn.CreateCommand();
                command.CommandText = @"
                    SELECT
	                    Blogs.*,
	                    Authors.ID AS 'AuthorID',
	                    Authors.FirstName || ' ' || Authors.LastName AS 'AuthorFullName'
                    FROM
                        Blogs
                        INNER JOIN Authors ON Authors.ID = Blogs.AuthorID
                    WHERE
	                    Blogs.Active = 1
	                    AND PublishDate <= datetime('now')
                    ORDER BY
                        PublishDate DESC
                    LIMIT $limit";

                command.Parameters.AddWithValue("$limit", numberToLoad);
                conn.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        blogList.Add(new Blog() { 
                        
                            ID = Database.GetID(reader["ID"]),
                            Title = Database.GetString(reader["Title"]),
                            Subtitle = Database.GetString(reader["Subtitle"]),
                            Content = Database.GetString(reader["Content"]),
                            ImagePath = Database.GetString(reader["ImagePath"]),
                            ImageThumbnailPath = Database.GetString(reader["ImageThumbnailPath"]),
                            Slug = Database.GetString(reader["Slug"]),
                            PublishDate = Database.GetDateTime(reader["PublishDate"]).Value,
                            AuthorID = Database.GetID(reader["AuthorID"]),
                            AuthorFullName = Database.GetString(reader["AuthorFullName"])
                        });
                    }
                }

                conn.Close();
            }

            return blogList;
        }

        public IEnumerable<Blog> GetByTagID(int tagID)
        {
            //List<Blog> blogList = new List<Blog>();

            //using (var db = new SparkServerEntities())
            //{
            //    blogList = db.BlogsTags.Where(u => u.TagID == tagID)
            //                           .Select(p => p.Blog)
            //                           .Where(p => p.PublishDate <= DateTime.Now)
            //                           .Where(p => p.Active)
            //                           .Include(a => a.Author)
            //                           .Include(a => a.BlogsTags)
            //                           .ToList();
            //}

            //return blogList;

            return new List<Blog>();
        }
    }
}
