using QMap.Core.Command;
using System.Data;
using System.Data.SqlClient;

namespace QMap.SqlServer
{
    public class QMapSqlServerCommand : QMapCommandAdapterBase<SqlCommand>
    {
        public QMapSqlServerCommand(SqlCommand command) : base(command)
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
