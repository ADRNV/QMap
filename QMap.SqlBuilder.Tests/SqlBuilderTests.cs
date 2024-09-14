using QMap.Tests.Share.DataBase;
using QMap.Tests.Share.Helpers.Sql;

namespace QMap.SqlBuilder.Tests
{
    public class SqlBuilderTests : IDisposable
    {
        private IList<IParser> _parsers;

        public SqlBuilderTests(IList<IParser> parsers)
        {
            _parsers = parsers;
        }

        [Fact]
        public void BuildWhereNoThrowsErrors()
        {
            QueryBuilder queryBuilder = new QueryBuilder();

            queryBuilder.BuildWhere((int i) => i == 1);
        }

        public void BuildFromNoThrowsErrors()
        {

        }

        public void BuildSelectNoThrowsErrors()
        {

        }

        [Fact]
        public void BuildWhereWithClassNoThrowsErrors()
        {
            QueryBuilder queryBuilder = new QueryBuilder();


        }

        [Fact]
        public void FullSqlBuildNoThrowsErrors()
        {
            QueryBuilder queryBuilder = new QueryBuilder();

            var sql = queryBuilder
                .Select(typeof(TypesTestEntity))
                .From(typeof(TypesTestEntity))
                .Where((TypesTestEntity e) => e.Id == 1)
                .Build();
        }

        [Fact]
        public void FullSqlBuildWithParamsNoThrowsErrors()
        {
            QueryBuilder queryBuilder = new QueryBuilder();

            var sql = queryBuilder
                .Select(typeof(TypesTestEntity))
                .From(typeof(TypesTestEntity))
                .Where((TypesTestEntity e, int id) => e.Id == id)
                .Build();
        }

        [Fact]
        public void FullSqlBuildWitoutSyntaxErrosInAllParsers()
        {
            QueryBuilder queryBuilder = new QueryBuilder();

            var sql = queryBuilder
                .Select(typeof(TypesTestEntity))
                .From(typeof(TypesTestEntity))
                .Where((TypesTestEntity e, int id) => e.Id == id)
                .Build();

            _parsers.ToList().ForEach(p =>
            {
                var errors = p.Parse(sql);

                Assert.Null(errors);
            });
        }

        public void Dispose()
        {
            
        }
    }
}