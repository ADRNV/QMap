using Microsoft.EntityFrameworkCore;
using QMap.Tests.Share.Common.DataBase;

namespace QMap.Tests.Share.DataBase
{
    public class TestContext : BaseDbContext
    {
        public TestContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<TypesTestEntity> TypesTestEntity { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TypesTestEntity>()
                .HasKey(t => t.Id);
        }
    }
}
