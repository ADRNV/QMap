using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QMap.Tests.Common;
using QMap.Tests.Share;
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

            services.AddTransient<IEnumerable<IQMapConnectionFactoryBase>, List<IQMapConnectionFactoryBase>>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();

                var factories = new List<IQMapConnectionFactoryBase>
                {
                    new SqlServerQMapConnectionFactory(configuration)
                };

                return factories;
            });

            services.AddDbContext<TestContext>(options =>
            {
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                options.UseSqlServer(configuration.GetConnectionString("TestDbConnectionSqlServer"));
            });
        }
    }
}
