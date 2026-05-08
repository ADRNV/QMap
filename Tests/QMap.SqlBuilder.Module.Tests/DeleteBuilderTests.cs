namespace QMap.SqlBuilder.Module.Tests;

using System.Linq.Expressions;
using QMap.Core.Dialects;
using QMap.SqlBuilder;

public class DeleteBuilderTests
{
    [Fact]
    public void BuildDelete_ContainsDeleteKeyword()
    {
        var builder = new StatementsBuilders(new SqlDialectBase());
        var result = builder
            .Delete<TestEntity>(out var _)
            .From(typeof(TestEntity));
        var sql = result.Build();

        Assert.Contains("delete", sql, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void BuildDelete_Build_ThrowsInvalidOperationException()
    {
        var builder = new StatementsBuilders(new SqlDialectBase());
        var deleteBuilder = builder.Delete<TestEntity>(out var _);

        Assert.Throws<InvalidOperationException>(() => deleteBuilder.Build());
    }

    [Fact]
    public void BuildDeleteFrom_ContainsFromKeyword()
    {
        var builder = new StatementsBuilders(new SqlDialectBase());
        var result = builder
            .Delete<TestEntity>(out var _)
            .From(typeof(TestEntity));
        var sql = result.Build();

        Assert.Contains("from", sql, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void BuildDeleteFrom_ContainsEntityTypeName()
    {
        var builder = new StatementsBuilders(new SqlDialectBase());
        var result = builder
            .Delete<TestEntity>(out var _)
            .From(typeof(TestEntity));
        var sql = result.Build();

        Assert.Contains("TestEntity", sql);
    }

    [Fact]
    public void BuildDeleteWithWhere_ContainsWhereKeyword()
    {
        var builder = new StatementsBuilders(new SqlDialectBase());
        Expression<Func<TestEntity, bool>> expr = e => e.Id > 0;
        var result = builder
            .Delete<TestEntity>(out var _)
            .From(typeof(TestEntity))
            .Where<TestEntity>(expr, out var _);
        var sql = result.Build();

        Assert.Contains("where", sql, StringComparison.OrdinalIgnoreCase);
    }
}
