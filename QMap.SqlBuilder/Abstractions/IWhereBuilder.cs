using System.Linq.Expressions;

namespace QMap.SqlBuilder.Abstractions
{
    public interface IWhereBuilder : IQueryBuilder
    {
        IWhereBuilder BuildWhere(IFromBuilder quryBuilder, LambdaExpression lambdaExpression);
    }
}
