using QMap.Core.Mapping;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;

namespace QMap.Mapping
{
    public class ProjectionMapper<TSource, TResult> : IProjectionMapper<TResult>
    {
        private readonly Func<IDataReader, TResult> _mapFunc;

        public ProjectionMapper(Expression<Func<TSource, TResult>> selector)
        {
            _mapFunc = BuildMapFunc(selector);
        }

        public IEnumerable<TResult> Map(IDataReader dataReader)
        {
            while (dataReader.Read())
                yield return _mapFunc(dataReader);
        }

        private static Func<IDataReader, TResult> BuildMapFunc(Expression<Func<TSource, TResult>> selector)
        {
            var readerParam = Expression.Parameter(typeof(IDataReader), "reader");
            var visitor = new DataReaderProjectionVisitor(selector.Parameters[0], readerParam);
            var newBody = visitor.Visit(selector.Body);

            return Expression.Lambda<Func<IDataReader, TResult>>(newBody, readerParam).Compile();
        }

        private class DataReaderProjectionVisitor : ExpressionVisitor
        {
            private readonly ParameterExpression _sourceParam;
            private readonly ParameterExpression _readerParam;

            private static readonly MethodInfo _readValueMethod =
                typeof(ProjectionHelper).GetMethod(nameof(ProjectionHelper.ReadValue))!;

            public DataReaderProjectionVisitor(ParameterExpression sourceParam, ParameterExpression readerParam)
            {
                _sourceParam = sourceParam;
                _readerParam = readerParam;
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                if (node.Expression == _sourceParam && node.Member is PropertyInfo prop)
                {
                    return Expression.Call(
                        _readValueMethod.MakeGenericMethod(prop.PropertyType),
                        _readerParam,
                        Expression.Constant(prop.Name));
                }

                return base.VisitMember(node);
            }
        }
    }

    internal static class ProjectionHelper
    {
        public static T ReadValue<T>(IDataReader reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            var value = reader.GetValue(ordinal);

            if (value == DBNull.Value || value is null)
                return default!;

            if (value is T typed)
                return typed;

            var targetType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
            return (T)Convert.ChangeType(value, targetType);
        }
    }
}
