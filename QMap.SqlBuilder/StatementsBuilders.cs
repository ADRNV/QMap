using QMap.Core.Dialects;
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
        public ISqlDialect SqlDialect { get; }

        public StatementsBuilders(ISqlDialect sqlDialect)
        {
            SqlDialect = sqlDialect != null ? sqlDialect : new SqlDialectBase();
        }

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

        public SelectBuilder(ISqlDialect sqlDialect) : base(sqlDialect)
        {
        }

        public string Sql
        {
            get => _sql;

            set => _sql = value;
        }

        public ISelectBuilder BuidSelect(Type type)
        {
            Sql += "select * ";

            return this;
        }

        public ISelectBuilder BuidSelect(Expression type)
        {
            //TODO Add selecting by members list and expression
            Sql += "select * ";

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

        public ConcurrentDictionary<string, string> Aliases
        {
            get => _aliases;
        }

        internal bool CanBeTerminalStatement { get => true; }

        private string _sql = "";

        public FromBuilder(ISqlDialect sqlDialect) : base(sqlDialect)
        {
        }

        public string Sql
        {
            get => _sql;
            set => _sql = value;
        }

        public IFromBuilder BuildFrom(ISelectBuilder quryBuilder, Type entity, params Type[] entities)
        {
            this.Sql += $"{quryBuilder.Sql}" + $" from {entity.Name} " + _aliases.GetOrAdd(entity.Name, (ak) => NameToAlias(entity.Name));

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

        public WhereBuilder(ISqlDialect sqlDialect) : base(sqlDialect)
        {
        }

        public string Sql
        {
            get => _sql;

            set => _sql = value;
        }

        public IWhereBuilder BuildWhere<T>(IFromBuilder fromBuilder, LambdaExpression expression)
        {
            var visitor = new LambdaVisitor(expression, SqlDialect);

            visitor.Visit()
                .First()
                .Visit();

            this.Sql += $"{fromBuilder.Sql}" + " where " + PushAliases(visitor.Sql.ToString(), fromBuilder.Aliases);

            return this;
        }

        private string PushAliases(string sql, ConcurrentDictionary<string, string> aliases)
        {
            var withlAliases = "";

            foreach (var type in aliases.Keys)
            {
                withlAliases = sql.Replace(type, aliases[type]);
            }

            return withlAliases;
        }

        public string Build()
        {
            return Sql;
        }
    }
}
