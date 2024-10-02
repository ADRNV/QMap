using QMap.Core.Dialects;

namespace QMap.SqlBuilder.Abstractions
{
    public interface IQueryBuilder
    {
        string Sql { get; internal set; }

        bool CanBeTerminalStatement { get; internal set; }

        internal ISqlDialect SqlDialect { get; }

        string Build();
    }
}
