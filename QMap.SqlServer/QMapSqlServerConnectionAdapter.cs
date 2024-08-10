using QMap.Core;
using System.Data.SqlClient;

namespace QMap.SqlServer
{
    public class QMapSqlServerConnectionAdapter : QMapConnectionAdapterBase<SqlConnection>
    {
        public QMapSqlServerConnectionAdapter(SqlConnection connection) : base(connection)
        {
        }

        public override void Dispose()
        {
            _connection.Dispose();
        }
    }
}