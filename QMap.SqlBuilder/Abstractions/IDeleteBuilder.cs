using System.Linq.Expressions;

namespace QMap.SqlBuilder.Abstractions
{
    public interface IDeleteBuilder : IQueryBuilder
    {
        public IDeleteBuilder BuildDelete<T>();

        public IFromBuilder BuildFrom();
    }
}
