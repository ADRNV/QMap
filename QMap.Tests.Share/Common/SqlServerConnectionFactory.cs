using Microsoft.Extensions.Configuration;
using QMap.Core;
using QMap.SqlServer;
using QMap.Tests.Share;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Data.SqlClient;
using QMap.Tests.Share.DataBase;

namespace QMap.Tests.Common
{
    public class SqlServerConnectionFactory : QMapConnectionFactoryBase
    {
        public SqlServerConnectionFactory(IConfiguration configuration, DbContextOptions options) : base(configuration, options)
        {
      
        }

        public override IQMapConnection Create()
        {
            var connectionString = _configuration.GetConnectionString("TestDbConnectionSqlServer");
        
            return new SqlConnection(connectionString).Adapt();
        }
    }
}
