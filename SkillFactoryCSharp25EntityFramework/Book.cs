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

        public string Authors { get; set; }

        public string Category { get; set; }


    }
}
