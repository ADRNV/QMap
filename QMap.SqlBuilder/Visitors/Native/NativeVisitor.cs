using QMap.Core.Dialects;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace QMap.SqlBuilder.Visitors.Native
{
    public class NativeVisitor : ExpressionVisitor
    {
        public StringBuilder Sql
        {
            get;
        }

        protected readonly Type[] _constantTypes = new[] { typeof(bool) };

        protected readonly ISqlDialect _sqlDialect;

        public NativeVisitor(ISqlDialect sqlDialect)
        {
            _sqlDialect = sqlDialect;

            Sql = new StringBuilder();
        }

        public Expression VisitPredicateLambda<T>(Expression<Func<T, bool>> node)
        {
            return VisitLambda(node);
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            switch (node.Body.NodeType)
            {
                case ExpressionType.Constant:
                    VisitConstant((ConstantExpression)node.Body);
                    break;
                case ExpressionType.Lambda:
                    VisitLambda(node);
                    break;
                case ExpressionType.Parameter:
                    VisitParameter((ParameterExpression)node.Body);
                    break;
                case ExpressionType.Or:
                case ExpressionType.And:
                case ExpressionType.Equal:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.AndAlso:
                case ExpressionType.NotEqual:
                    VisitBinary((BinaryExpression)node.Body);
                    break;
                case ExpressionType.MemberAccess:
                    VisitMember((MemberExpression)node.Body);
                    break;
                default:
                    throw new InvalidOperationException("Cant find node type");
            }

            return node;
        }

        protected override Expression VisitConstant(ConstantExpression constantExpression)
        {
            if (_sqlDialect.RequireMapping(constantExpression.Value))
            {
                Sql.Append($" {_sqlDialect.Map(constantExpression.Value)}");
            }
            else
            {
                Sql.Append($" {constantExpression.Value} ");
            }

            return constantExpression; 
        }

        protected override Expression VisitBinary(BinaryExpression binaryExpression)
        {
            Expression left = this.Visit(binaryExpression.Left);
            Sql.Append($"{MapOperators(binaryExpression.NodeType)}");
            Expression right = this.Visit(binaryExpression.Right);
            
            return binaryExpression;
        }

        protected override Expression VisitMember(MemberExpression memeberExpression)
        {
            //TODO NOW!:
            //Write logic for get diffrence between Property member type(like 'e.Property')
            //and member expression with closure class(like <>c__DisplayClass) and property getter expression compilation opportunity
            if (memeberExpression.Member.MemberType == MemberTypes.Field ||
                (memeberExpression.Member.MemberType == MemberTypes.Property))
            {
                //Brute try
                try
                {
                    var value = GetMemberValue(memeberExpression);

                    if (_sqlDialect.RequireMapping(value))
                    {
                        Sql.Append($" {_sqlDialect.Map(value)}");
                    }
                    else
                    {
                        Sql.Append($" {value} ");
                    }
                }
                catch
                {
                    Sql.Append($" {memeberExpression.Member.DeclaringType.Name}.{memeberExpression.Member.Name} ");
                }
            }
            else
            {
                Sql.Append($" {memeberExpression.Member.DeclaringType.Name}.{memeberExpression.Member.Name} ");
            }

            return memeberExpression;
        }

        protected object GetMemberValue(MemberExpression memberExpression)
        {
            //GetInstance
            var objectExpression = memberExpression.Expression;

            var lambda = Expression.Lambda<Func<object>>(
                Expression.Convert(objectExpression, typeof(object))
            );

            var getInstance = lambda.Compile();
            object instance = getInstance.Invoke();

            var member = memberExpression.Member;
            //External value
            if (member is FieldInfo field)
            {
                return field?.GetValue(instance);
            }

            else if (member is PropertyInfo property)
            {
                return property?.GetValue(instance);
            }
            else
            {
                throw new InvalidOperationException("Cant visit member of type "+ memberExpression.Member.MemberType);
            }
        }

        protected override Expression VisitParameter(ParameterExpression parameterExpression)
        {
            Sql.Append($" @{parameterExpression.Name}");

            return parameterExpression;
        }

        //Inject SQL Dialect ? 
        protected string MapOperators(ExpressionType expressionType)
        {
            return expressionType switch
            {
                ExpressionType.Equal => "=",
                ExpressionType.NotEqual => "!=",
                ExpressionType.GreaterThan => ">",
                ExpressionType.GreaterThanOrEqual => ">=",
                ExpressionType.LessThanOrEqual => "<=",
                ExpressionType.LessThan => "<",
                ExpressionType.AndAlso => "and",
                ExpressionType.Or => "or",
                _ => throw new InvalidOperationException("Unknown operator")
            };
        }

    }
}
