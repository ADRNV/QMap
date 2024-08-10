using System.Data;

namespace QMap.Core.Command
{
    public interface IQMapCommand : IDbCommand
    {
        Task<IDataReader> ExecuteReaderAsync(CancellationToken cancellationToken);

        Task<IDataReader> ExecuteReader(CancellationToken cancellationToken);
    }
}
