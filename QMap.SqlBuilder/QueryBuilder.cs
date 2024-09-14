using QMap.SqlBuilder.Visitors;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace QMap.SqlBuilder
{
    public class QueryBuilder : IQueryBuilder
    {
        private ConcurrentDictionary<string, string> _aliases = new ConcurrentDictionary<string, string>();

        private string _sql = "";

        public string Sql 
        {
            get => _sql;
        }

        public void BuildWhere(LambdaExpression expression)
        {
            if(expression.ReturnType != typeof(bool))
            {
                throw new ArgumentException("Only bool expressions can be translated to WHERE expression");
            }

            var visitor = new LambdaVisitor(expression);

            visitor.Visit()
                .First()
                .Visit();

            _sql += " where " + visitor.Sql.ToString();
        }

        public void BuildFrom(Type entity, params Type[] entities)
        {
            _sql += $" from {entity.Name} " + _aliases.GetOrAdd(entity.Name, (ak) => NameToAlias(entity.Name));
        }

        public void BuidSelect(Type type)
        {
            //TODO Add selecting by members list and expression
            _sql += "select * ";
        }

        private string NameToAlias(string name, int skips = 3)
        {
            string alias = name;

            alias = new string(name
                .ToLower()
                .AsEnumerable()
                .Where(c => alias.IndexOf(c) % skips == 0)
                .ToArray());

            return alias;
        }
    }
}
