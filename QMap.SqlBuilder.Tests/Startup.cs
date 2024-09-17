using Microsoft.Extensions.DependencyInjection;
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
        }
    }
}
