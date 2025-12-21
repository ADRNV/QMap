using AutoFixture;
using QMap.Core.Dialects;
using QMap.SqlServer;
using QMap.Tests.Share.Common.Fakes.Connections;
using QMap.Tests.Share.DataBase;
using QMap.Tests.Share.Helpers.Sql;
using System.Linq.Expressions;
using System.Reflection;

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
                .Where<TypesTestEntity>((TypesTestEntity e) => 1 == 1, out var parameters)
                .Build();

            _parsers.ToList().ForEach(p =>
            {
                var errors = p.Parse(sql);

                Assert.Null(errors);
            });
        }

        [Trait("SQL", "Insert")]
        [Fact]
        public void FullSqlInsertWithAllPropertiesBuildWitoutSyntaxErrosInAllParsers()
        {
            var connectionFake = FakeConnectionExtensions.Create();

            StatementsBuilders queryBuilder = new StatementsBuilders(connectionFake.Dialect);

            var entity = new Fixture()
                 .Build<TypesTestEntity>()
                 .Without(e => e.Id)
                 .Create<TypesTestEntity>();

            var sql = queryBuilder
                .BuildInsert(connectionFake, out var _, entity);

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
            var connectionFake = FakeConnectionExtensions.Create();

            StatementsBuilders queryBuilder = new StatementsBuilders(connectionFake.Dialect);

            var entity = new Fixture()
                 .Build<TypesTestEntity>()
                 .Without(e => e.Id)
                 .Create<TypesTestEntity>();

            var sql = queryBuilder
                .BuildInsert(connectionFake, out var _, entity, (p) => p.Id);

            _parsers.ToList().ForEach(p =>
            {
                var errors = p.Parse(sql);

                Assert.Null(errors);
            });
        }

        [Trait("SQL", "Update")]
        [Fact]
        public void BuildUpdateNoThrowsErros()
        {
            var connectionFake = FakeConnectionExtensions.Create();

            StatementsBuilders queryBuilder = new StatementsBuilders(connectionFake.Dialect);

            var entity = new Fixture()
                 .Build<TypesTestEntity>()
                 .Without(e => e.Id)
                 .Create<TypesTestEntity>();

            entity.IntField = 2048;

            var sql = queryBuilder
                .Update<TypesTestEntity, int>(connectionFake, out var _, () => entity.IntField, entity.IntField)
                .Where<TypesTestEntity>((TypesTestEntity e) => e.Id > 0, out var parameters)
                .Build();

            _parsers.ToList().ForEach(p =>
            {
                var errors = p.Parse(sql);

                Assert.Null(errors);
            });
        }

        [Trait("SQL", "Update")]
        [Fact]
        public void BuildUpdateThrowsInvalidOperationExceptionWhenGetNoProperty()
        {
            var connectionFake = FakeConnectionExtensions.Create();

            StatementsBuilders queryBuilder = new StatementsBuilders(connectionFake.Dialect);

            var entity = new Fixture()
                 .Build<TypesTestEntity>()
                 .Without(e => e.Id)
                 .Create();

            Assert.Throws<InvalidOperationException>(() =>
            {
               var sql = queryBuilder
                .Update<TypesTestEntity, string>(connectionFake, out var _, () => entity.ToString(), "")
                .Where<TypesTestEntity>((TypesTestEntity e) => e.Id > 0, out var parameters)
                .Build();
            });
        }

        [Trait("SQL", "Delete")]
        [Fact]
        public void BuildDeleteNoThrowsErrors()
        {
            StatementsBuilders queryBuilder = new StatementsBuilders(new SqlDialectBase());

            var sql = queryBuilder
                .Delete<TypesTestEntity>(out var _)
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
                    .Delete<TypesTestEntity>(out var _)
                    .Build();
            });
        }

        [Trait("SQL", "Delete")]
        [Fact]
        public void DeleteSqlBuidWitoutSyntaxErrosInAllParsers()
        {
            StatementsBuilders queryBuilder = new(new TSqlDialect());

            var sql = queryBuilder
            .Delete<TypesTestEntity>(out var _)
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
                .Delete<TypesTestEntity>(out var _)
                .From(typeof(TypesTestEntity))
                .Where<TypesTestEntity>((TypesTestEntity t) => t.Id < 0, out var parameters)
                .Build();

            _parsers.ToList().ForEach(p =>
            {
                var errors = p.Parse(sql);

                Assert.Null(errors);
            });
        }
    }
}
