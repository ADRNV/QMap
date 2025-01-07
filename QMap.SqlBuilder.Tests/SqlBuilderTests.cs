using AutoFixture;
using QMap.Core.Dialects;
using QMap.SqlServer;
using QMap.Tests.Share.DataBase;
using QMap.Tests.Share.Helpers.Sql;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace QMap.SqlBuilder.Tests
{
    public class SqlBuilderTests
    {
        private IList<IParser> _parsers;

        private IList<ISqlDialect> _dialects;

        public SqlBuilderTests(IList<IParser> parsers, IList<ISqlDialect> dialects)
        {
            _parsers = parsers;

            _dialects = dialects;
        }

        [Fact]
        [Trait("SQL", "Full")]
        public void BuildNonTerminalStatementThrowsInvalidOperationException()
        {
            _dialects.ToList().ForEach((d) =>
            {
                StatementsBuilders queryBuilder = new StatementsBuilders(d);

                Assert.Throws<InvalidOperationException>(() =>
                {
                    queryBuilder.Build();
                });
            });
        }

        [Trait("SQL", "Full")]
        public void BuildWithTerminalSttementNoThrowsErrors()
        {
            StatementsBuilders queryBuilder = new(new SqlDialectBase());

            queryBuilder
                .Select(typeof(TypesTestEntity))
                .From(typeof(TypesTestEntity))
                .Build();
        }

        [Fact]
        [Trait("SQL", "Where")]
        public void BuildWithoutWhereNoThrowsErrors()
        {
            StatementsBuilders queryBuilder = new(new SqlDialectBase());

            var s = queryBuilder
                .Select(typeof(TypesTestEntity))
                .From(typeof(TypesTestEntity))
                .Build();
        }
        
        [Fact]
        [Trait("SQL", "Full")]
        public void FullSqlBuildNoThrowsErrors()
        {
            StatementsBuilders queryBuilder = new(new SqlDialectBase());

            queryBuilder
                .Select(typeof(TypesTestEntity))
                .From(typeof(TypesTestEntity))
                .Where<TypesTestEntity>((TypesTestEntity e) => 1 == 1)
                .Build();
        }

        [Fact]
        [Trait("SQL", "Full")]
        public void FullSqlBuildWithParamsNoThrowsErrors()
        {
            StatementsBuilders queryBuilder = new StatementsBuilders(new SqlDialectBase());

            var sql = queryBuilder
                .Select(typeof(TypesTestEntity))
                .From(typeof(TypesTestEntity))
                .Where<TypesTestEntity>((TypesTestEntity e) => e.Id == 1)
                .Build();
        }

        [Fact]
        [Trait("SQL", "Full")]
        public void BuildInsertNoThrowsErrors()
        {
            StatementsBuilders queryBuilder = new StatementsBuilders(new SqlDialectBase());

            var entity = new Fixture()
                .Create<TypesTestEntity>();

            var parameters = new Dictionary<string, object>();

            var connection = new SqlConnection("Server=localhost;Database=TestDb;Integrated Security=true;TrustServerCertificate=Yes;Encrypt=false")
                .Adapt();

            var sql = queryBuilder
                .BuildInsert(connection, out parameters, entity);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(42.0f)]
        [InlineData(42.0d)]
        [InlineData("1")]
        [InlineData(false)]
        [Trait("SQL", "Full")]
        public void WhereMapExpressionWithInternalValueRights(object value)
        {
            _parsers.ToList().ForEach(c =>
            {
                StatementsBuilders queryBuilder = new StatementsBuilders(new SqlDialectBase());

                var external = value;

                queryBuilder
                    .Select(typeof(TypesTestEntity))
                    .From(typeof(TypesTestEntity))
                    .Where<TypesTestEntity>((TypesTestEntity t) => t.StringField == external);
            });
        }
    }
}