using Microsoft.Extensions.Configuration;
using QMap.Core;
using QMap.SqlServer;
using System.Data.SqlClient;

namespace QMap.Tests.Common
{
    public class SqlServerQMapConnectionFactory : IQMapConnectionFactoryBase
    {
        public SqlServerQMapConnectionFactory(IConfiguration configuration) : base(configuration)
        {

        }

        public override IQMapConnection Create()
        {
            var connectionString = _configuration.GetConnectionString("TestDbConnectionSqlServer");
            return new QMapSqlServerConnectionAdapter(new SqlConnection(connectionString));
        }
    }
}
