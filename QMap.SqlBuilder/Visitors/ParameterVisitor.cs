using System.Linq.Expressions;
using System.Text;

namespace QMap.SqlBuilder.Visitors
{
    public class ParameterVisitor : VisitorBase<ParameterExpression>
    {
        private StringBuilder _sql {  get; set; }

        public ParameterExpression ParameterExpression { get; set; }

        public ParameterVisitor(ParameterExpression parameterExpression, ref StringBuilder stringBuilder) : base(parameterExpression)
        {
            ParameterExpression = parameterExpression;

            _sql = stringBuilder;
        }

        public override IEnumerable<IVisitor> Visit()
        {
            _sql.Append($" @{ParameterExpression.Name}");

            return null;
        }
    }
}
