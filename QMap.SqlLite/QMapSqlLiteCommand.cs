using QMap.Core.Command;
using System.Data;

using Microsoft.Data.Sqlite;

namespace QMap.Sqlite
{
    public class QMapSqlLiteCommand : QMapCommandAdapterBase<SqliteCommand>
    {
        public QMapSqlLiteCommand(SqliteCommand command) : base(command)
        {
        }

        public override Task<IDataReader> ExecuteReader(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<IDataReader> ExecuteReaderAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
