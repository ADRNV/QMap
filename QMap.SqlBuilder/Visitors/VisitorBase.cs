using QMap.Core.Dialects;
using System.Linq.Expressions;
using System.Text;

namespace QMap.SqlBuilder.Visitors
{
    public abstract class VisitorBase<E> : IVisitor where E : Expression
    {
        protected readonly E _node;

        protected readonly Type[] _constantTypes = new[] { typeof(bool) };

        protected readonly ISqlDialect _sqlDialect;

        public VisitorBase(E node, ISqlDialect sqlDialect)
        {
            _node = node;

            _sqlDialect = sqlDialect;
        }

        public abstract IEnumerable<IVisitor> Visit();

        public ExpressionType NodeType => _node.NodeType;

        public static IVisitor CreateFromExpression(Expression node, ref StringBuilder stringBuilder, ISqlDialect sqlDialect = null)
        {

            switch (node.NodeType)
            {
                case ExpressionType.Constant:
                    return new ConstantVisitor((ConstantExpression)node, ref stringBuilder, sqlDialect);
                case ExpressionType.Lambda:
                    return new LambdaVisitor((LambdaExpression)node, sqlDialect);
                case ExpressionType.Parameter:
                    return new ParameterVisitor((ParameterExpression)node, ref stringBuilder, sqlDialect);
                case ExpressionType.Or:
                case ExpressionType.And:
                case ExpressionType.Equal:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.AndAlso:
                    return new BinaryVisitor((BinaryExpression)node, ref stringBuilder, sqlDialect);
                case ExpressionType.MemberAccess:
                    return new MemberVisitor((MemberExpression)node, ref stringBuilder, sqlDialect);
                default:
                    throw new InvalidOperationException("Cant find node type");
            }
        }
    }

}
