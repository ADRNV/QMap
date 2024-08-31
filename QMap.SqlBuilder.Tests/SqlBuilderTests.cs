namespace QMap.SqlBuilder.Tests
{
    public class SqlBuilderTests
    {
        [Fact]
        public void BuildWhereNoThrowsErrors()
        {
            QueryBuilder queryBuilder = new QueryBuilder();

            queryBuilder.BuildWhere((int i) => i == 1);
        }
    }
}