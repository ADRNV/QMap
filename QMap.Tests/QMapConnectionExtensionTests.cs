using AutoFixture;
using QMap.Tests.Common;
using QMap.Tests.Common.DataBase;

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

                connection.Close();

                Assert.Equivalent(expectedEntity, factEntity);
            });
        }
    }
}