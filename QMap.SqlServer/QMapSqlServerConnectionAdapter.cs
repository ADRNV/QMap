using QMap.Core;
using QMap.Core.Dialects;
using System.Data.SqlClient;

namespace QMap.SqlServer
{
    public class QMapSqlServerConnectionAdapter : QMapConnectionAdapterBase<SqlConnection>
    {
        public override ISqlDialect Dialect => new TSqlDialect();

        public QMapSqlServerConnectionAdapter(SqlConnection connection) : base(connection)
        {
        }

        public override void Dispose()
        {
            _connection.Dispose();
        }
    }
}