namespace QMap.SqlBuilder.Module.Tests;

using System.Linq.Expressions;
using QMap.Core.Dialects;
using QMap.SqlBuilder;
using QMap.Tests.Share.Common.Fakes.Connections;

public class UpdateBuilderTests
{
    [Fact]
    public void BuildUpdate_ContainsUpdateKeyword()
    {
        var connectionFake = FakeConnectionExtensions.Create();
        var builder = new StatementsBuilders(connectionFake.Dialect);

        Expression<Func<TestEntity, int>> propExpr = e => e.Value;
        var result = builder
            .Update<TestEntity, int>(connectionFake, out var _, propExpr, 20);
        var sql = result.Sql;

        Assert.Contains("update", sql, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void BuildUpdate_ContainsSetKeyword()
    {
        var connectionFake = FakeConnectionExtensions.Create();
        var builder = new StatementsBuilders(connectionFake.Dialect);

        Expression<Func<TestEntity, int>> propExpr = e => e.Value;
        var result = builder
            .Update<TestEntity, int>(connectionFake, out var _, propExpr, 20);
        var sql = result.Sql;

        Assert.Contains("set", sql, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void BuildUpdate_ContainsPropertyName()
    {
        var connectionFake = FakeConnectionExtensions.Create();
        var builder = new StatementsBuilders(connectionFake.Dialect);

        Expression<Func<TestEntity, int>> propExpr = e => e.Value;
        var result = builder
            .Update<TestEntity, int>(connectionFake, out var _, propExpr, 20);
        var sql = result.Sql;

        Assert.Contains("Value", sql);
    }

    [Fact]
    public void BuildUpdate_RegistersParameterWithCorrectValue()
    {
        var connectionFake = FakeConnectionExtensions.Create();
        var builder = new StatementsBuilders(connectionFake.Dialect);

        Expression<Func<TestEntity, int>> propExpr = e => e.Value;
        var result = builder
            .Update<TestEntity, int>(connectionFake, out var parameters, propExpr, 20);

        Assert.NotEmpty(parameters);
    }

    [Fact]
    public void BuildUpdate_NonMemberExpression_ThrowsInvalidOperationException()
    {
        var connectionFake = FakeConnectionExtensions.Create();
        var builder = new StatementsBuilders(connectionFake.Dialect);

        Assert.Throws<InvalidOperationException>(() =>
        {
            Expression<Func<TestEntity, string>> propExpr = e => e.ToString();
            var result = builder
                .Update<TestEntity, string>(connectionFake, out var _, propExpr, "test");
        });
    }
}
