using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillFactoryCSharp25EntityFramework.Repositories
{
    public class GenreRepository : BaseRepository
    {
        public GenreRepository() : base() { }

        public GenreRepository(AppContext appContext) : base(appContext) { }

        public int CreateGenre(string genreName)
        {
            DbContext.Genres.Add(new Genre { Name = genreName });
            return DbContext.SaveChanges();
        }
        public Genre GetGenreById(int id)
        {
            return DbContext.Genres.SingleOrDefault(c => c.Id == id);
        }

        public List<Genre> GetAllGenres()
        {
            return DbContext.Genres.ToList();
        }

        public int UpdateGenreName(int genreId, string genreNewName)
        {
            GetGenreById(genreId).Name = genreNewName;
            return DbContext.SaveChanges();
        }

        public void PrintGenresList()
        {
            int iterator = 1;
            var genresList = GetAllGenres();
            foreach (Genre item in genresList)
            {
                Console.WriteLine("N{0} - {1}", iterator, item.Name);
                iterator++;
            };
        }

        public int DeleteGenre(int id)
        {
            int result;
            using (var transactionContext = DbContext.Database.BeginTransaction())
            {
                Genre genreToDelete = GetGenreById(id);
                if (genreToDelete.BooksOfGenre is not null)
                {
                    foreach (Book book in genreToDelete.BooksOfGenre)
                    {
                        book.GenreId = null;
                        book.Genre = null;
                    }
                    genreToDelete.BooksOfGenre.Clear();
                }
                DbContext.Genres.Remove(genreToDelete);
                result = DbContext.SaveChanges();
                transactionContext.Commit();
            }
            return result;
        }
    }
}
