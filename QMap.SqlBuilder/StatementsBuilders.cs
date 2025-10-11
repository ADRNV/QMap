using QMap.Core.Dialects;
using QMap.SqlBuilder.Abstractions;
using QMap.SqlBuilder.Visitors;
using QMap.SqlBuilder.Visitors.Native;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace QMap.SqlBuilder
{
    public class StatementsBuilders : IQueryBuilder
    {
        private string _sql = "";

        private bool _canBeTerminalStatement;
        public ISqlDialect SqlDialect { get; }

        public Dictionary<string, object> Parameters { get; protected set; } = new Dictionary<string, object>();

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

        public IFromBuilder BuildFrom(IDeleteBuilder quryBuilder, Type entity, params Type[] entities)
        {
            this.Sql += $"{quryBuilder.Sql}" + $" from {entity.Name} ";

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
            var visitor = new NativeVisitor(SqlDialect);
         
            visitor.VisitPredicateLambda((Expression<Func<T, bool>>)expression);

            Parameters = visitor.Parameters;

            this.Sql += $"{fromBuilder.Sql}" + " where \n" + PushAliases(visitor.Sql.ToString(), fromBuilder.Aliases);

            return this;
        }

        public IWhereBuilder BuildWhere<T>(IUpdateBuilder fromBuilder, LambdaExpression expression)
        {
            var visitor = new NativeVisitor(SqlDialect);

            visitor.VisitPredicateLambda((Expression<Func<T, bool>>)expression);
               
            this.Sql += $"{fromBuilder.Sql}" + " where " + visitor.Sql.ToString();

            return this;
        }

        private string PushAliases(string sql, ConcurrentDictionary<string, string> aliases)
        {
            var withlAliases = "";

            foreach (var type in aliases.Keys)
            {
                withlAliases = sql.Replace(type, aliases[type]);
            }

            return string.IsNullOrEmpty(withlAliases) ? sql : withlAliases;
        }

        public string Build()
        {
            return Sql;
        }
    }

    public class InsertBuilder : StatementsBuilders, IInsertBuilder
    {
        public InsertBuilder(ISqlDialect sqlDialect) : base(sqlDialect)
        {
        }

        public string Build()
        {
            return Sql;
        }

        public IInsertBuilder BuildInsert<T>(T entity)
        {
            var columns = BuildColumns<T>();
            var values = BuildValues(entity, columns);

            Sql = $"insert into {typeof(T).Name} " +
                $"({columns.Aggregate((c1, c2) => $"{c1},{c2}")})"
                + "values"
                + $"({values.Aggregate((v1, v2) => $"{v1},{v2}")})";


            return this;
        }

        public IInsertBuilder BuildInsertExcept<T>(T entity, Func<PropertyInfo, bool> exceptPropsFilter)
        {
            var columns = BuildColumns<T>(exceptPropsFilter);
            var values = BuildValues(entity, columns);

            Sql = $"insert into {typeof(T).Name} " +
                $"({columns.Aggregate((c1, c2) => $"{c1},{c2}")})"
                + "values"
                + $"({values.Aggregate((v1, v2) => $"{v1},{v2}")})";

            return this;
        }

        private IEnumerable<string> BuildColumns<T>(Func<PropertyInfo, bool>? exceptPropsFilter = null)
        {
            var properties = typeof(T)
                  .GetProperties(BindingFlags.Public
                     | BindingFlags.GetProperty
                     | BindingFlags.SetProperty
                     | BindingFlags.Instance)
                  .AsEnumerable();

            if (exceptPropsFilter != null)
            {
                properties = properties.Where(p => !exceptPropsFilter.Invoke(p));
            }

            return properties
                 .Select(p => p.Name);
        }

        private IEnumerable<string> BuildValues<T>(T entity, IEnumerable<string> columns)
        {
            var properties = typeof(T)
                  .GetProperties(BindingFlags.Public
                     | BindingFlags.GetProperty
                     | BindingFlags.SetProperty
                     | BindingFlags.Instance)
                  .AsEnumerable();

#nullable disable
            return properties
                .Where(p => columns.Contains(p.Name))
                .Select(p =>
                {

                    Parameters.Add(SqlDialect.ParameterName + p.Name, p.GetValue(entity));

                    return $"{SqlDialect.ParameterName}{p.Name}";
                });
        }
    }

    public class UpdateBuilder : StatementsBuilders, IUpdateBuilder
    {
        public UpdateBuilder(ISqlDialect sqlDialect) : base(sqlDialect)
        {
        }

        public IUpdateBuilder BuildUpdate<T, V>(Expression<Func<V>> propertySelector, V value)
        {
            var memberExpression = propertySelector.Body as MemberExpression;

            if (memberExpression is null) throw new InvalidOperationException("Delegate must return property of object");

            var typeInfo = memberExpression.Member.DeclaringType;

            var property = (PropertyInfo)memberExpression.Member;

            Sql = $"update {typeInfo.Name} set ";

            var parameterKey = SqlDialect.ParameterName + property.Name;
            Parameters.Add(parameterKey, value);

            Sql += $"{property.Name} = {parameterKey}";
            return this;       
        }
    }

    public class DeleteBuilder : StatementsBuilders, IDeleteBuilder
    {
        public DeleteBuilder(ISqlDialect sqlDialect) : base(sqlDialect)
        {
        }

        public Dictionary<string, object> Parameters => throw new NotImplementedException();

        private Type _entity = null;

        public string Build()
        {
            throw new InvalidOperationException();
        }

        public IDeleteBuilder BuildDelete<T>()
        {
            this.Sql = "delete";

            _entity = typeof(T);

            return this;
        }

        public IFromBuilder BuildFrom()
        {
            return new FromBuilder(this.SqlDialect)
                .BuildFrom(this, _entity, null);
        }
    }
}
