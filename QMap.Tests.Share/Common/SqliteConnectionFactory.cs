using Microsoft.Extensions.Configuration;
using QMap.Core;
using QMap.Sqlite;
using QMap.Tests.Share;
using Microsoft.Data.Sqlite;
using QMap.Tests.Share.DataBase;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace QMap.Tests.Common
{
    public class SqliteConnectionFactory : QMapConnectionFactoryBase
    {
   
        public SqliteConnectionFactory(IConfiguration configuration, DbContextOptions options) : base(configuration, options)
        {

        }

        public override IQMapConnection Create()
        {
            var connectionString = _configuration.GetConnectionString("TestDbConnectionSqlite");
            return new SqliteConnection(connectionString).Adapt();
        }
    }
}
