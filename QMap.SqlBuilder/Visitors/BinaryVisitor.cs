using System.Linq.Expressions;
using System.Text;

namespace QMap.SqlBuilder.Visitors
{
    internal class BinaryVisitor : VisitorBase<BinaryExpression>
    {
        private StringBuilder _sql;

        public BinaryVisitor(BinaryExpression binaryExpression, ref StringBuilder stringBuilder) : base(binaryExpression)
        {
            _sql = stringBuilder;
        }

        public override IEnumerable<IVisitor> Visit()
        {
            _sql.Append("(");

            var right = CreateFromExpression(_node.Left, ref _sql)
                .Visit();

            _sql.Append(MapOperators(_node.NodeType));

            var left = CreateFromExpression(_node.Right, ref _sql)
               .Visit();

            _sql.Append(")");

            return null;
        }

        protected string MapOperators(ExpressionType expressionType)
        {
            return expressionType switch
            {
                ExpressionType.Equal => "=",
                ExpressionType.NotEqual => "!=",
                ExpressionType.GreaterThan => ">",
                ExpressionType.LessThan => "<",
                ExpressionType.AndAlso => "and",
                ExpressionType.Or => "or",
                _ => throw new InvalidOperationException("Unknown operator")
            };
        }
    }
}
