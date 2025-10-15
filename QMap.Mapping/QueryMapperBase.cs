using QMap.Core.Mapping;
using QMap.Mapping.Enumerable;
using System.Data;

namespace QMap.Mapping
{
    public class QueryMapperBase : IQueryMapper
    {
        private readonly IEntityMapper _entityMapper;

        public QueryMapperBase(IEntityMapper entityMapper)
        {
            _entityMapper = entityMapper;
        }

        public virtual IEnumerable<T> Map<T>(IDataReader dataReader) where T : class, new()
        {
            var enumerator = new ReaderEnumerator<T>(dataReader, _entityMapper);

            var rows = new EnumerableReader<T>(enumerator);

            return rows;
        }
    }
}