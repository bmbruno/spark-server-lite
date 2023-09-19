using Microsoft.Data.Sqlite;
using SparkServerLite.Infrastructure;
using SparkServerLite.Interfaces;
using SparkServerLite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace SparkServer.Infrastructure.Repositories
{
    public class BlogRepository : IBlogRepository<Blog>
    {
        private readonly IAppSettings _settings;

        public BlogRepository(IAppSettings settings)
        {
            _settings = settings;
        }

        public Blog Get(int ID)
        {
            Blog blog = new Blog();

            using (var conn = new SqliteConnection(_settings.DatabaseConnectionString))
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
                        blog.Markdown = Database.GetString(reader["Markdown"]);
                        blog.Content = Database.GetString(reader["Content"]);
                        blog.MediaFolder = Database.GetString(reader["MediaFolder"]);
                        blog.ImagePath = Database.GetString(reader["ImagePath"]);
                        blog.ImageThumbnailPath = Database.GetString(reader["ImageThumbnailPath"]);
                        blog.Slug = Database.GetString(reader["Slug"]);
                        blog.PublishDate = Database.GetDateTime(reader["PublishDate"]).Value;
                        blog.AuthorID = Database.GetID(reader["AuthorID"]);
                        blog.AuthorFullName = Database.GetString(reader["AuthorFullName"]);
                        blog.CreateDate = Database.GetDateTime(reader["CreateDate"]).Value;
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

            using (var conn = new SqliteConnection(_settings.DatabaseConnectionString))
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
	                    -- AND PublishDate <= datetime('now')
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
                            Markdown = Database.GetString(reader["Markdown"]),
                            Content = Database.GetString(reader["Content"]),
                            ImagePath = Database.GetString(reader["ImagePath"]),
                            ImageThumbnailPath = Database.GetString(reader["ImageThumbnailPath"]),
                            Slug = Database.GetString(reader["Slug"]),
                            PublishDate = Database.GetDateTime(reader["PublishDate"]).Value,
                            AuthorID = Database.GetID(reader["AuthorID"]),
                            AuthorFullName = Database.GetString(reader["AuthorFullName"]),
                            CreateDate = Database.GetDateTime(reader["CreateDate"]).Value
                    });
                    }
                }

                conn.Close();
            }

            return blogList;
        }

        public int Create(Blog newItem)
        {
            long newID = 0;

            using (var conn = new SqliteConnection(_settings.DatabaseConnectionString))
            {
                conn.Open();

                SqliteCommand command = conn.CreateCommand();

                // Initial insert of minimum required data
                command.CommandText = @"INSERT INTO Blogs (Title, PublishDate, Slug, AuthorID, CreateDate) VALUES ($title, $publishDate, $slug, $authorID, $createDate);";
                command.Parameters.AddWithValue("$title", newItem.Title);
                command.Parameters.AddWithValue("$publishDate", newItem.PublishDate.ToString(FormatHelper.SQLiteDateTime));
                command.Parameters.AddWithValue("$slug", newItem.Slug);
                command.Parameters.AddWithValue("$authorID", newItem.AuthorID);
                command.Parameters.AddWithValue("$createDate", newItem.CreateDate.ToString(FormatHelper.SQLiteDateTime));
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

                if (!String.IsNullOrEmpty(newItem.Markdown))
                {
                    updateSQL.Append("UPDATE Blogs SET Markdown = $markdown WHERE ID = $id;");
                    command.Parameters.AddWithValue("$markdown", newItem.Markdown);
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
            using (var conn = new SqliteConnection(_settings.DatabaseConnectionString))
            {
                conn.Open();

                SqliteCommand command = conn.CreateCommand();
                StringBuilder updateSQL = new StringBuilder();

                // Title - required
                updateSQL.Append("UPDATE Blogs SET Title = $title WHERE ID = $id;");
                command.Parameters.AddWithValue("$title", updateItem.Title);

                // Publish Date - required
                updateSQL.Append("UPDATE Blogs SET PublishDate = $imageThumbPath WHERE ID = $id;");
                command.Parameters.AddWithValue("$imageThumbPath", updateItem.PublishDate);

                // Markdown - required
                updateSQL.Append("UPDATE Blogs SET Markdown = $markdown WHERE ID = $id;");
                command.Parameters.AddWithValue("$markdown", updateItem.Markdown);

                // Content - required
                updateSQL.Append("UPDATE Blogs SET Content = $content WHERE ID = $id;");
                command.Parameters.AddWithValue("$content", updateItem.Content);

                // Author - required
                updateSQL.Append("UPDATE Blogs SET AuthorID = $author WHERE ID = $id;");
                command.Parameters.AddWithValue("$author", updateItem.AuthorID);

                // Modified Date - system level field
                updateSQL.Append("UPDATE Blogs SET ModifiedDate = $today WHERE ID = $id;");
                command.Parameters.AddWithValue("$today", DateTime.Now.ToString(FormatHelper.SQLiteDateTime));

                if (!String.IsNullOrEmpty(updateItem.Subtitle))
                {
                    updateSQL.Append("UPDATE Blogs SET Subtitle = $subtitle WHERE ID = $id;");
                    command.Parameters.AddWithValue("$subtitle", updateItem.Subtitle);
                }

                if (!String.IsNullOrEmpty(updateItem.Slug))
                {
                    updateSQL.Append("UPDATE Blogs SET Slug = $slug WHERE ID = $id;");
                    command.Parameters.AddWithValue("$slug", updateItem.Slug);
                }

                if (!String.IsNullOrEmpty(updateItem.MediaFolder))
                {
                    updateSQL.Append("UPDATE Blogs SET MediaFolder = $mediaFolder WHERE ID = $id;");
                    command.Parameters.AddWithValue("$mediaFolder", updateItem.MediaFolder);
                }

                if (!String.IsNullOrEmpty(updateItem.ImagePath))
                {
                    updateSQL.Append("UPDATE Blogs SET ImagePath = $imagePath WHERE ID = $id;");
                    command.Parameters.AddWithValue("$imagePath", updateItem.ImagePath);
                }

                if (!String.IsNullOrEmpty(updateItem.ImageThumbnailPath))
                {
                    updateSQL.Append("UPDATE Blogs SET ImageThumbnailPath = @imageThumbPath WHERE ID = $id;");
                    command.Parameters.AddWithValue("@imageThumbPath", updateItem.ImageThumbnailPath);
                }

                command.CommandText = updateSQL.ToString();
                command.Parameters.AddWithValue("$id", updateItem.ID);
                command.ExecuteNonQuery();

                conn.Close();
            }

            return;
        }

        public void Delete(int ID)
        {
            using (var conn = new SqliteConnection(_settings.DatabaseConnectionString))
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

            using (var conn = new SqliteConnection(_settings.DatabaseConnectionString))
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
                        blog.Markdown = Database.GetString(reader["Markdown"]);
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

        public IEnumerable<Blog> GetAllPublished()
        {
            List<Blog> blogList = new List<Blog>();

            using (var conn = new SqliteConnection(_settings.DatabaseConnectionString))
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
                            Markdown = Database.GetString(reader["Markdown"]),
                            Content = Database.GetString(reader["Content"]),
                            ImagePath = Database.GetString(reader["ImagePath"]),
                            ImageThumbnailPath = Database.GetString(reader["ImageThumbnailPath"]),
                            Slug = Database.GetString(reader["Slug"]),
                            PublishDate = Database.GetDateTime(reader["PublishDate"]).Value,
                            AuthorID = Database.GetID(reader["AuthorID"]),
                            AuthorFullName = Database.GetString(reader["AuthorFullName"]),
                            CreateDate = Database.GetDateTime(reader["CreateDate"]).Value
                        });
                    }
                }

                conn.Close();
            }

            return blogList;
        }

        public IEnumerable<Blog> GetByDate(int year, int? month)
        {
            List<Blog> blogList = new List<Blog>();

            using (var conn = new SqliteConnection(_settings.DatabaseConnectionString))
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
                            AuthorFullName = Database.GetString(reader["AuthorFullName"]),
                            CreateDate = Database.GetDateTime(reader["CreateDate"]).Value
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

            using (var conn = new SqliteConnection(_settings.DatabaseConnectionString))
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
                            AuthorFullName = Database.GetString(reader["AuthorFullName"]),
                            CreateDate = Database.GetDateTime(reader["CreateDate"]).Value
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

            using (var conn = new SqliteConnection(_settings.DatabaseConnectionString))
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
                            AuthorFullName = Database.GetString(reader["AuthorFullName"]),
                            CreateDate = Database.GetDateTime(reader["CreateDate"]).Value
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

            using (var conn = new SqliteConnection(_settings.DatabaseConnectionString))
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
                            AuthorFullName = Database.GetString(reader["AuthorFullName"]),
                            CreateDate = Database.GetDateTime(reader["CreateDate"]).Value
                        });
                    }
                }

                conn.Close();
            }

            return blogList;
        }

        public bool SlugExists(string slug)
        {
            bool slugExists = false;

            using (var conn = new SqliteConnection(_settings.DatabaseConnectionString))
            {
                SqliteCommand command = conn.CreateCommand();
                command.CommandText = @"
                    SELECT ID
                    FROM Blogs
                    WHERE
                        Active = 1
                        AND Slug = $slug";

                command.Parameters.AddWithValue("$slug", slug);

                conn.Open();

                using (var reader = command.ExecuteReader())
                {
                    reader.Read();
                    slugExists = reader.HasRows;
                }

                conn.Close();
            }

            return slugExists;
        }

        public string GetLatestBlogBanner(string folderFragment)
        {
            Blog blog = new Blog();
            string latestBlogBanner = string.Empty;

            using (var conn = new SqliteConnection(_settings.DatabaseConnectionString))
            {
                SqliteCommand command = conn.CreateCommand();
                command.CommandText = @"
                    SELECT ImagePath
                    FROM Blogs
                    WHERE
	                    Active = 1
	                    AND ImagePath LIKE $fragment
                    ORDER BY PublishDate DESC
                    LIMIT 1";

                command.Parameters.AddWithValue("$fragment", $"{folderFragment}%");
                conn.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        string imagePath = Database.GetString(reader["ImagePath"]);

                        if (!String.IsNullOrEmpty(imagePath))
                            latestBlogBanner = Path.GetFileName(imagePath);
                    }
                    else
                    {
                        conn.Close();
                        throw new Exception($"No rows returned for latest blog hero query.");
                    }
                }

                conn.Close();
            }

            return latestBlogBanner;
        }
    }
}