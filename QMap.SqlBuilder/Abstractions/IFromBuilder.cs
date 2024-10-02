using QMap.Core.Dialects;
using System.Collections.Concurrent;

namespace QMap.SqlBuilder.Abstractions
{
    public interface IFromBuilder : IQueryBuilder
    {
        ConcurrentDictionary<string, string> Aliases { get; }

        public IFromBuilder BuildFrom(ISelectBuilder quryBuilder, Type entity, params Type[] entities);     
    }
}
