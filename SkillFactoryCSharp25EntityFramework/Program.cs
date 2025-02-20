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
                Console.WriteLine("Databases created, starting the process of initial filling...");
                FillTheBase(db);
                CRUDDemo(db);
                OperationsDemo(db);
                db.SaveChanges();
            }
        }

        static void FillTheBase(AppContext db)
        {
            db.Authors.AddRange(
                                new Author { FirstName = "Фёдор", LastName = "Двинятин" },
                                new Author { FirstName = "Ланочка", LastName = "Дельрей" },
                                new Author { FirstName = "Вазипупий", LastName = "Козлов" },
                                new Author { FirstName = "Элджей", LastName = "Моргенштерн" },
                                new Author { FirstName = "Джон", LastName = "Ромеро" }
                               );
            db.Users.AddRange(
                                new User { Name = "Nabibat0r", Email = "serega@nagibator.ru" },
                                new User { Name = "RakNaM1de", Email = "rack@rachok.com" },
                                new User { Name = "S1lvana", Email = "silvana@wc.com" },
                                new User { Name = "HexxMagister", Email = "hexx@mag.ru" },
                                new User { Name = "Detroit", Email = "detroit@become.hu" }
                             );
            db.Genres.AddRange(
                                new Genre { Name = "Сопливая романтика" },
                                new Genre { Name = "Science fiction" },
                                new Genre { Name = "Научпоп" },
                                new Genre { Name = "Фанфики для взрослых" },
                                new Genre { Name = "Ужасей" }
                              );
            db.Books.AddRange(
                                new Book { Title = "Как Петька и Василий Иваныч таз искали", YearOfPublishing = 1970, Genre = "Сопливая романтика" },
                                new Book { Title = "Сто лет выполнения тасков по Entity Framework", YearOfPublishing = 1666, Genre = "Фанфики для взрослых" },
                                new Book { Title = "Матерные частушки для малышей", YearOfPublishing = 1984, Genre = "Научпоп" },
                                new Book { Title = "Ночной позор", YearOfPublishing = 2020, Genre = "Science fiction" },
                                new Book { Title = "Транспофигизм Inc.", YearOfPublishing = 2024, Genre = "Ужасей" },
                                new Book { Title = "Властелин больших бетонных колец", YearOfPublishing = 1988, Genre = "Ужасей" },
                                new Book { Title = "Копание в колодце разума в поисках названий книг", YearOfPublishing = 2022, Genre = "Science fiction" }
                             );
            db.SaveChanges();
            Console.WriteLine("Initial filling complete, starting initial CRUD operations");
        }
        static void CRUDDemo(AppContext db)
        {
            //теперь присвоим книжкам авторов, заодно продемонстрируем CRUD:
            // - выберем все объекты сразу
            // - выберем объекты по идентификатору
            // - добавим жанр в базу и тут же его удалим
            // - переименуем третьего пользователя
            // - изменим год для пятой книги
            // - выдадим её на руки третьему пользователю

            AuthorRepository authorRepository = new AuthorRepository(db);
            BookRepository bookRepository = new BookRepository(db);
            GenreRepository genreRepository = new GenreRepository(db);
            UserRepository usersRepository = new UserRepository(db);


            Console.WriteLine("Let's see all our authors:");
            var authorsList = authorRepository.GetAllAuthors();
            int iterator = 1;
            foreach (Author item in authorsList)
                {
                Console.WriteLine("N{0} - {1} {2}", iterator, item.FirstName, item.LastName);
                iterator++;
                }
            Console.WriteLine("Let's see third of our authors: " + authorRepository.GetAuthorById(3).ToString());
            Console.WriteLine("/================================/");
            Console.WriteLine("Now we are ready to assign some authors to our books!");
            var rand = new Random();
            foreach (Book item in bookRepository.GetAllBooks())
                {
                int random_id = rand.Next(1, authorRepository.GetAllAuthors().Count);
                bookRepository.SetAuthor(item.Id, authorRepository.GetAuthorById(random_id));
                Console.WriteLine("N{0} - {1}", item.Id, bookRepository.GetBook(item.Id).ToString());
                }
            Console.WriteLine("/================================/");
            Console.WriteLine("Let's add a new genre! But first, here's our current genres");
            genreRepository.PrintGenresList();
            Console.WriteLine("/================================/");
            genreRepository.CreateGenre("SPECIAL");
            Console.WriteLine("We just added a new special genre. Let's see our genres list again: ");
            genreRepository.PrintGenresList();
            Console.WriteLine("/================================/");
            genreRepository.DeleteGenre(genreRepository.GetAllGenres().Count);
            Console.WriteLine("We just deleted a new special genre. Let's see our genres list again: ");
            genreRepository.PrintGenresList();
            Console.WriteLine("/================================/");
            Console.WriteLine("Let's meet our users:");
            usersRepository.PrintUsersList();
            usersRepository.ChangeUserName(3, "NEWUSERNAME");
            Console.WriteLine("We have our third user renamed. Look!");
            usersRepository.PrintUsersList();
            Console.WriteLine("/================================/");
            Console.WriteLine("Let's change year for the fifth book! Now we have this fifth book:");
            bookRepository.UpdateBookPublishYear(5, 1994);
            Console.WriteLine(bookRepository.GetBook(5).ToString());
            Console.WriteLine("Now we're going to assign it to third user! Look how many books he have now: " + usersRepository.GetUserBorrowedBooksCount(3).ToString());
            usersRepository.AssignBookToUser(3, 5);
            Console.WriteLine("Assigned! Look how many books he have now: " + usersRepository.GetUserBorrowedBooksCount(3).ToString());
        }

        static void OperationsDemo(AppContext db)
        {
            AuthorRepository authorRepository = new AuthorRepository(db);
            BookRepository bookRepository = new BookRepository(db);
            GenreRepository genreRepository = new GenreRepository(db);
            UserRepository usersRepository = new UserRepository(db);

            //Здесь продемонстрируем девять заданий из 25.5.4
            //1. Получать список книг определенного жанра и вышедших между определенными годами.
            Console.WriteLine("\n1. Here's books of genre #2 issued between 1994 and 2025");
            bookRepository.PrintBooksList(bookRepository.GetBooksByGenreIssueDate(2, 1994, 2025));
            //2. Получать количество книг определенного автора в библиотеке.
            Console.WriteLine("\n2. Author #1 writed {0} books", authorRepository.GetBooksCountByAuthor(0));
            //3. Получать количество книг определенного жанра в библиотеке.
            Console.WriteLine("\n3. Genre #4 contains {0} books", bookRepository.GetBooksCountByGenre(4));
            //4. Получать булевый флаг о том, есть ли книга определенного автора и с определенным названием в библиотеке.
            Console.WriteLine("\n4. Is there a book of {0}, named {1}? Answer: {2}", authorRepository.GetAuthorById(4).ToString() ,bookRepository.GetBook(2).Title, bookRepository.CheckBook(authorRepository.GetAuthorById(4).LastName, bookRepository.GetBook(2).Title));
            //5. Получать булевый флаг о том, есть ли определенная книга на руках у пользователя.
            Console.WriteLine("\n5. Is the book #5 borrowed? Answer: " + bookRepository.IsBookBorrowed(5).ToString());
            //6. Получать количество книг на руках у пользователя.
            Console.WriteLine("\n6. How many books did we assign to user#3? Answer: " + usersRepository.GetUserBorrowedBooksCount(3));
            //7. Получение последней вышедшей книги.
            Console.WriteLine("\n7. The newest book is: " + bookRepository.GetNewestBook().ToString());
            //8. Получение списка всех книг, отсортированного в алфавитном порядке по названию.
            Console.WriteLine("\n8. Now we got our books sorted by title alphabetically:");
            bookRepository.PrintBooksList(bookRepository.GetAllBooksOrderedByTitle());
            //9. Получение списка всех книг, отсортированного в порядке убывания года их выхода.
            Console.WriteLine("\n9. Now we got our books sorted by issue date descending:");
            bookRepository.PrintBooksList(bookRepository.GetAllBooksOrderedByYearFromNewest());





        }

    }
}