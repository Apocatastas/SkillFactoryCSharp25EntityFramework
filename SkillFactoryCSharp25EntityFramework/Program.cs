using SkillFactoryCSharp25EntityFramework.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillFactoryCSharp25EntityFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new AppContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
                FillTheBase(db);
                Demo(db);
                db.SaveChanges();
            }
        }

        static void FillTheBase(AppContext db)
        {
            db.Authors.AddRange(
                                new Author { FirstName = "", LastName = "" },
                                new Author { FirstName = "", LastName = "" },
                                new Author { FirstName = "", LastName = "" },
                                new Author { FirstName = "", LastName = "" },
                                new Author { FirstName = "", LastName = "" }
                               );
            db.Users.AddRange(
                                new User {Name = "", Email = "" },
                                new User { Name = "", Email = "" },
                                new User { Name = "", Email = "" },
                                new User { Name = "", Email = "" },
                                new User { Name = "", Email = "" }
                             );
            db.Genres.AddRange(
                                new Genre { Name = "" },
                                new Genre { Name = "" },
                                new Genre { Name = "" },
                                new Genre { Name = "" },
                                new Genre { Name = "" }
                              );
            db.Books.AddRange(
                                new Book { Title = "", YearOfPublishing = 0 },
                                new Book { Title = "", YearOfPublishing = 0 },
                                new Book { Title = "", YearOfPublishing = 0 },
                                new Book { Title = "", YearOfPublishing = 0 },
                                new Book { Title = "", YearOfPublishing = 0 },
                                new Book { Title = "", YearOfPublishing = 0 },
                                new Book { Title = "", YearOfPublishing = 0 },
                                new Book { Title = "", YearOfPublishing = 0 }
                             );
            db.SaveChanges();
            
            //теперь присвоим книжкам жанры и авторов, заодно продемонстрируем CRUD:
            // - выберем объекты по идентификатору
            // - выберем все объекты сразу
            // - добавим жанр в базу и тут же его удалим
            // - переименуем третьего пользователя
            // - изменим год для пятой книги
            // - выдадим её на руки третьему пользователю

        }

        static void Demo(AppContext db)
        {
            //Здесь продемонстрируем девять заданий из 25.5.4
            //1. Получать список книг определенного жанра и вышедших между определенными годами.
            //2. Получать количество книг определенного автора в библиотеке.
            //3. Получать количество книг определенного жанра в библиотеке.
            //4. Получать булевый флаг о том, есть ли книга определенного автора и с определенным названием в библиотеке.
            //5. Получать булевый флаг о том, есть ли определенная книга на руках у пользователя.
            //6. Получать количество книг на руках у пользователя.
            //7. Получение последней вышедшей книги.
            //8. Получение списка всех книг, отсортированного в алфавитном порядке по названию.
            //9. Получение списка всех книг, отсортированного в порядке убывания года их выхода.
        }

    }
}