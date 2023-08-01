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
            Blog blog = new Blog();

            using (var conn = new SqliteConnection(Configuration.DatabaseConnectionString))
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
	                    AND Blogs.ID = $blogID
                    ORDER BY
	                    PublishDate DESC
                    LIMIT 1";

                command.Parameters.AddWithValue("$blogID", ID);
                conn.Open();

                using (var reader = command.ExecuteReader())
                {
                    reader.Read();

                    if (reader.HasRows)
                    {
                        blog.ID = Database.GetID(reader["ID"]);
                        blog.Title = Database.GetString(reader["Title"]);
                        blog.Subtitle = Database.GetString(reader["Subtitle"]);
                        blog.Content = Database.GetString(reader["Content"]);
                        blog.ImagePath = Database.GetString(reader["ImagePath"]);
                        blog.ImageThumbnailPath = Database.GetString(reader["ImageThumbnailPath"]);
                        blog.Slug = Database.GetString(reader["Slug"]);
                        blog.PublishDate = Database.GetDateTime(reader["PublishDate"]).Value;
                        blog.AuthorID = Database.GetID(reader["AuthorID"]);
                        blog.AuthorFullName = Database.GetString(reader["AuthorFullName"]);
                    }
                    else
                    {
                        conn.Close();
                        throw new Exception($"Blog not found with ID {ID.ToString()}");
                    }
                }

                conn.Close();
            }

            return blog;
        }

        public IEnumerable<Blog> GetAll()
        {
            List<Blog> blogList = new List<Blog>();

            using (var conn = new SqliteConnection(Configuration.DatabaseConnectionString))
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

        public Blog Get(int year, int month, string slug)
        {
            Blog blog = new Blog();

            using (var conn = new SqliteConnection(Configuration.DatabaseConnectionString))
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
	                    AND Slug = $slug
                    ORDER BY
                        PublishDate DESC ";

                command.Parameters.AddWithValue("$slug", slug);

                conn.Open();

                using (var reader = command.ExecuteReader())
                {
                    reader.Read();

                    if (reader.HasRows)
                    {
                        blog.ID = Database.GetID(reader["ID"]);
                        blog.Title = Database.GetString(reader["Title"]);
                        blog.Subtitle = Database.GetString(reader["Subtitle"]);
                        blog.Content = Database.GetString(reader["Content"]);
                        blog.ImagePath = Database.GetString(reader["ImagePath"]);
                        blog.ImageThumbnailPath = Database.GetString(reader["ImageThumbnailPath"]);
                        blog.Slug = Database.GetString(reader["Slug"]);
                        blog.PublishDate = Database.GetDateTime(reader["PublishDate"]).Value;
                        blog.AuthorID = Database.GetID(reader["AuthorID"]);
                        blog.AuthorFullName = Database.GetString(reader["AuthorFullName"]);
                    }
                    else
                    {
                        conn.Close();
                        throw new Exception($"No blog found for slug '{slug}'");
                    }
                }

                conn.Close();
            }

            return blog;
        }

        public IEnumerable<Blog> GetByDate(int year, int? month)
        {
            List<Blog> blogList = new List<Blog>();

            using (var conn = new SqliteConnection(Configuration.DatabaseConnectionString))
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
                        PublishDate DESC ";

                // Build start/end dates based on month/year input params
                string startDate, endDate = string.Empty;

                if (month.HasValue)
                {
                    startDate = new DateTime(year, month.Value, 1).ToString(FormatHelper.SQLiteDate);
                    endDate = new DateTime(year, month.Value, DateTime.DaysInMonth(year, month.Value)).ToString(FormatHelper.SQLiteDate);
                }
                else
                {
                    startDate = new DateTime(year, 1, 1).ToString(FormatHelper.SQLiteDate);
                    endDate = new DateTime(year, 12, DateTime.DaysInMonth(year, 12)).ToString(FormatHelper.SQLiteDate);
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

            using (var conn = new SqliteConnection(Configuration.DatabaseConnectionString))
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
            List<Blog> blogList = new List<Blog>();

            using (var conn = new SqliteConnection(Configuration.DatabaseConnectionString))
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
	                    LEFT JOIN BlogsToTags ON BlogsToTags.BlogID = Blogs.ID
                    WHERE
	                    Blogs.Active = 1
	                    AND PublishDate <= datetime('now')
	                    AND BlogsToTags.BlogTagID = $tagID
                    ORDER BY
	                    PublishDate DESC";

                command.Parameters.AddWithValue("$tagID", tagID);
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

        public IEnumerable<Blog> GetByTagName(string tagName)
        {
            List<Blog> blogList = new List<Blog>();

            using (var conn = new SqliteConnection(Configuration.DatabaseConnectionString))
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
	                    LEFT JOIN BlogsToTags ON BlogsToTags.BlogID = Blogs.ID
	                    LEFT JOIN BlogTags ON BlogTags.ID = BlogsToTags.BlogTagID
                    WHERE
	                    Blogs.Active = 1
	                    AND PublishDate <= datetime('now')
	                    AND BlogTags.Name = $tagName
                    ORDER BY
	                    PublishDate DESC";

                command.Parameters.AddWithValue("$tagName", tagName);
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
    }
}
