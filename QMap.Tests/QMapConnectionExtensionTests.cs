using AutoFixture;
using QMap.Tests.Share;
using QMap.Tests.Share.DataBase;
using System.Data.SqlClient;

namespace QMap.Tests
{
    public class QMapConnectionExtensionTests : IDisposable
    {
        private TestContext _testContext;

        private List<IQMapConnectionFactoryBase> _connectionFactories;

        public QMapConnectionExtensionTests(TestContext testContext, IEnumerable<IQMapConnectionFactoryBase> connectionFactories)
        {
            _connectionFactories = connectionFactories
                .ToList();

            _testContext = testContext;
            _testContext.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _testContext.Database.EnsureDeleted();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(100)]
        public void QueryMapRowRights(int count)
        {
            _connectionFactories.ForEach(c =>
            {
                var expectedEntity = new Fixture()
               .Build<TypesTestEntity>()
               .Without(t => t.Id)
               .CreateMany(count);

                _testContext.TypesTestEntity.AddRange(expectedEntity);

                _testContext.SaveChanges();

                IEnumerable<TypesTestEntity> factEntity;

                using var connection = c.Create();

                connection.Open();

                factEntity = connection.Query<TypesTestEntity>("select * from TypesTestEntity");

                Assert.Equivalent(expectedEntity, factEntity);

                connection.Close();
            });
        }

        [Fact]
        public void QueryReturnsEmptyEnumerableWhenNotRowsOfType()
        {
            _connectionFactories.ForEach(c =>
            {
                var expectedEntities = _testContext.TypesTestEntity
                .Where((e) => false)
                .AsEnumerable();

                IEnumerable<TypesTestEntity> factEntity;

                using var connection = c.Create();

                connection.Open();

                factEntity = connection.Query<TypesTestEntity>("select * from TypesTestEntity where 1 = 0");

                Assert.Equivalent(expectedEntities, factEntity);

                connection.Close();
            });
        }

        [Fact]
        public void QueryThrowsExceptionWhenWringSql()
        {
            _connectionFactories.ForEach(c =>
            {
                using var connection = c.Create();

                connection.Open();

                Assert.Throws<SqlException>(() =>
                {
                    connection.Query<TypesTestEntity>("select * from where 1 = 0");
                });

                connection.Close();
            });
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(0)]
        public void QueryThrowsExceptionWhenNotMatchEntity(int count)
        {
            _connectionFactories.ForEach(c =>
            {
                var expectedEntity = new Fixture()
               .Build<TypesTestEntity>()
               .Without(t => t.Id)
               .CreateMany(count);

                _testContext.TypesTestEntity.AddRange(expectedEntity);

                _testContext.SaveChanges();

                using var connection = c.Create();

                connection.Open();

                Assert.Throws<InvalidOperationException>(() =>
                {
                    connection.Query<WrongEntity>("select * from TypesTestEntity").ToArray();
                });

                connection.Close();
            });
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(100)]
        public void WhereMapRowRights(int count)
        {
            _connectionFactories.ForEach(c =>
            {
                var expectedEntity = new Fixture()
               .Build<TypesTestEntity>()
               .Without(t => t.Id)
               .CreateMany(count);

                _testContext.TypesTestEntity.AddRange(expectedEntity);

                _testContext.SaveChanges();

                IEnumerable<TypesTestEntity> factEntity;

                using var connection = c.Create();

                connection.Open();
                //TSQL errors when parse True constant
                factEntity = connection.Where<TypesTestEntity>((TypesTestEntity e) => e.Id == e.Id);

                Assert.Equivalent(expectedEntity, factEntity.ToArray());

                connection.Close();
            });
        }
    }
}