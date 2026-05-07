using Microsoft.Extensions.DependencyInjection;
using QMap.Core.Dialects;
using QMap.SqlServer;
using QMap.Tests.Share.Helpers.Sql;

namespace QMap.SqlBuilder.Tests
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IList<IParser>>(sp =>
            {
                return new List<IParser>()
                {
                    new TSqlParser()
                };
            });

            services.AddSingleton<IList<ISqlDialect>>(sp =>
            {
                return new List<ISqlDialect>()
                {
                    new TSqlDialect()
                };
            });
        }
    }
}
