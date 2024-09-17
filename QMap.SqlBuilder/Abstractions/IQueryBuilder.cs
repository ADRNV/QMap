using System.Linq.Expressions;

namespace QMap.SqlBuilder.Abstractions
{
    public interface IQueryBuilder
    {
        string Sql { get; internal set; }

        bool CanBeTerminalStatement { get; internal set; }

        string Build();
    }
}
