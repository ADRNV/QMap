using System.Data.SqlClient;

namespace QMap.SqlServer
{
    public static class SqlConnectionExtensions
    {
        public static QMapSqlServerConnectionAdapter Adapt(this SqlConnection sqlConnection)
        {
            return new QMapSqlServerConnectionAdapter(sqlConnection);
        }
    }
}
