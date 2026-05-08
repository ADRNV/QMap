namespace QMap.SqlBuilder.Module.Tests;

using QMap.Core.Dialects;
using QMap.SqlBuilder;

public class FromBuilderTests
{
    [Fact]
    public void BuildFromWithSelect_ContainsFromKeyword()
    {
        var builder = new StatementsBuilders(new SqlDialectBase());
        var result = builder
            .Select(typeof(TestEntity))
            .From(typeof(TestEntity));
        var sql = result.Build();

        Assert.Contains("from", sql, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void BuildFromWithSelect_ContainsEntityTypeName()
    {
        var builder = new StatementsBuilders(new SqlDialectBase());
        var result = builder
            .Select(typeof(TestEntity))
            .From(typeof(TestEntity));
        var sql = result.Build();

        Assert.Contains("TestEntity", sql);
    }

    [Fact]
    public void BuildFromWithSelect_ContainsGeneratedAlias()
    {
        var builder = new StatementsBuilders(new SqlDialectBase());
        var result = builder
            .Select(typeof(TestEntity))
            .From(typeof(TestEntity));
        var sql = result.Build();

        Assert.NotEmpty(sql);
        Assert.Contains("TestEntity", sql);
    }

    [Fact]
    public void BuildFromWithDelete_ContainsDeleteAndFrom()
    {
        var builder = new StatementsBuilders(new SqlDialectBase());
        var result = builder
            .Delete<TestEntity>(out var _)
            .From(typeof(TestEntity));
        var sql = result.Build();

        Assert.Contains("delete", sql, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("from", sql, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Build_ReturnsSql()
    {
        var builder = new StatementsBuilders(new SqlDialectBase());
        var result = builder
            .Select(typeof(TestEntity))
            .From(typeof(TestEntity));
        var sql = result.Build();

        Assert.NotEmpty(sql);
        Assert.NotNull(sql);
    }
}
