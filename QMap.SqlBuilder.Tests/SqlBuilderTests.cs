using QMap.Tests.Share.DataBase;
using QMap.Tests.Share.Helpers.Sql;

namespace QMap.SqlBuilder.Tests
{
    public class SqlBuilderTests
    {
        private IList<IParser> _parsers;

        public SqlBuilderTests(IList<IParser> parsers)
        {
            _parsers = parsers;
        }

        [Fact]
        public void BuildNonTerminalStatementThrowsInvalidOperationException()
        {
            StatementsBuilders queryBuilder = new StatementsBuilders();

            Assert.Throws<InvalidOperationException>(() =>
            {
                queryBuilder.Build();
            });
        }

        public void BuildWithTerminalSttementNoThrowsErrors()
        {
            StatementsBuilders queryBuilder = new StatementsBuilders();

            queryBuilder
                .Select(typeof(TypesTestEntity))
                .From(typeof(TypesTestEntity))
                .Build();
        }

        [Fact]
        public void BuildWithoutWhereNoThrowsErrors()
        {
            StatementsBuilders queryBuilder = new StatementsBuilders();

            queryBuilder
                .Select(typeof(TypesTestEntity))
                .From(typeof(TypesTestEntity));
        }

        [Fact]
        public void FullSqlBuildNoThrowsErrors()
        {
            StatementsBuilders queryBuilder = new StatementsBuilders();

            queryBuilder
                .Select(typeof(TypesTestEntity))
                .From(typeof(TypesTestEntity))
                .Where((TypesTestEntity e) => 1 == 1);
        }

        [Fact]
        public void FullSqlBuildWithParamsNoThrowsErrors()
        {
            StatementsBuilders queryBuilder = new StatementsBuilders();

            var sql = queryBuilder
                .Select(typeof(TypesTestEntity))
                .From(typeof(TypesTestEntity))
                .Where((TypesTestEntity e, int id) => e.Id == id)
                .Build();
        }

        [Fact]
        public void FullSqlBuildWitoutSyntaxErrosInAllParsers()
        {
            StatementsBuilders queryBuilder = new StatementsBuilders();

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
    }
}