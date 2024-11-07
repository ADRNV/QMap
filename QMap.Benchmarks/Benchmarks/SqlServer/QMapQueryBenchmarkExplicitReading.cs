using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using QMap.SqlServer;
using QMap.Tests.Share.DataBase;
using System.Data.SqlClient;

namespace QMap.Benchmarks.Benchmarks.SqlServer
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class QMapQueryBenchmarkExplicitReading
    {
        public static string ConnectionString = "Server=localhost;Database=TestDb;Integrated Security=true;TrustServerCertificate=Yes;Encrypt=false";

        [Benchmark]
        public void QMapQueryMethodBenchmark()
        {
            using var connection = new SqlConnection(ConnectionString).Adapt();

            connection.Open();

            var query = connection.Query<TypesTestEntity>("select * from TypesTestEntity");

            List<TypesTestEntity> typesTestEntities = new();

            foreach (var entity in query)
            {
                typesTestEntities.Add(entity);
            }

            connection.Close();
        }

        [Benchmark(Baseline = true)]
        public void DapperQueryMethodBenchmark()
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();

            var query = Dapper.SqlMapper
                .Query<TypesTestEntity>(connection, "select * from TypesTestEntity");

            List<TypesTestEntity> typesTestEntities = new();

            foreach (var entity in query)
            {
                typesTestEntities.Add(entity);
            }

            connection.Close();
        }
    }
}
