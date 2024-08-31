using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using QMap;
using QMap.SqlServer;
using QMap.Core;
using QMap.Tests.Share.DataBase;
using System.Data.SqlClient;
using Dapper;

namespace QMap.Benchmarks.Benchmarks.SqlServer
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class QMapQueryBenchmark
    {
        public static string ConnectionString = "Server=localhost;Database=TestDb;Integrated Security=true;TrustServerCertificate=Yes;Encrypt=false";
        
        [Benchmark]
        public void QMapQueryMethodBenchmark()
        {
            using var connection = new SqlConnection(ConnectionString).Adapt();

            connection.Open();

            connection.Query<TypesTestEntity>("select * from TypesTestEntity");

            connection.Close();
        }

        [Benchmark(Baseline = true)]      
        public void DapperQueryMethodBenchmark()
        {
            using var connection = new SqlConnection(ConnectionString);

            connection.Open();

            Dapper.SqlMapper
                .Query<TypesTestEntity>(connection, "select * from TypesTestEntity");
            
            connection.Close();
        }
    }
}
