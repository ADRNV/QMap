using AutoFixture;
using QMap.Core.Dialects;
using QMap.SqlServer;
using QMap.Tests.Share.DataBase;
using QMap.Tests.Share.Helpers.Sql;

namespace QMap.SqlBuilder.Tests
{
    public class SqlBuilderParseTests
    {

        private IList<IParser> _parsers;

        private IList<ISqlDialect> _dialects;

        public SqlBuilderParseTests(IList<IParser> parsers, IList<ISqlDialect> dialects)
        {
            _parsers = parsers;

            _dialects = dialects;
        }

        [Trait("SQL", "Full")]
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

        [Trait("SQL", "Insert")]
        [Fact]
        public void FullSqlInsertBuildWitoutSyntaxErrosInAllParsers()
        {
            StatementsBuilders queryBuilder = new(new TSqlDialect());

            var entity = new Fixture()
                 .Create<TypesTestEntity>();
            //Connections ?
            ////var sql = queryBuilder
            ////   .BuildInsert(entity);


            //_parsers.ToList().ForEach(p =>
            //{
            //    var errors = p.Parse(sql);

            //    Assert.Null(errors);
            //});
        }

        [Trait("SQL", "Delete")]
        [Fact]
        public void BuildDeleteNoThrowsErrors()
        {
            StatementsBuilders queryBuilder = new StatementsBuilders(new SqlDialectBase());

            var sql = queryBuilder
                .Delete<TypesTestEntity>()
                .From(typeof(TypesTestEntity))
                .Build();
        }

        [Trait("SQL", "Delete")]
        [Fact]
        public void BuildDeleteThrowInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                StatementsBuilders queryBuilder = new StatementsBuilders(new SqlDialectBase());

                var sql = queryBuilder
                    .Delete<TypesTestEntity>()
                    .Build();
            });
        }

        [Trait("SQL", "Delete")]
        [Fact]
        public void DeleteSqlBuidWitoutSyntaxErrosInAllParsers()
        {
            StatementsBuilders queryBuilder = new(new TSqlDialect());

            var sql = queryBuilder
            .Delete<TypesTestEntity>()
                .From(typeof(TypesTestEntity))
                .Build();

            _parsers.ToList().ForEach(p =>
            {
                var errors = p.Parse(sql);

                Assert.Null(errors);
            });
        }

        [Trait("SQL", "Delete")]
        [Fact]
        public void DeleteFullSqlBuidWitoutSyntaxErrosInAllParsers()
        {
            StatementsBuilders queryBuilder = new(new SqlDialectBase());

            var sql = queryBuilder
                .Delete<TypesTestEntity>()
                .From(typeof(TypesTestEntity))
                .Where<TypesTestEntity>((TypesTestEntity t) => t.Id < 0)
                .Build();

            _parsers.ToList().ForEach(p =>
            {
                var errors = p.Parse(sql);

                Assert.Null(errors);
            });
        }
    }
}
