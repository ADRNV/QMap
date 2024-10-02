using QMap.Core.Dialects;
using System.Linq.Expressions;
using System.Text;

namespace QMap.SqlBuilder.Visitors
{
    public class MemberVisitor : VisitorBase<MemberExpression>
    {
        private MemberExpression _memberExpression;

        private StringBuilder _sql { get; set; }

        public MemberVisitor(MemberExpression node, ref StringBuilder stringBuilder, ISqlDialect sqlDialect) : base(node, sqlDialect)
        {
            _memberExpression = node;

            _sql = stringBuilder;
        }

        public override IEnumerable<IVisitor> Visit()
        {
            _sql.Append($" {_node.Member.DeclaringType.Name}.{_node.Member.Name} ");

            return null;
        }
    }
}
