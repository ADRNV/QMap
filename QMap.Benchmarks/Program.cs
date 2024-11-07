using BenchmarkDotNet.Running;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QMap.Benchmarks.Benchmarks;
using QMap.Benchmarks.Benchmarks.SqlServer;
using QMap.Benchmarks.DI;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

var sqlServer = new SqlServerDependency(builder);

sqlServer.Configure();

builder.Services.AddHostedService<BenchmarkService>(sp =>
{
    return new BenchmarkService(() =>
    {
        BenchmarkRunner.Run<QMapQueryBenchmark>();
        BenchmarkRunner.Run<QMapQueryBenchmarkExplicitReading>();

    }, sp.GetRequiredService<IEnumerable<Action>>());
});

builder.Build()
    .Run();
