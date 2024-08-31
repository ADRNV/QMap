using System.Linq.Expressions;
using System.Text;

namespace QMap.SqlBuilder.Visitors
{
    public class ConstantVisitor : VisitorBase<ConstantExpression>
    {
        public StringBuilder _sql;
        public ConstantExpression ConstantNode { get; }

        public ConstantVisitor(ConstantExpression constantExpression, ref StringBuilder stringBuilder) : base(constantExpression)
        {
            ConstantNode = constantExpression;

            _sql = stringBuilder;
        }

        public override IEnumerable<IVisitor> Visit()
        {
            _sql.Append($" {ConstantNode.Value} ");

            return null;
        }
    }
}
