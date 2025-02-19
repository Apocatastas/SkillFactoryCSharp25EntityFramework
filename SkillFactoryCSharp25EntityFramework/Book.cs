using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillFactoryCSharp25EntityFramework
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int YearOfPublishing { get; set; }
        public int? UserId { get; set; }
        public User? BorrowedBy { get; set; }
        public List<Author> Authors { get; set; }
        public string Genre { get; set; }
        public int? GenreId { get; set; }

        public override string ToString()
        {
            string Authors = this.Authors.Count > 1 ? string.Join(", ", this.Authors) : this.Authors[0].ToString();
            return $"{Authors}. {Title}. - {YearOfPublishing}.";
        }
    }
}
