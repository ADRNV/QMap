using System.Linq.Expressions;
namespace QMap.SqlBuilder
{
    public interface IQueryBuilder
    {
        public string BuildWhere(LambdaExpression expression);
    }
}
