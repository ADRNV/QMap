namespace QMap.SqlBuilder.Module.Tests;

using QMap.Core.Dialects;
using QMap.SqlBuilder;
using QMap.Tests.Share.Common.Fakes.Connections;

public class InsertBuilderTests
{
    [Fact]
    public void BuildInsert_ContainsInsertIntoKeyword()
    {
        var connectionFake = FakeConnectionExtensions.Create();
        var builder = new StatementsBuilders(connectionFake.Dialect);
        var entity = new TestEntity { Id = 1, Name = "test", Value = 10 };

        var sql = builder.BuildInsert(connectionFake, out var _, entity);

        Assert.Contains("insert into", sql, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void BuildInsert_ContainsEntityTypeName()
    {
        var connectionFake = FakeConnectionExtensions.Create();
        var builder = new StatementsBuilders(connectionFake.Dialect);
        var entity = new TestEntity { Id = 1, Name = "test", Value = 10 };

        var sql = builder.BuildInsert(connectionFake, out var _, entity);

        Assert.Contains("TestEntity", sql);
    }

    [Fact]
    public void BuildInsert_ContainsAllPublicPropertyNames()
    {
        var connectionFake = FakeConnectionExtensions.Create();
        var builder = new StatementsBuilders(connectionFake.Dialect);
        var entity = new TestEntity { Id = 1, Name = "test", Value = 10 };

        var sql = builder.BuildInsert(connectionFake, out var _, entity);

        Assert.Contains("Id", sql);
        Assert.Contains("Name", sql);
        Assert.Contains("Value", sql);
    }

    [Fact]
    public void BuildInsert_PopulatesParameters()
    {
        var connectionFake = FakeConnectionExtensions.Create();
        var builder = new StatementsBuilders(connectionFake.Dialect);
        var entity = new TestEntity { Id = 1, Name = "test", Value = 10 };

        var sql = builder.BuildInsert(connectionFake, out var parameters, entity);

        Assert.NotEmpty(parameters);
        Assert.Equal(3, parameters.Count);
    }

    [Fact]
    public void BuildInsertExcept_OmitsExcludedProperty()
    {
        var connectionFake = FakeConnectionExtensions.Create();
        var builder = new StatementsBuilders(connectionFake.Dialect);
        var entity = new TestEntity { Id = 1, Name = "test", Value = 10 };

        var sql = builder.BuildInsert(connectionFake, out var _, entity, (e) => e.Id);

        Assert.DoesNotContain("Id", sql);
    }

    [Fact]
    public void BuildInsertExcept_IncludesNonExcludedProperties()
    {
        var connectionFake = FakeConnectionExtensions.Create();
        var builder = new StatementsBuilders(connectionFake.Dialect);
        var entity = new TestEntity { Id = 1, Name = "test", Value = 10 };

        var sql = builder.BuildInsert(connectionFake, out var _, entity, (e) => e.Id);

        Assert.Contains("Name", sql);
        Assert.Contains("Value", sql);
    }
}
