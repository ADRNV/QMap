using System.Linq.Expressions;

namespace QMap.SqlBuilder.Abstractions
{
    public interface ISelectBuilder : IQueryBuilder
    {
        public ISelectBuilder BuidSelect(Type type);

        public ISelectBuilder BuidSelect(Expression type);

    }
}
