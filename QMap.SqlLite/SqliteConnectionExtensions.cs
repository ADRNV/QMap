using Microsoft.Data.Sqlite;

namespace QMap.Sqlite
{
    public static class SqliteConnectionExtensions
    {
        public static QMapSqliteConnectionAdapter Adapt(this SqliteConnection sqlConnection)
        {
            return new QMapSqliteConnectionAdapter(sqlConnection);
        }
    }
}
