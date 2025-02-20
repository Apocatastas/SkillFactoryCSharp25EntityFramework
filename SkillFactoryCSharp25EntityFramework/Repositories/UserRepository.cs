using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillFactoryCSharp25EntityFramework.Repositories
{
    public class UserRepository : BaseRepository
    {
        public UserRepository() : base() { }
        public UserRepository(AppContext appContext) : base(appContext) { }
        /// <summary>
        /// Добавление пользователя
        /// </summary>
        public int AddUser(string userName, string email)
        {
            DbContext.Users.Add(new User { Name = userName, Email = email });
            return DbContext.SaveChanges();
        }
        /// <summary>
        /// Выбрать одного пользователя
        /// </summary>
        public User GetUser(int id)
        {
            return DbContext.Users.SingleOrDefault(x => x.Id == id);
        }
        /// <summary>
        /// Выбрать всех пользователей
        /// </summary>
        public List<User> GetAllUsers()
        {
            return DbContext.Users.ToList();
        }
        /// <summary>
        /// Обновление имени пользователя
        /// </summary>
        public int ChangeUserName(int userId, string userNewName)
        {
            User userToUpdate = GetUser(userId);
            userToUpdate.Name = userNewName;
            return DbContext.SaveChanges();
        }
        /// <summary>
        /// Выдать книгу пользователю
        /// </summary>
        public int AssignBookToUser(int userId, int bookId)
        {
            User userToUpdate = GetUser(userId);
            Book bookToUpdate = DbContext.Books.SingleOrDefault(b => b.Id == bookId);
            userToUpdate.BooksBorrowed.Add(bookToUpdate);
            return DbContext.SaveChanges();
        }
        /// <summary>
        /// Сдать книгу обратно
        /// </summary>
        public int UnAssignBookFromUser(int userId, int bookId)
        {
            User userToUpdate = GetUser(userId);
            Book bookToUpdate = DbContext.Books.SingleOrDefault(b => b.Id == bookId);
            userToUpdate.BooksBorrowed.Remove(bookToUpdate);
            return DbContext.SaveChanges();
        }
        /// <summary>
        /// Удалить пользователя
        /// </summary>
        public int DeleteUser(int userId)
        {
            int result;
            using (var transactionContext = DbContext.Database.BeginTransaction())
            {
                User userToDelete = GetUser(userId);
                if (userToDelete.BooksBorrowed is not null)
                {
                    foreach (Book book in userToDelete.BooksBorrowed)
                    {
                        book.UserId = null;
                    }
                    userToDelete.BooksBorrowed.Clear();
                }
                DbContext.Users.Remove(userToDelete);
                result = DbContext.SaveChanges();
                transactionContext.Commit();
            }
            return result;
        }

        /// <summary>
        /// 6. Получение количества книг на руках у пользователя.
        /// </summary>
        public int GetUserBorrowedBooksCount(int userId)
        {
            return GetUser(userId).BooksBorrowed.Count();
        }
    }
}