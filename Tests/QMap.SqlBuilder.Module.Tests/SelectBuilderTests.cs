namespace QMap.SqlBuilder.Module.Tests;

using QMap.Core.Dialects;
using QMap.SqlBuilder;

public class SelectBuilderTests
{
    [Fact]
    public void SelectByType_ContainsSelectStar()
    {
        var builder = new StatementsBuilders(new SqlDialectBase());
        var result = builder.Select(typeof(TestEntity)).From(typeof(TestEntity));
        var sql = result.Build();

        Assert.Contains("select *", sql);
    }

    [Fact]
    public void SelectWithAnonymousProjection_ContainsAllColumns()
    {
        var builder = new StatementsBuilders(new SqlDialectBase());
        var result = builder
            .Select((TestEntity e) => new { e.Id, e.Name })
            .From(typeof(TestEntity));
        var sql = result.Build();

        Assert.Contains("Id", sql);
        Assert.Contains("Name", sql);
        Assert.DoesNotContain("*", sql);
    }

    [Fact]
    public void SelectWithSingleMemberProjection_ContainsSingleColumn()
    {
        var builder = new StatementsBuilders(new SqlDialectBase());
        var result = builder
            .Select((TestEntity e) => e.Id)
            .From(typeof(TestEntity));
        var sql = result.Build();

        Assert.Contains("Id", sql);
        Assert.DoesNotContain("*", sql);
    }

    [Fact]
    public void SelectBuild_ThrowsInvalidOperationException()
    {
        var builder = new StatementsBuilders(new SqlDialectBase());
        var selectBuilder = builder.Select(typeof(TestEntity));

        Assert.Throws<InvalidOperationException>(() => selectBuilder.Build());
    }
}
