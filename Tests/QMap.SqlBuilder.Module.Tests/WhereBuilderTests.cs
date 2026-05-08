namespace QMap.SqlBuilder.Module.Tests;

using System.Linq.Expressions;
using QMap.Core.Dialects;
using QMap.SqlBuilder;

public class WhereBuilderTests
{
    [Fact]
    public void BuildWhere_ContainsWhereKeyword()
    {
        var builder = new StatementsBuilders(new SqlDialectBase());
        Expression<Func<TestEntity, bool>> expr = e => e.Id == 1;
        var result = builder
            .Select(typeof(TestEntity))
            .From(typeof(TestEntity))
            .Where<TestEntity>(expr, out var _);
        var sql = result.Build();

        Assert.Contains("where", sql, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void BuildWhere_EqualOperator_ContainsEqualSign()
    {
        var builder = new StatementsBuilders(new SqlDialectBase());
        Expression<Func<TestEntity, bool>> expr = e => e.Id == 1;
        var result = builder
            .Select(typeof(TestEntity))
            .From(typeof(TestEntity))
            .Where<TestEntity>(expr, out var _);
        var sql = result.Build();

        Assert.Contains("=", sql);
    }

    [Fact]
    public void BuildWhere_NotEqualOperator_ContainsNotEqualSign()
    {
        var builder = new StatementsBuilders(new SqlDialectBase());
        Expression<Func<TestEntity, bool>> expr = e => e.Id != 5;
        var result = builder
            .Select(typeof(TestEntity))
            .From(typeof(TestEntity))
            .Where<TestEntity>(expr, out var _);
        var sql = result.Build();

        Assert.Contains("!=", sql);
    }

    [Fact]
    public void BuildWhere_GreaterThanOperator_ContainsOperator()
    {
        var builder = new StatementsBuilders(new SqlDialectBase());
        Expression<Func<TestEntity, bool>> expr = e => e.Id > 0;
        var result = builder
            .Select(typeof(TestEntity))
            .From(typeof(TestEntity))
            .Where<TestEntity>(expr, out var _);
        var sql = result.Build();

        Assert.Contains(">", sql);
    }

    [Fact]
    public void BuildWhere_LessThanOperator_ContainsOperator()
    {
        var builder = new StatementsBuilders(new SqlDialectBase());
        Expression<Func<TestEntity, bool>> expr = e => e.Id < 100;
        var result = builder
            .Select(typeof(TestEntity))
            .From(typeof(TestEntity))
            .Where<TestEntity>(expr, out var _);
        var sql = result.Build();

        Assert.Contains("<", sql);
    }

    [Fact]
    public void BuildWhere_AndAlsoOperator_ContainsAndKeyword()
    {
        var builder = new StatementsBuilders(new SqlDialectBase());
        Expression<Func<TestEntity, bool>> expr = e => e.Id == 1 && e.Value == 2;
        var result = builder
            .Select(typeof(TestEntity))
            .From(typeof(TestEntity))
            .Where<TestEntity>(expr, out var _);
        var sql = result.Build();

        Assert.Contains("and", sql, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void BuildWhere_OrOperator_ContainsOrKeyword()
    {
        var builder = new StatementsBuilders(new SqlDialectBase());
        Expression<Func<TestEntity, bool>> expr = e => e.Id == 1 || e.Value == 2;
        var result = builder
            .Select(typeof(TestEntity))
            .From(typeof(TestEntity))
            .Where<TestEntity>(expr, out var _);
        var sql = result.Build();

        Assert.Contains("or", sql, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void BuildWhere_CapturedIntVariable_RegistersParameter()
    {
        var capturedId = 42;
        var builder = new StatementsBuilders(new SqlDialectBase());
        Expression<Func<TestEntity, bool>> expr = e => e.Id == capturedId;
        var result = builder
            .Select(typeof(TestEntity))
            .From(typeof(TestEntity))
            .Where<TestEntity>(expr, out var parameters);

        Assert.NotEmpty(parameters);
    }

    [Fact]
    public void BuildWhere_CapturedStringVariable_RegistersParameter()
    {
        var capturedName = "test";
        var builder = new StatementsBuilders(new SqlDialectBase());
        Expression<Func<TestEntity, bool>> expr = e => e.Name == capturedName;
        var result = builder
            .Select(typeof(TestEntity))
            .From(typeof(TestEntity))
            .Where<TestEntity>(expr, out var parameters);

        Assert.NotEmpty(parameters);
    }

    [Fact]
    public void BuildWhere_SqlInjectionString_ParameterizedNotLiteral()
    {
        var injectionString = "' DROP TABLE TestEntity; --'";
        var builder = new StatementsBuilders(new SqlDialectBase());
        Expression<Func<TestEntity, bool>> expr = e => e.Name == injectionString;
        var result = builder
            .Select(typeof(TestEntity))
            .From(typeof(TestEntity))
            .Where<TestEntity>(expr, out var _);
        var sql = result.Build();

        Assert.Contains("@", sql);
        Assert.DoesNotContain("DROP TABLE", sql, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void BuildWhere_ContainsSqlFromFromBuilder()
    {
        var builder = new StatementsBuilders(new SqlDialectBase());
        Expression<Func<TestEntity, bool>> expr = e => e.Id == 1;
        var result = builder
            .Select(typeof(TestEntity))
            .From(typeof(TestEntity))
            .Where<TestEntity>(expr, out var _);
        var sql = result.Build();

        Assert.Contains("select", sql, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("from", sql, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("where", sql, StringComparison.OrdinalIgnoreCase);
    }
}
