using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillFactoryCSharp25EntityFramework.Repositories
{
    public class BookRepository : BaseRepository
    {
        public BookRepository() : base() { }
        public BookRepository(AppContext appContext) : base(appContext) { }

        /// <summary>
        /// Выбор одной книги
        /// </summary>
        public Book GetBook(int bookId)
        {
            return DbContext.Books.SingleOrDefault(b => b.Id == bookId);
        }
        /// <summary>
        /// Выбор всех книг
        /// </summary>
        /// <returns></returns>
        public List<Book> GetAllBooks()
        {
            return DbContext.Books.ToList();
        }

        /// <summary>
        /// Добавление книги
        /// </summary>
        public int AddBook(string title, int yearOfPublishing)
        {
            DbContext.Books.Add(new Book { Title = title, YearOfPublishing = yearOfPublishing });
            return DbContext.SaveChanges();
        }
        /// <summary>
        /// Удаление книги
        /// </summary>
        public int DeleteBook(int bookId)
        {
            int result;
            using (var transactionContext = DbContext.Database.BeginTransaction())
            {
                Book bookToDelete = GetBook(bookId);
                if (bookToDelete.UserId is not null)
                {
                    DbContext.Users.SingleOrDefault(u => u.Id == bookToDelete.UserId)
                        .BooksBorrowed.Remove(bookToDelete);
                }
                DbContext.Books.Remove(bookToDelete);
                result = DbContext.SaveChanges();
                transactionContext.Commit();
            }
            return result;
        }

        /// <summary>
        /// Обновление года выпуска книги
        /// </summary>
        public int UpdateBookPublishYear(int bookId, int bookNewYearOfPublish)
        {
            Book bookToUpdate = GetBook(bookId);
            bookToUpdate.YearOfPublishing = bookNewYearOfPublish;
            return DbContext.SaveChanges();
        }

    }
}
