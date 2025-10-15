using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QMap.Core;
using QMap.Tests.Common;
using QMap.Tests.Share;
using QMap.Tests.Share.Common;
using QMap.Tests.Share.DataBase;

namespace QMap.Tests
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(@"TestConfiguration.json", false)
                .Build();



            services.AddSingleton<IConfiguration>(configuration);

            services.AddScoped<IQMapConnectionFactory, SqlServerConnectionFactory>(sp =>
            {
                var connectionString = configuration.GetConnectionString("TestDbConnectionSqlServer");
                
                var options = new DbContextOptionsBuilder()
                    .EnableDetailedErrors()
                    .UseSqlServer(connectionString)
                    .Options;

                return new SqlServerConnectionFactory(configuration, options);
            });

            services.AddScoped<IQMapConnectionFactory, SqliteConnectionFactory>(sp =>
            {
                var connectionString = configuration.GetConnectionString("TestDbConnectionSqlite");

                var options = new DbContextOptionsBuilder()
                    .EnableDetailedErrors()
                    .UseSqlite(connectionString)
                    .Options;

                return new SqliteConnectionFactory(configuration, options);
            });

        }
    }
}
