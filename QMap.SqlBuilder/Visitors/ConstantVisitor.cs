using QMap.Core.Dialects;
using System.Linq.Expressions;
using System.Text;

namespace QMap.SqlBuilder.Visitors
{
    public class ConstantVisitor : VisitorBase<ConstantExpression>
    {
        public StringBuilder _sql;
        public ConstantExpression ConstantNode { get; }

        public ConstantVisitor(ConstantExpression constantExpression, ref StringBuilder stringBuilder, ISqlDialect sqlDialect) : base(constantExpression, sqlDialect)
        {
            ConstantNode = constantExpression;

            _sql = stringBuilder;
        }

        public override IEnumerable<IVisitor> Visit()
        {
            if (_constantTypes.Contains(ConstantNode.Type))
            {
                _sql.Append($" {_sqlDialect.Constants[ConstantNode.Value?.ToString()]} ");
            }
            else
            {
                _sql.Append($" {ConstantNode.Value} ");
            }
          
            return null;
        }
    }
}
