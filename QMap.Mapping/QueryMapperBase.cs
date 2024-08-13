using QMap.Core.Mapping;
using System.Data;

namespace QMap.Mapping
{
    public class QueryMapperBase<T> : IQueryMapper where T : class, new()
    {
        private readonly IEntityMapper<T> _entityMapper;

        public QueryMapperBase(IEntityMapper<T> entityMapper)
        {
            _entityMapper = entityMapper;
        }

        public virtual IEnumerable<T> Map<T>(IDataReader dataReader) where T : class, new() 
        {
            List<T> rows = new List<T>();

            while (dataReader.Read())
            {
                rows.Add(_entityMapper.Map<T>(dataReader));
            }

            return rows;
        }    
    }
}