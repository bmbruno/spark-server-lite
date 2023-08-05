using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using SparkServerLite.Infrastructure;
using SparkServerLite.Interfaces;
using SparkServerLite.Models;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Linq;

namespace SparkServer.Infrastructure.Repositories
{
    public class BlogTagRepository : IBlogTagRepository<BlogTag>
    {
        public BlogTag Get(int ID)
        {
            BlogTag tag = new BlogTag();

            using (var conn = new SqliteConnection(Configuration.DatabaseConnectionString))
            {
                SqliteCommand command = conn.CreateCommand();
                command.CommandText = @"
                    SELECT
	                    BlogTags.ID,
	                    BlogTags.Name
                    FROM
	                    BlogTags
                    WHERE
	                    Active = 1
                        AND ID = $id";

                command.Parameters.AddWithValue("$id", ID);
                conn.Open();

                using (var reader = command.ExecuteReader())
                {
                    reader.Read();

                    if (reader.HasRows)
                    {
                        tag.ID = Database.GetID(reader["ID"]);
                        tag.Name = Database.GetString(reader["Name"]);
                    }
                    else
                    {
                        conn.Close();
                        throw new Exception($"No blog tag found for ID {ID.ToString()}");
                    }
                }

                conn.Close();
            }

            return tag;

        }

        public IEnumerable<BlogTag> GetAll()
        {
            List<BlogTag> tagList = new List<BlogTag>();

            using (var conn = new SqliteConnection(Configuration.DatabaseConnectionString))
            {
                SqliteCommand command = conn.CreateCommand();
                command.CommandText = @"
                    SELECT
	                    BlogTags.ID,
	                    BlogTags.Name
                    FROM
	                    BlogTags
                    WHERE
	                    BlogTags.Active = 1
                    ORDER BY
                        BlogTags.Name ASC";

                conn.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tagList.Add(new BlogTag()
                        {
                            ID = Database.GetID(reader["ID"]),
                            Name = Database.GetString(reader["Name"])
                        });
                    }
                }

                conn.Close();
            }

            return tagList;
        }

        public int Create(BlogTag newItem)
        {
            long newID = 0;

            using (var conn = new SqliteConnection(Configuration.DatabaseConnectionString))
            {
                conn.Open();

                SqliteCommand command = conn.CreateCommand();

                // Initial insert of minimum required data
                command.CommandText = @"INSERT INTO Blogs (Name) VALUES ($name);";
                command.Parameters.AddWithValue("$title", newItem.Name);
                command.ExecuteNonQuery();
                command.Parameters.Clear();

                command.CommandText = "SELECT last_insert_rowid()";
                newID = (long)command.ExecuteScalar();
            }

            return Convert.ToInt32(newID);

        }

        public void Update(BlogTag updateItem)
        {
            using (var conn = new SqliteConnection(Configuration.DatabaseConnectionString))
            {
                conn.Open();

                SqliteCommand command = conn.CreateCommand();
                command.CommandText = "UPDATE BlogTags SET Name = $name WHERE ID = $id;";
                command.Parameters.AddWithValue("$name", updateItem.Name);
                command.Parameters.AddWithValue("$id", updateItem.ID);
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
                    UPDATE BlogTags
                    SET Active = 0
                    WHERE ID = $tagID";

                command.Parameters.AddWithValue("$tagID", ID);
                command.ExecuteNonQuery();
                conn.Close();
            }

            return;
        }

        public bool Exists(string name)
        {
            bool tagExists = false;

            using (var conn = new SqliteConnection(Configuration.DatabaseConnectionString))
            {
                SqliteCommand command = conn.CreateCommand();
                command.CommandText = @"
                    SELECT ID
                    FROM BlogTags
                    WHERE
                        Active = 1
                        AND LOWER(Name) = $slug";

                command.Parameters.AddWithValue("$slug", name.ToLower());

                conn.Open();

                using (var reader = command.ExecuteReader())
                {
                    reader.Read();
                    tagExists = reader.HasRows;
                }

                conn.Close();
            }

            return tagExists;
        }

        public IEnumerable<BlogTag> GetTagsInUse()
        {
            List<BlogTag> tagList = new List<BlogTag>();

            using (var conn = new SqliteConnection(Configuration.DatabaseConnectionString))
            {
                SqliteCommand command = conn.CreateCommand();
                command.CommandText = @"
                    SELECT
	                    BlogTags.ID,
	                    BlogTags.Name
                    FROM
	                    BlogTags
	                    INNER JOIN BlogsToTags ON BlogsToTags.BlogTagID = BlogTags.ID
                    WHERE
	                    BlogTags.Active = 1
                    ORDER BY
                        BlogTags.Name ASC";

                conn.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tagList.Add(new BlogTag()
                        {
                            ID = Database.GetID(reader["ID"]),
                            Name = Database.GetString(reader["Name"])
                        });
                    }
                }

                conn.Close();
            }

            return tagList;
        }

        public IEnumerable<BlogTag> GetForBlog(int blogID)
        {
            List<BlogTag> tagList = new List<BlogTag>();

            using (var conn = new SqliteConnection(Configuration.DatabaseConnectionString))
            {
                SqliteCommand command = conn.CreateCommand();
                command.CommandText = @"
                    SELECT
	                    BlogTags.ID,
                        BlogTags.Name
                    FROM
	                    BlogTags
	                    INNER JOIN BlogsToTags ON BlogsToTags.BlogTagID = BlogTags.ID
                    WHERE
	                    BlogTags.Active = 1
	                    AND BlogsToTags.BlogID = $blogID";

                command.Parameters.AddWithValue("$blogID", blogID);

                conn.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tagList.Add(new BlogTag()
                        {
                            ID = Database.GetID(reader["ID"]),
                            Name = Database.GetString(reader["Name"])
                        });
                    }
                }

                conn.Close();
            }

            return tagList;
        }

        public void UpdateTagsForBlog(int blogID, IEnumerable<int> updatedList)
        {
            //using (var db = new SparkServerEntities())
            //{
            //    var oldTags = db.BlogsTags.Where(u => u.BlogID == blogID);

            //    if (oldTags != null)
            //        db.BlogsTags.RemoveRange(oldTags);

            //    DateTime createDate = DateTime.Now;

            //    foreach (var newID in updatedList)
            //    {
            //        BlogsTags newTag = new BlogsTags()
            //        {
            //            BlogID = blogID,
            //            TagID = newID,
            //            Active = true,
            //            CreateDate = createDate
            //        };

            //        db.BlogsTags.Add(newTag);
            //    }

            //    db.SaveChanges();
            //}

            return;
        }

    }
}