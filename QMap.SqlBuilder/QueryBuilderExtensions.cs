using QMap.Core;
using QMap.SqlBuilder.Abstractions;
using System.Linq.Expressions;
using System.Reflection;

namespace QMap.SqlBuilder
{
    public static class QueryBuilderExtensions
    {
        public static IFromBuilder From(this ISelectBuilder queryBuilder, Type entity, params Type[] entities)
        {
            return new FromBuilder(queryBuilder.SqlDialect)
                .BuildFrom(queryBuilder, entity, entities);
        }

        public static IFromBuilder From(this IDeleteBuilder queryBuilder, Type entity)
        {
            return new FromBuilder(queryBuilder.SqlDialect)
                .BuildFrom(queryBuilder, entity, null!);
        }

        public static IQueryBuilder Where<T>(this IFromBuilder queryBuilder, LambdaExpression expression)
        {
            return new WhereBuilder(queryBuilder.SqlDialect)
               .BuildWhere<T>(queryBuilder, expression);
        }

        public static IQueryBuilder Where<T>(this IUpdateBuilder queryBuilder, LambdaExpression expression)
        {
            return new WhereBuilder(queryBuilder.SqlDialect)
               .BuildWhere<T>(queryBuilder, expression);
        }

        public static ISelectBuilder Select(this IQueryBuilder queryBuilder, Type entity)
        {
            return new SelectBuilder(queryBuilder.SqlDialect)
                 .BuidSelect(entity);
        }

        public static IUpdateBuilder Update<T, V>(this IQueryBuilder queryBuilder, IQMapConnection connection, out Dictionary<string, object> parameters, T entity, Expression<Func<V>> propertySelectors)
        {
            var builder = new UpdateBuilder(queryBuilder.SqlDialect)
                .BuildUpdate(entity, propertySelectors);

            parameters = builder.Parameters;

            return builder;
        }

        public static IDeleteBuilder Delete<T>(this IQueryBuilder queryBuilder)
        {
            return new DeleteBuilder(queryBuilder.SqlDialect)
                .BuildDelete<T>();
        }

        public static string Build(this IQueryBuilder queryBuilder)
        {
            return queryBuilder.Build();
        }

        public static string BuildInsert<T>(this IQueryBuilder queryBuilder, IQMapConnection connection, out Dictionary<string, object> parameters, T entity, Func<PropertyInfo, bool> exceptProperty)
        {
            var builder = new InsertBuilder(queryBuilder.SqlDialect)
                .BuildInsertExcept(entity, exceptProperty);

            parameters = builder.Parameters;

            return builder.Build();
        }

        public static string BuildInsert<T>(this IQueryBuilder queryBuilder, IQMapConnection connection, out Dictionary<string, object> parameters, T entity)
        {
            var builder = new InsertBuilder(queryBuilder.SqlDialect)
                .BuildInsert(entity);

            parameters = builder.Parameters;

            return builder.Build();
        }
    }
}
