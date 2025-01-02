using QMap.Core.Dialects;
using System.Linq.Expressions;
using System.Reflection;
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
            //True when lambda interact with global variable
            if (_node.Member.MemberType == System.Reflection.MemberTypes.Field)
            {
                var name = _node.Member.Name;

                var constantExpression = _node.Expression as ConstantExpression;

                var field = _node.Member as FieldInfo;

                var value = field.GetValue(constantExpression.Value);

                if (_sqlDialect.RequireMapping(value))
                {
                    _sql.Append($" {_sqlDialect.Map(value)}");
                }
                else
                {
                    _sql.Append($" {value} ");
                }
            }
            else
            {
               _sql.Append($" {_node.Member.DeclaringType.Name}.{_node.Member.Name} ");
            }

            return null;
        }
    }
}
