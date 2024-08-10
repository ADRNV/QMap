using Npgsql;
using QMap.Core.Command;
using System.Data;

namespace QMap.Postgres
{
    public class QMapNpgsqlCommand : QMapCommandAdapterBase<NpgsqlCommand>
    {


        public Task<IDataReader> ExecuteReader(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<IDataReader> ExecuteReader(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IDataReader> ExecuteReaderAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<IDataReader> ExecuteReaderAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
