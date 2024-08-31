using System.Linq.Expressions;
using System.Text;
using System.Xml.Linq;

namespace QMap.SqlBuilder.Visitors
{
    internal class LambdaVisitor : VisitorBase<LambdaExpression>
    {
        public StringBuilder Sql = new StringBuilder();

        public LambdaVisitor(LambdaExpression lambdaExpression) : base(lambdaExpression)
        {

        }

        public override IEnumerable<IVisitor> Visit()
        {
            var argumentVisitors = new List<IVisitor>();

            //foreach (var argumentExpression in base._node.Parameters)
            //{
            //    var argumentVisitor = CreateFromExpression(argumentExpression);
            //    argumentVisitors.Add(CreateFromExpression(argumentExpression));
            //    argumentVisitor.Visit();
            //}

            return new IVisitor[] {
                
                CreateFromExpression(_node.Body, ref Sql)
            };
        }
    }
}
