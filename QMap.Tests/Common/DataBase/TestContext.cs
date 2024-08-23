using Microsoft.EntityFrameworkCore;

namespace QMap.Tests.Common.DataBase
{
    public class TestContext : DbContext
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
