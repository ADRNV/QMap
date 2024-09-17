using QMap.SqlBuilder.Abstractions;
using System.Linq.Expressions;

namespace QMap.SqlBuilder
{
    public static class QueryBuilderExtensions
    {
        public static IFromBuilder From(this ISelectBuilder queryBuilder, Type entity, params Type[] entities)
        {
            return new FromBuilder()
                .BuildFrom(queryBuilder, entity, entities);
        }

        public static IQueryBuilder Where(this IFromBuilder queryBuilder, LambdaExpression expression)
        {
            return new WhereBuilder()
               .BuildWhere(queryBuilder, expression);
        }

        public static ISelectBuilder Select(this IQueryBuilder queryBuilder, Type entity)
        {
            return new SelectBuilder()
                 .BuidSelect(entity);
        }

        public static ISelectBuilder Select(Expression entity)
        {
            return new SelectBuilder()
                 .BuidSelect(entity);
        }

        public static string Build(this IQueryBuilder queryBuilder)
        {
            return queryBuilder.Sql;
        }
    }
}
