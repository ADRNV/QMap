using AutoFixture;
using BenchmarkDotNet.Attributes;
using Dapper.Contrib.Extensions;
using QMap.SqlServer;
using QMap.Tests.Share.DataBase;
using System.Data.SqlClient;

namespace QMap.Benchmarks.Benchmarks.SqlServer
{
    public class QMapInsertBenchmark
    {
        public static string ConnectionString = "Server=localhost;Database=TestDb;Integrated Security=true;TrustServerCertificate=Yes;Encrypt=false";

        [Benchmark]
        public void QMapInsertMethodBenchmark()
        {
            using var connection = new SqlConnection(ConnectionString).Adapt();

            connection.Open();

            var entity = new Fixture().Build<TypesTestEntity>()
                .Without(t => t.Id)
                .Create();

            connection.Insert(entity, e => e.Id);

            connection.Close();
        }
    }
}
