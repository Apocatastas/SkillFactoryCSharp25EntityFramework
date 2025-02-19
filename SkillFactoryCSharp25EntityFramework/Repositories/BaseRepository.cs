using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillFactoryCSharp25EntityFramework.Repositories
{
    public abstract class BaseRepository
    {
        public AppContext DbContext { get; private set; }

        public BaseRepository()
        {
            DbContext = new AppContext();
        }

        public BaseRepository(AppContext dbContext)
        {
            DbContext = dbContext;
        }
    }
}
