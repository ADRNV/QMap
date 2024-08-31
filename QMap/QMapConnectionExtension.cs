﻿using QMap.Core;
using QMap.Core.Mapping;
using QMap.Mapping;
using System.Collections.Concurrent;

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
    }
}
