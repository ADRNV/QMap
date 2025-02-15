using Microsoft.Data.Sqlite;
using Moq;
using QMap.Core;
using QMap.Core.Dialects;
using QMap.Sqlite;
using System.Data;
using System.Data.Common;

namespace QMap.Tests.Share.Common.Fakes.Connections
{
    public class FakeQMapConnectionAdapter : QMapConnectionAdapterBase<DbConnection>
    {
        public override ISqlDialect Dialect => new SqlDialectBase();

        public FakeQMapConnectionAdapter(DbConnection connection) : base(connection)
        {

        }

        public override void Dispose()
        {
            _connection.Dispose();
        }
    }
}
