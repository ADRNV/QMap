using QMap.Core;
using QMap.Core.Dialects;
using Microsoft.Data.Sqlite;

namespace QMap.Sqlite
{
    public class QMapSqliteConnectionAdapter : QMapConnectionAdapterBase<SqliteConnection>
    {
        public override ISqlDialect Dialect => new SqliteDialect();

        public QMapSqliteConnectionAdapter(SqliteConnection connection) : base(connection)
        {
        }

        public override void Dispose()
        {
            _connection.Dispose();
        }
    }
}