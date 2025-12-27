using QMap.Core.Mapping;
using System.Linq.Expressions;
using System.Reflection;

namespace QMap.SqlBuilder.Abstractions
{
    public interface IUpdateBuilder : IQueryBuilder
    {
        IUpdateBuilder BuildUpdate<T, TProperty>(Expression<Func<T, TProperty>> propertySelector, TProperty value);
    }
}
