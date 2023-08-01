using SparkServerLite.Interfaces;
using SparkServerLite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using SparkServerLite.Infrastructure;
using System.Security.Cryptography;

namespace SparkServer.Infrastructure.Repositories
{
    public class AuthorRepository : IAuthorRepository<Author>
    {
        public Author Get(int ID)
        {
            Author author = new Author();

            using (var conn = new SqliteConnection(Configuration.DatabaseConnectionString))
            {
                SqliteCommand command = conn.CreateCommand();
                command.CommandText = @"
                    SELECT *
                    FROM Authors
                    WHERE
                        ID = $id
                        AND Active = 1
                    LIMIT 1";

                command.Parameters.AddWithValue("$id", ID);
                conn.Open();

                using (var reader = command.ExecuteReader())
                {
                    reader.Read();

                    if (reader.HasRows)
                    {
                        author.ID = Database.GetID(reader["ID"]);
                        author.SSOID = Database.GetGuid(reader["SSOID"]);
                        author.FirstName = Database.GetString(reader["FirstName"]);
                        author.LastName = Database.GetString(reader["LastName"]);
                        author.Email = Database.GetString(reader["Email"]);
                        author.Active = Database.GetBoolean(reader["Active"]);
                        author.CreateDate = Database.GetDateTime(reader["CreateDate"]);
                    }
                    else
                    {
                        throw new Exception($"No Author found for ID {ID}");
                    }
                }

                conn.Close();
            }

            return author;
        }

        public Author Get(Guid ssoID)
        {
            Author author = new Author();

            using (var conn = new SqliteConnection(Configuration.DatabaseConnectionString))
            {
                SqliteCommand command = conn.CreateCommand();
                command.CommandText = @"
                    SELECT *
                    FROM Authors
                    WHERE
                        SSOID = $ssoid
                        AND Active = 1
                    LIMIT 1";

                command.Parameters.AddWithValue("$ssoid", ssoID.ToString());
                conn.Open();

                using (var reader = command.ExecuteReader())
                {
                    reader.Read();

                    if (reader.HasRows)
                    {
                        author.ID = Database.GetID(reader["ID"]);
                        author.SSOID = Database.GetGuid(reader["SSOID"]);
                        author.FirstName = Database.GetString(reader["FirstName"]);
                        author.LastName = Database.GetString(reader["LastName"]);
                        author.Email = Database.GetString(reader["Email"]);
                        author.Active = Database.GetBoolean(reader["Active"]);
                        author.CreateDate = Database.GetDateTime(reader["CreateDate"]);
                    }
                    else
                    {
                        throw new Exception($"No Author found for SSOID '{ssoID.ToString()}'");
                    }
                }

                conn.Close();
            }

            return author;
        }

        public IEnumerable<Author> GetAll()
        {
            List<Author> authors = new List<Author>();

            using (var conn = new SqliteConnection(Configuration.DatabaseConnectionString))
            {
                SqliteCommand command = conn.CreateCommand();
                command.CommandText = @"
                    SELECT *
                    FROM Authors
                    WHERE
                        Active = 1";

                conn.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        authors.Add(new Author() {
                            ID = Database.GetID(reader["ID"]),
                            SSOID = Database.GetGuid(reader["SSOID"]),
                            FirstName = Database.GetString(reader["FirstName"]),
                            LastName = Database.GetString(reader["LastName"]),
                            Email = Database.GetString(reader["Email"]),
                            Active = Database.GetBoolean(reader["Active"]),
                            CreateDate = Database.GetDateTime(reader["CreateDate"])
                        });
                    }
                }

                conn.Close();
            }

            return authors;
        }

        public int Create(Author newItem)
        {
            //using (var db = new SparkServerEntities())
            //{
            //    db.Author.Add(newItem);
            //    db.SaveChanges();
            //}

            //return newItem.ID;

            return 0;
        }

        public void Update(Author updateItem)
        {
            //using (var db = new SparkServerEntities())
            //{
            //    Author toUpdate = db.Author.FirstOrDefault(u => u.ID == updateItem.ID);

            //    if (toUpdate == null)
            //        throw new Exception($"Could not find Author with ID of {updateItem.ID}");

            //    toUpdate = updateItem;
            //    db.Entry(toUpdate).State = System.Data.Entity.EntityState.Modified;
            //    db.SaveChanges();
            //}

            return;
        }

        public void Delete(int ID)
        {
            //using (var db = new SparkServerEntities())
            //{
            //    Author toDelete = db.Author.FirstOrDefault(u => u.ID == ID);

            //    if (toDelete == null)
            //        throw new Exception($"Could not find Author with ID of {ID}");

            //    toDelete.Active = false;

            //    db.SaveChanges();
            //}

            return;
        }
    }
}
