using System.Linq.Expressions;
namespace QMap.SqlBuilder
{
    public interface IQueryBuilder
    {
         void BuildWhere(LambdaExpression expression);

        void BuildFrom(Type entity, params Type[] entities);

        void BuidSelect(Type entity);

        string Sql {  get; }
    }
}
