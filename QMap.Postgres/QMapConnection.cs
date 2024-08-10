using Npgsql;
using QMap.Core;
using QMap.Core.Command;

namespace QMap.Postgres
{
    public class QMapNpgsqlConnection : QMapConnectionAdapterBase<NpgsqlConnection>
    {
        public QMapNpgsqlConnection(NpgsqlConnection connection) : base(connection)
        {
            
        }

        public override IQMapCommand CreateCommand(FormattableString sql)
        {
            return new QMapNpgsqlCommand(new NpgsqlCommand(sql.ToString(), _connection));
        }
    }
}
