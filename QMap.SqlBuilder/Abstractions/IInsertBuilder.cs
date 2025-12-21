using System.Linq.Expressions;
using System.Reflection;

namespace QMap.SqlBuilder.Abstractions
{
    public interface IInsertBuilder : IQueryBuilder
    {
        IInsertBuilder BuildInsert<T>(T entity);
        public IInsertBuilder BuildInsertExcept<T, TProperty>(T entity, Expression<Func<T, TProperty>> exceptPropsFilter);
    }
}
