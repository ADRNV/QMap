using QMap.SqlBuilder.Abstractions;
using QMap.SqlBuilder.Visitors;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace QMap.SqlBuilder
{
    public class StatementsBuilders : IQueryBuilder
    {
        private string _sql = "";

        private bool _canBeTerminalStatement;

        public bool CanBeTerminalStatement
        {
            get => _canBeTerminalStatement;

            set
            {
                _canBeTerminalStatement = value;
            }
        }

        public string Sql
        {
            get => _sql;

            set => _sql = value;
        }

        public string Build()
        {
            if (!CanBeTerminalStatement) throw new InvalidOperationException();

            return _sql;
        }
    }

    public class SelectBuilder : StatementsBuilders, ISelectBuilder
    {
        private string _sql = "";

        public string Sql
        {
            get => _sql;

            set => _sql = value;
        }

        public ISelectBuilder BuidSelect(Type type)
        {
            _sql += "select * ";

            return this;
        }

        public ISelectBuilder BuidSelect(Expression type)
        {
            //TODO Add selecting by members list and expression
            _sql += "select * ";

            return this;
        }

        public string Build()
        {
            throw new InvalidOperationException();
        }
    }

    public class FromBuilder : StatementsBuilders, IFromBuilder
    {
        private ConcurrentDictionary<string, string> _aliases = new ConcurrentDictionary<string, string>();

        internal bool CanBeTerminalStatement { get => true; }

        private string _sql = "";

        public string Sql
        {
            get => _sql;
            set => _sql = value;
        }

        public IFromBuilder BuildFrom(ISelectBuilder quryBuilder, Type entity, params Type[] entities)
        {
            _sql += $" from {entity.Name} " + _aliases.GetOrAdd(entity.Name, (ak) => NameToAlias(entity.Name));

            return this;
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

        public string Build()
        {
            return _sql;
        }
    }

    public class WhereBuilder : StatementsBuilders, IWhereBuilder
    {
        private string _sql = "";

        public string Sql
        {
            get => _sql;

            set => _sql = value;
        }

        public IWhereBuilder BuildWhere(IFromBuilder fromBuilder, LambdaExpression expression)
        {
            if (expression.ReturnType != typeof(bool))
            {
                throw new ArgumentException("Only bool expressions can be translated to WHERE expression");
            }

            var visitor = new LambdaVisitor(expression);

            visitor.Visit()
                .First()
                .Visit();

            fromBuilder.Sql += " where " + visitor.Sql.ToString();

            return this;
        }

        public string Build()
        {
            return Sql;
        }
    }
}
