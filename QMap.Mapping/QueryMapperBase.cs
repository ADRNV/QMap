using QMap.Core.Mapping;
using System.Data;
using System.Reflection;
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
            var typeInfo = typeof(T);

            var props = typeInfo.GetProperties(BindingFlags.Public);

            var instance = new T();

            T[] rows = new T[dataReader.RecordsAffected];

            rows.ToList().Select(e =>
            {
                dataReader.Read();
                return _entityMapper.Map<T>(dataReader);
            });

            return rows.AsEnumerable();
        }    
    }
}