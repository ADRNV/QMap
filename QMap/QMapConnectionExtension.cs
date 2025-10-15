using QMap.Core;
using QMap.Core.Mapping;
using QMap.Mapping;
using QMap.SqlBuilder;
using System.Collections.Concurrent;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;

namespace QMap
{
    public static class QMapConnectionExtension
    {
        private readonly static ConcurrentDictionary<Type, IEntityMapper> _mappersCache = new();

        /// <summary>
        /// Execute query and map to <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection">Connection</param>
        /// <param name="sql">Qery text</param>
        /// <param name="customMapper">Custom mapper if required, else uses base implementation"/></param>
        /// <returns></returns>
        public static IEnumerable<T> Query<T>(this IQMapConnection connection, string sql, IEntityMapper? customMapper = null) where T : class, new()
        {
            var mapper = _mappersCache.GetOrAdd(typeof(T),
                (customMapper is null ? new EntityMapper() : customMapper));

            var queryMapper = new QueryMapperBase(mapper);

            var command = connection.CreateCommand();
            command.CommandText = sql;

            return queryMapper.Map<T>(command.ExecuteReader());
        }

        public static IEnumerable<T> Where<T>(this IQMapConnection connection, LambdaExpression predicate, IEntityMapper? customMapper = null) where T : class, new()
        {
            var mapper = _mappersCache.GetOrAdd(typeof(T),
                (customMapper is null ? new EntityMapper() : customMapper));

            var queryMapper = new QueryMapperBase(mapper);

            var command = connection.CreateCommand();
            var sql = new StatementsBuilders(connection.Dialect)
                .Select(typeof(T))
                .From(typeof(T))
                .Where<T>(predicate, out var parameters)
                .Build();

            command.CommandText = sql;

            connection.Dialect.BuildParameters(command, parameters);

            return queryMapper.Map<T>(command.ExecuteReader());
        }

        public static void Insert<T>(this IQMapConnection connection, T entity, Func<PropertyInfo, bool> exceptProperty)
        {
            var command = connection.CreateCommand();
            var sql = new StatementsBuilders(connection.Dialect)
                .BuildInsert(connection, out var parameters, entity, exceptProperty);

            command.CommandText = sql;

            connection.Dialect.BuildParameters(command, parameters);

            command.ExecuteNonQuery();
        }

        public static void Insert<T>(this IQMapConnection connection, T entity)
        {
            var command = connection.CreateCommand();
            var sql = new StatementsBuilders(connection.Dialect)
                .BuildInsert(connection, out var parameters, entity);

            command.CommandText = sql;

            connection.Dialect.BuildParameters(command, parameters);

            command.ExecuteNonQuery();
        }

        public static void Update<T, V>(this IQMapConnection connection, Expression<Func<V>> propertySelector, V value, LambdaExpression predicate) where T : class, new()
        {
            var command = connection.CreateCommand();

            var sql = new StatementsBuilders(connection.Dialect)
                .Update<T,V>(connection, out var parameters, propertySelector, value)
                .Where<T>(predicate, out var parameters1)
                .Build();
            
            command.CommandText = sql;

            var allParameters = parameters.AsEnumerable().Concat(parameters1).ToDictionary((p) => p.Key, (p) => p.Value);

            connection.Dialect.BuildParameters(command, allParameters);

            command.ExecuteNonQuery();
        }

        public static void Delete<T>(this IQMapConnection connection, LambdaExpression predicate, IEntityMapper? customMapper = null) where T : class, new()
        {
            var command = connection.CreateCommand();
            var sql = new StatementsBuilders(connection.Dialect)
                .Delete<T>()
                .From(typeof(T))
                .Where<T>(predicate, out var parameters)
                .Build();

            command.CommandText = sql;

            command.ExecuteNonQuery();
        }
    }
}
