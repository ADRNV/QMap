using QMap.Core.Dialects;
using System.Data;

namespace QMap.Core
{
    public interface IQMapConnection : IDbConnection
    {
        ISqlDialect Dialect { get; }
    }
}
