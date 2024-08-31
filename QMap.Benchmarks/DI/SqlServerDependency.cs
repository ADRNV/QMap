using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using QMap.Tests.Share.DataBase;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using AutoFixture;

namespace QMap.Benchmarks.DI
{
    public class SqlServerDependency
    {
        private HostApplicationBuilder _host;

        private IConfiguration _configuration;

        public SqlServerDependency(HostApplicationBuilder host) 
        {
            _host = host;
        }

        public void Configure()
        {
            var configuration = new ConfigurationBuilder()
               .AddJsonFile(@"TestConfiguration.json", false)
               .Build();

            _host.Services.AddDbContext<TestContext>(options =>
            {
                //options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                options.UseSqlServer(configuration.GetConnectionString("TestDbConnectionSqlServer"));
            });

            _host.Services.AddSingleton<IEnumerable<Action>, List<Action>>(sp =>
            {
                return new List<Action>()
                {
                    () =>
                    {

                         var expectedEntity = new Fixture()
                            .Build<TypesTestEntity>()
                            .Without(t => t.Id)
                            .With(t => t.ByteField)
                            .CreateMany(50000);

                        var db = sp.GetRequiredService<TestContext>();
                         db.Database.EnsureDeleted();
                        db.Database.EnsureCreated();

                        db.TypesTestEntity.AddRange(expectedEntity);

                        db.SaveChanges();
                    }
                };
            });

        }
    }
}
