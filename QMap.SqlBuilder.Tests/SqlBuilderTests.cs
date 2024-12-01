using AutoFixture;
using QMap.Core.Dialects;
using QMap.SqlServer;
using QMap.Tests.Share.DataBase;
using QMap.Tests.Share.Helpers.Sql;
using System.Data.SqlClient;

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

        public void BuildWithTerminalSttementNoThrowsErrors()
        {
            StatementsBuilders queryBuilder = new(new SqlDialectBase());

            queryBuilder
                .Select(typeof(TypesTestEntity))
                .From(typeof(TypesTestEntity))
                .Build();
        }

        [Fact]
        public void BuildWithoutWhereNoThrowsErrors()
        {
            StatementsBuilders queryBuilder = new(new SqlDialectBase());

            var s = queryBuilder
                .Select(typeof(TypesTestEntity))
                .From(typeof(TypesTestEntity))
                .Build();
        }

        [Fact]
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
        public void FullSqlBuildWitoutSyntaxErrosInAllParsers()
        {
            //TODO
            //1. Find parser for Postgres, MySQL, etc
            //2. What with reduce and MS SQL bit ?!
            StatementsBuilders queryBuilder = new(new TSqlDialect());

            var sql = queryBuilder
                .Select(typeof(TypesTestEntity))
                .From(typeof(TypesTestEntity))
                .Where<TypesTestEntity>((TypesTestEntity e) => 1 == 1)
                .Build();

            _parsers.ToList().ForEach(p =>
            {
                var errors = p.Parse(sql);

                Assert.Null(errors);
            });
        }

        [Fact]
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

        [Fact]
        public void FullSqlInsertBuildWitoutSyntaxErrosInAllParsers()
        {
            StatementsBuilders queryBuilder = new(new TSqlDialect());

            var entity = new Fixture()
                 .Create<TypesTestEntity>();

            ////var sql = queryBuilder
            ////   .BuildInsert(entity);


            //_parsers.ToList().ForEach(p =>
            //{
            //    var errors = p.Parse(sql);

            //    Assert.Null(errors);
            //});
        }
    }
}