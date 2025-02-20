using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        public int SetAuthor(int bookID, Author author)
        {
            int result;
            using (var transactionContext = DbContext.Database.BeginTransaction())
            {
                Book bookToUpdate = GetBook(bookID);
                if (bookToUpdate.Authors is null) { bookToUpdate.Authors = new List<Author>(); }
                bookToUpdate.Authors.Add(author);
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

        /// <summary>
        /// 1. Получение списка книг определенного жанра и вышедших между определенными годами.
        /// </summary>
        public List<Book> GetBooksByGenreIssueDate(int? genreId = null, int? after = null, int? before = null)
        {
            var query = from book in DbContext.Books
                        where ((genreId == null) || (book.GenreId == genreId))
                        && ((after == null) || (book.YearOfPublishing >= after))
                        && ((before == null) || (book.YearOfPublishing <= before))
                        select book;
            return query.ToList();
        }

        /// <summary>
        /// 3. Получение количества книг определенного жанра в библиотеке.
        /// </summary>
        public int GetBooksCountByGenre(int genreId)
        {
            var query = from book in DbContext.Books
                        where book.GenreId == genreId
                        select book;
            return query.Count();
        }

        /// <summary>
        /// 4. Получение булевого флага о том, есть ли книга определенного автора и с определенным названием в библиотеке.
        /// </summary>
        public bool CheckBook(string? authorsLastname = null, string? bookTitle = null)
        {
            var query = from book in DbContext.Books
                        where ((authorsLastname == null) || (book.Authors.Any(a => a.LastName == authorsLastname)))
                        && ((bookTitle == null) || (book.Title == bookTitle))
                        select book;
            return query.Any();
        }

        /// <summary>
        /// 5. Получение булевого флага о том, есть ли определенная книга на руках у пользователя.
        /// </summary>
        public bool IsBookBorrowed(int bookId)
        {
            return GetBook(bookId).BorrowedBy is not null;
        }

        /// <summary>
        /// 7. Получение последней вышедшей книги.
        /// </summary>
        public Book GetNewestBook()
        {
            var query = from Book book in DbContext.Books
                        orderby book.YearOfPublishing descending, book.Id descending
                        select book;
            return query.First();
        }

        /// <summary>
        /// 8. Получение списка всех книг, отсортированного в алфавитном порядке по названию.
        /// </summary>
        public List<Book> GetAllBooksOrderedByTitle()
        {
            var query = from Book book in DbContext.Books
                        orderby book.Title
                        select book;
            return query.ToList();
        }

        /// <summary>
        /// 9. Получение списка всех книг, отсортированного в порядке убывания года их выхода.
        /// </summary>
        public List<Book> GetAllBooksOrderedByYearFromNewest()
        {
            var query = from Book book in DbContext.Books
                        orderby book.YearOfPublishing descending
                        select book;
            return query.ToList();
        }

    }
}
