﻿using Microsoft.Data.Sqlite;
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
            List<Blog> blogList = new List<Blog>();
            long newID = 0;

            using (var conn = new SqliteConnection(Configuration.DatabaseConnectionString))
            {
                conn.Open();

                SqliteCommand command = conn.CreateCommand();

                // Initial insert of minimum required data
                command.CommandText = @"INSERT INTO Blogs (Title, PublishDate, Slug, AuthorID) VALUES ($title, $publishDate, $slug, $authorID);";
                command.Parameters.AddWithValue("$title", newItem.Title);
                command.Parameters.AddWithValue("$publishDate", newItem.PublishDate.ToString(FormatHelper.SQLiteDate));
                command.Parameters.AddWithValue("$slug", newItem.Slug);
                command.Parameters.AddWithValue("$authorID", newItem.AuthorID);
                command.ExecuteNonQuery();
                command.Parameters.Clear();

                command.CommandText = "SELECT last_insert_rowid()";
                newID = (long)command.ExecuteScalar();
                
                // Updates of various fields
                StringBuilder updateSQL = new StringBuilder();
                bool needsUpdate = false;

                if (!String.IsNullOrEmpty(newItem.Subtitle))
                {
                    updateSQL.Append("UPDATE Blogs SET Subtitle = $subtitle WHERE ID = $id;");
                    command.Parameters.AddWithValue("$subtitle", newItem.Subtitle);
                    needsUpdate = true;
                }

                if (!String.IsNullOrEmpty(newItem.Content))
                {
                    updateSQL.Append("UPDATE Blogs SET Content = $content WHERE ID = $id;");
                    command.Parameters.AddWithValue("$content", newItem.Content);
                    needsUpdate = true;
                }

                if (!String.IsNullOrEmpty(newItem.ImagePath))
                {
                    updateSQL.Append("UPDATE Blogs SET ImagePath = $imagePath WHERE ID = $id;");
                    command.Parameters.AddWithValue("$imagePath", newItem.ImagePath);
                    needsUpdate = true;
                }

                if (!String.IsNullOrEmpty(newItem.ImageThumbnailPath))
                {
                    updateSQL.Append("UPDATE Blogs SET ImageThumbnailPath = $imageThumbPath WHERE ID = $id;");
                    command.Parameters.AddWithValue("$imageThumbPath", newItem.ImageThumbnailPath);
                    needsUpdate = true;
                }

                if (needsUpdate)
                {
                    command.CommandText = updateSQL.ToString();
                    command.Parameters.AddWithValue("$id", newID);
                    command.ExecuteNonQuery();
                }

                conn.Close();
            }

            return Convert.ToInt32(newID);
        }

        public void Update(Blog updateItem)
        {
            using (var conn = new SqliteConnection(Configuration.DatabaseConnectionString))
            {
                conn.Open();

                SqliteCommand command = conn.CreateCommand();
                command.CommandText = @"
                    UPDATE Blogs
                    SET
	                    Title = $title,
	                    Subtitle = $subtitle,
	                    Content = $content,
	                    Slug = $slug,
	                    PublishDate = $publishDate,
	                    ImagePath = $imagePath,
	                    ImageThumbnailPath = $imageThumbnailPath,
	                    AuthorID = $authorID
                    WHERE ID = $blogID";

                command.Parameters.AddWithValue("$title", updateItem.Title);
                command.Parameters.AddWithValue("$subtitle", updateItem.Subtitle);
                command.Parameters.AddWithValue("$content", updateItem.Content);
                command.Parameters.AddWithValue("$slug", updateItem.Slug);
                command.Parameters.AddWithValue("$publishDate", updateItem.PublishDate.ToString(FormatHelper.SQLiteDate));
                command.Parameters.AddWithValue("$imagePath", updateItem.ImagePath);
                command.Parameters.AddWithValue("$imageThumbnailPath", updateItem.ImageThumbnailPath);
                command.Parameters.AddWithValue("$authorID", updateItem.AuthorID);
                command.Parameters.AddWithValue("$blogID", updateItem.ID);

                command.ExecuteNonQuery();

                conn.Close();
            }

            return;
        }

        public void Delete(int ID)
        {
            using (var conn = new SqliteConnection(Configuration.DatabaseConnectionString))
            {
                conn.Open();

                SqliteCommand command = conn.CreateCommand();
                command.CommandText = @"
                    UPDATE Blogs
                    SET Active = 0
                    WHERE ID = $blogID";

                command.Parameters.AddWithValue("$blogID", ID);
                command.ExecuteNonQuery();
                conn.Close();
            }

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

        // TODO: create check for pre-existing Blog based on slug
    }
}
