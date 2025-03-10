﻿using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillFactoryCSharp25EntityFramework.Repositories
{
    public class AuthorRepository : BaseRepository
    {
        public AuthorRepository() : base() { }
        public AuthorRepository(AppContext appContext) : base(appContext) { }

        public int AddAuthor(string firstName, string lastName)
        {
            DbContext.Authors.Add(new Author { FirstName = firstName, LastName = lastName });
            return DbContext.SaveChanges();
        }

        public Author GetAuthorById(int id)
        {
            return DbContext.Authors.SingleOrDefault(a => a.Id == id);
        }

        public List<Author> GetAllAuthors()
        {
            return DbContext.Authors.ToList();
        }

        public int UpdateAuthor(int authorId, string firstName, string lastName)
        {
            Author authorToUpdate = GetAuthorById(authorId);
            authorToUpdate.FirstName = firstName;
            authorToUpdate.LastName = lastName;
            return DbContext.SaveChanges();
        }

        public int AddBookToAuthor(int authorId, int bookId)
        {
            Author authorToUpdate = GetAuthorById(authorId);
            Book bookToUpdate = DbContext.Books.SingleOrDefault(b => b.Id == bookId);
            authorToUpdate.Books.Add(bookToUpdate);
            return DbContext.SaveChanges();
        }

        public int DeleteAuthor(int authorId)
        {
            int result;
            using (var transactionContext = DbContext.Database.BeginTransaction())
            {
                Author authorToDelete = GetAuthorById(authorId);
                if (authorToDelete.Books is not null)
                {
                    foreach (Book book in authorToDelete.Books)
                    {
                        book.Authors.Remove(authorToDelete);
                    }
                    authorToDelete.Books.Clear();
                }
                DbContext.Authors.Remove(authorToDelete);
                result = DbContext.SaveChanges();
                transactionContext.Commit();
            }
            return result;
        }
        /// <summary>
        /// 2. Получение количества книг определенного автора в библиотеке
        /// </summary>
        public int GetBooksCountByAuthor(int authorId)
        {
            return GetAuthorById(authorId).Books.Count();
        }
    }
}
