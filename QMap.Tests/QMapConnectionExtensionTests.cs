using AutoFixture;
using Azure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using QMap.Core.Dialects;
using QMap.SqlBuilder;
using QMap.Tests.Share;
using QMap.Tests.Share.Common;
using QMap.Tests.Share.DataBase;
using System.Data.Common;
using System.Data.SqlClient;
using Xunit.Abstractions;
using static Dapper.SqlMapper;

namespace QMap.Tests
{
    public class QMapConnectionExtensionTests : IDisposable
    {

        private List<IQMapConnectionFactory> _connectionFactories;

        private static TestContext _context;

        public QMapConnectionExtensionTests(IEnumerable<IQMapConnectionFactory> connectionFactories)
        {
            _connectionFactories = connectionFactories
                .ToList();
        }

        public void Dispose()
        {
            
            //_context.Database.EnsureDeleted();
            //_context.Dispose();   
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

                _context = c.GetDbContext<TestContext>();

                _context.Database.EnsureDeleted();
                _context.Database.EnsureCreated();

                _context.TypesTestEntity.AddRange(expectedEntity);

                _context.SaveChanges();

                IEnumerable<TypesTestEntity> factEntity;

                using var connection = c.Create();

                connection.Open();

                factEntity = connection.Query<TypesTestEntity>("select * from TypesTestEntity");

                Assert.Equivalent(expectedEntity, factEntity);

                connection.Close();

                _context.Database.EnsureDeleted();
            });
        }

        [Fact]
        public void QueryReturnsEmptyEnumerableWhenNotRowsOfType()
        {
            _connectionFactories.ForEach(c =>
            {
                var expectedEntities = c.GetDbContext<TestContext>().TypesTestEntity
                .Where((e) => false)
                .AsEnumerable();

                IEnumerable<TypesTestEntity> factEntity;

                var context = c.GetDbContext<TestContext>();

                context.Database.EnsureCreated();

                using var connection = c.Create();

                connection.Open();

                factEntity = connection.Query<TypesTestEntity>("select * from TypesTestEntity where 1 = 0");

                Assert.Equivalent(expectedEntities, factEntity);

                connection.Close();

                context.Database.EnsureDeleted();
            });
        }

        [Fact]
        public void QueryThrowsExceptionWhenWringSql()
        {
            _connectionFactories.ForEach(c =>
            {

                var context = c.GetDbContext<TestContext>();

                context.Database.EnsureCreated();

                using var connection = c.Create();

                connection.Open();

                try
                {
                    connection.Query<TypesTestEntity>("select * from where 1 = 0");

                }
                catch (SqlException sqle)
                {
                    Assert.True(true);
                }
                catch (SqliteException sqliex)
                {
                    Assert.True(true);
                }
                catch (Exception ex)
                {
                    Assert.Fail(ex.Message);
                }

                connection.Close();

                context.Database.EnsureDeleted();
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

                var context = c.GetDbContext<TestContext>();

                context.Database.EnsureCreated();

                context.TypesTestEntity.AddRange(expectedEntity);

                context.SaveChanges();

                using var connection = c.Create();

                connection.Open();

                Assert.Throws<InvalidOperationException>(() =>
                {
                    connection.Query<WrongEntity>("select * from TypesTestEntity").ToArray();
                });

                connection.Close();

                context.Database.EnsureDeleted();
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

                var context = c.GetDbContext<TestContext>();
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.TypesTestEntity.AddRange(expectedEntity);

                context.SaveChanges();

                IEnumerable<TypesTestEntity> factEntity;

                using var connection = c.Create();
                connection.Open();
                //TSQL errors when parse True constant
                factEntity = connection.Where<TypesTestEntity>((TypesTestEntity e) => e.Id != 0);

                Assert.Equivalent(expectedEntity.ToArray(), factEntity.ToArray());

                connection.Close();

                context.Database.EnsureDeleted();
            });
        }

        [Fact]
        public void InsertWithoutPropertyExecutesWithoutErrors()
        {
            _connectionFactories.ForEach(c =>
            {
                var entity = new Fixture()
               .Build<TypesTestEntity>()
               .Without(t => t.Id)
               .Create();

                var context = c.GetDbContext<TestContext>();

                context.Database.EnsureCreated();

                using var connection = c.Create();

                connection.Open();

                connection.Insert(entity, p => p.Name == "Id");

                connection.Close();

                context.Database.EnsureDeleted();
            });
        }

        [Fact]
        public void InsertWithIdentityThrowsException()
        {
            _connectionFactories.ForEach(c =>
            {
                var entity = new Fixture()
               .Build<TypesTestEntity>()
               .Without(t => t.Id)
               .Create();

                var context = c.GetDbContext<TestContext>();

                context.Database.EnsureCreated();

                using var connection = c.Create();            

                connection.Open();

                try
                {
                    connection.Insert(entity);
                }
                catch (SqlException sqle)
                {
                    Assert.True(true);
                }
                catch (SqliteException sqliex)
                {
                    Assert.True(true);
                }
                catch(Exception ex)
                {
                    Assert.Fail(ex.Message); 
                }

                connection.Close();

                context.Database.EnsureDeleted();
            });
        }

        [Fact]
        public void UpdateNoThrowsErrors()
        {
            _connectionFactories.ForEach(c =>
            {
                var entity = new Fixture()
               .Build<TypesTestEntity>()
               .Without(t => t.Id)
               .Create();

                var context = c.GetDbContext<TestContext>();
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.TypesTestEntity.Add(entity);

                context.SaveChanges();

                using var connection = c.Create();
                
                connection.Open();
                var newValue = new Random().Next();
                entity.IntField = newValue;
                //TSQL errors when parse True constant
                connection.Update<TypesTestEntity, int>(() => entity.IntField, newValue, (TypesTestEntity e) => e.Id == entity.Id);

                Assert.True(context.TypesTestEntity.Find(new object[] {entity.Id}).IntField == newValue);

                connection.Close();

                context.Database.EnsureDeleted();
            });
        }

        [Fact]
        public void UpdateUpdatesOnlyMathedEntities()
        {
            _connectionFactories.ForEach(c =>
            {
                var entities = new Fixture()
               .Build<TypesTestEntity>()
               .Without(t => t.Id)
               .CreateMany(15);

                var context = c.GetDbContext<TestContext>();
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.TypesTestEntity.AddRange(entities);

                var updateEntity = entities.First();
                
                context.SaveChanges();

                var newValue = new Random().Next();

                updateEntity.IntField = newValue;

                var id = updateEntity.Id;

                using var connection = c.Create();

                connection.Open();

                //TSQL errors when parse True constant
                connection.Update<TypesTestEntity, int>(() => updateEntity.IntField, newValue,(TypesTestEntity e) => e.Id == updateEntity.Id);

                Assert.True(context.TypesTestEntity.Find(new object[] { updateEntity.Id }).IntField == updateEntity.IntField);
                Assert.True(context.TypesTestEntity.Where((e) => e.Id != updateEntity.Id).All(e => e.IntField != newValue));


                connection.Close();

                context.Database.EnsureDeleted();
            });
        }

        [Fact]
        public void DeleteNoThorwsErrors()
        {
            _connectionFactories.ForEach(c =>
            {
                var name = Guid.NewGuid().ToString();

                var context = c.GetDbContext<TestContext>();

                context.Database.EnsureCreated();

                using var connection = c.Create();

                connection.Open();

                connection.Delete<TypesTestEntity>((TypesTestEntity e) => e.StringField == name);

                connection.Close();

                context.Database.EnsureDeleted();
            });
        }

        [Fact]
        public void DeleteNotRevemoveNotMatchedEntityes()
        {
            _connectionFactories.ForEach(c =>
            {
                var name = Guid.NewGuid().ToString();

                var entity = new Fixture()
               .Build<TypesTestEntity>()
               .Without(t => t.Id)
               .With(t => t.StringField)
               .Create();

                var context = c.GetDbContext<TestContext>();

                context.Database.EnsureCreated();

                context.AddRange(entity);

                context.SaveChanges();

                var initialCount = context.TypesTestEntity.Count();

                using var connection = c.Create();

                connection.Open();

                connection.Delete<TypesTestEntity>((TypesTestEntity e) => e.StringField != name);

                var allEntitiesDeleted = initialCount != context.TypesTestEntity.Count();

                Assert.True(allEntitiesDeleted);

                connection.Close();

                context.Database.EnsureDeleted();
            });
        }

        [Fact]
        public void Select_Should_Not_Drop_Statemant()
        {
            var builder = new StatementsBuilders(new SqlDialectBase());

            _connectionFactories.ForEach(c =>
            {

                var context = c.GetDbContext<TestContext>();

                context.Database.EnsureCreated();

                using var connection = c.Create();

                connection.Open();

                connection.Where<TypesTestEntity>((TypesTestEntity t) => t.StringField == "\'DROP TABLE TypesTestEntity;--'");
                
                context.TypesTestEntity.Count();
            });
        }
    }
}