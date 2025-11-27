using QMap.Core.Mapping;
using System.Linq.Expressions;
using System.Reflection;

namespace QMap.SqlBuilder.Abstractions
{
    public interface IUpdateBuilder : IQueryBuilder
    {
        IUpdateBuilder BuildUpdate<T, V>(Expression<Func<V>> propertySelector, V value);
    }
}
