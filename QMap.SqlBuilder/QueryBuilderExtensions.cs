using System.Linq.Expressions;

namespace QMap.SqlBuilder
{
    public static class QueryBuilderExtensions
    {
        public static IQueryBuilder From(this IQueryBuilder queryBuilder, Type entity, params Type[] entities)
        {
            queryBuilder.BuildFrom(entity, entities);

            return queryBuilder;
        }

        public static IQueryBuilder Where(this IQueryBuilder queryBuilder, LambdaExpression expression)
        {
            queryBuilder.BuildWhere(expression);

            return queryBuilder;
        }

        public static IQueryBuilder Select(this IQueryBuilder queryBuilder, Type entity)
        {
            queryBuilder.BuidSelect(entity);

            return queryBuilder;
        }

        public static string Build(this IQueryBuilder queryBuilder)
        {
            return queryBuilder.Sql;
        }
    }
}
