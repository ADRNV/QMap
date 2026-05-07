using QMap.Core.Dialects;
using QMap.Tests.Share.DataBase;

namespace QMap.SqlBuilder.Tests
{
    public class SqlBuilderSqlInjectionTests
    {
        [Fact]
        public void Select_Should_Not_Drop_Statemant()
        {
            var builder = new StatementsBuilders(new SqlDialectBase());

            var result = builder.Select(typeof(TypesTestEntity))
                .From(typeof(TypesTestEntity))
                .Where<TypesTestEntity>((TypesTestEntity t) => t.StringField == "\'DROP TABLE TypesTestEntity;--'", out var parameters);

            Assert.Contains("@", result.Sql); // либо pattern для placeholders
            Assert.DoesNotContain("DROP TABLE", result.Sql, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Insert_Should_Not_Drop_Statemant()
        {
            var builder = new StatementsBuilders(new SqlDialectBase());

            var result = builder.Select(typeof(TypesTestEntity))
                .From(typeof(TypesTestEntity))
                .Where<TypesTestEntity>((TypesTestEntity t) => t.StringField == "\'DROP TABLE TypesTestEntity;--'", out var parameters);

            Assert.Contains("@", result.Sql); // либо pattern для placeholders
            Assert.DoesNotContain("DROP TABLE", result.Sql, StringComparison.OrdinalIgnoreCase);

        }
    }
}
