﻿using SparkServerLite.Interfaces;
using SparkServerLite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace SparkServer.Infrastructure.Repositories
{
    public class AuthorRepository : IAuthorRepository<Author>
    {
        public Author Get(int ID)
        {
            //Author item;

            //using (var db = new SparkServerEntities())
            //{
            //    item = db.Author.FirstOrDefault(u => u.ID == ID);
            //}

            //return item;

            string? firstName = string.Empty;

            using (var conn = new SqliteConnection("Data Source=SparkServer.db"))
            {
                conn.Open();

                SqliteCommand command = conn.CreateCommand();
                command.CommandText = @"
                    SELECT *
                    FROM Authors
                    WHERE ID = $id";

                command.Parameters.AddWithValue("$id", ID);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        firstName = reader["FirstName"].ToString();
                    }
                }

                conn.Close();
            }


            return new Author() { FirstName = firstName ?? "NO NAME" };
        }

        public Author Get(Guid ssoID)
        {
            //Author item;

            //using (var db = new SparkServerEntities())
            //{
            //    item = db.Author.FirstOrDefault(u => u.SSOID == ssoID);
            //}

            //return item;

            return new Author();
        }

        public IEnumerable<Author> Get(Expression<Func<Author, bool>> whereClause)
        {
            // CALLING: ArticleRepo.Get(x => x.Title == "abcdef");
            // USING: db.Articles.Where(whereClause);

            //List<Author> results;

            //using (var db = new SparkServerEntities())
            //{
            //    results = db.Author.Where(whereClause).ToList();
            //}

            //return results;

            return new List<Author>();
        }

        public IEnumerable<Author> GetAll()
        {
            //List<Author> results;

            //using (var db = new SparkServerEntities())
            //{
            //    results = db.Author.ToList();
            //}

            //return results;

            return new List<Author>();
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
