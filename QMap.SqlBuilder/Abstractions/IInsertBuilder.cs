using System.Reflection;

namespace QMap.SqlBuilder.Abstractions
{
    public interface IInsertBuilder : IQueryBuilder
    {
        IInsertBuilder BuildInsert<T>(T entity);
        public IInsertBuilder BuildInsertExcept<T>(T entity, Func<PropertyInfo, bool> exceptPropsFilter);
    }
}
