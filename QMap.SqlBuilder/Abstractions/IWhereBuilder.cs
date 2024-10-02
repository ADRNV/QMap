using QMap.Core.Dialects;
using System.Linq.Expressions;

namespace QMap.SqlBuilder.Abstractions
{
    public interface IWhereBuilder : IQueryBuilder
    {
        IWhereBuilder BuildWhere<T>(IFromBuilder quryBuilder, LambdaExpression lambdaExpression);
    }
}
