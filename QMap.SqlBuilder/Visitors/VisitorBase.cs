using System.Linq.Expressions;
using System.Text;

namespace QMap.SqlBuilder.Visitors
{
    public abstract class VisitorBase<E> : IVisitor where E : Expression
    {
        protected readonly E _node;

        public VisitorBase(E node)
        {
            _node = node;
        }

        public abstract IEnumerable<IVisitor> Visit();

        public ExpressionType NodeType => _node.NodeType;

        public static IVisitor CreateFromExpression(Expression node, ref StringBuilder stringBuilder)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Constant:
                    return new ConstantVisitor((ConstantExpression)node, ref stringBuilder);
                case ExpressionType.Lambda:
                    return new LambdaVisitor((LambdaExpression)node);
                case ExpressionType.Parameter:
                    return new ParameterVisitor((ParameterExpression)node, ref stringBuilder);
                case ExpressionType.Or:
                case ExpressionType.And:
                case ExpressionType.Equal:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.AndAlso:
                    return new BinaryVisitor((BinaryExpression)node, ref stringBuilder);
                case ExpressionType.MemberAccess:
                    return new MemberVisitor((MemberExpression)node, ref stringBuilder);
                default:
                    throw new InvalidOperationException("Cant find node type");
            }
        }
    }

}
