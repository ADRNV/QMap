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

        public static IQueryBuilder Where<T>(this IFromBuilder queryBuilder, LambdaExpression expression, out Dictionary<string, object> parameters)
        {
            var builder = new WhereBuilder(queryBuilder.SqlDialect)
               .BuildWhere<T>(queryBuilder, expression);

            parameters = builder.Parameters;

            return builder;
        }

        public static IQueryBuilder Where<T>(this IUpdateBuilder queryBuilder, LambdaExpression expression, out Dictionary<string, object> parameters)
        {
            var builder = new WhereBuilder(queryBuilder.SqlDialect)
               .BuildWhere<T>(queryBuilder, expression);
            
            parameters = builder.Parameters;

            return builder;
        }

        public static ISelectBuilder Select(this IQueryBuilder queryBuilder, Type entity)
        {
            return new SelectBuilder(queryBuilder.SqlDialect)
                 .BuidSelect(entity);
        }

        public static IUpdateBuilder Update<T, TProperty>(this IQueryBuilder queryBuilder, IQMapConnection connection, out Dictionary<string, object> parameters, Expression<Func<T, TProperty>> propertySelectors, TProperty value)
        {
            var builder = new UpdateBuilder(queryBuilder.SqlDialect)
                .BuildUpdate<T, TProperty>(propertySelectors, value);

            parameters = builder.Parameters;

            return builder;
        }

        public static IDeleteBuilder Delete<T>(this IQueryBuilder queryBuilder, out Dictionary<string, object> parameters)
        {
            var builder = new DeleteBuilder(queryBuilder.SqlDialect)
                .BuildDelete<T>();

            parameters = builder.Parameters;

            return builder;
        }

        public static string Build(this IQueryBuilder queryBuilder)
        {
            return queryBuilder.Build();
        }

        public static string BuildInsert<T, TProperty>(this IQueryBuilder queryBuilder, IQMapConnection connection, out Dictionary<string, object> parameters, T entity, Expression<Func<T, TProperty>> exceptProp)
        {
            var builder = new InsertBuilder(queryBuilder.SqlDialect)
                .BuildInsertExcept(entity, exceptProp);

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
