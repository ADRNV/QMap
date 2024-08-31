using QMap.SqlBuilder.Visitors;
using System.Linq.Expressions;

namespace QMap.SqlBuilder
{
    public class QueryBuilder : IQueryBuilder
    {
        public string BuildWhere(LambdaExpression expression)
        {
            var visitor = new LambdaVisitor(expression);

            visitor.Visit()
                .First()
                .Visit();

            return visitor.Sql.ToString();
        }
    }
}
