using Microsoft.EntityFrameworkCore;

namespace QMap.Tests.Share.Common.DataBase
{
    public class BaseDbContext : DbContext
    {
        public BaseDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) 
        {
            
        }
    }
}
