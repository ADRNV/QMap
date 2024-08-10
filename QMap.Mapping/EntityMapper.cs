using QMap.Core.Mapping;
using System.Data;
using System.Reflection;

namespace QMap.Mapping
{
    public class EntityMapper<T> : IEntityMapper<T> where T: class, new()
    {
        public virtual T Map<T>(IDataReader dataReader) where T : class, new()
        {
            var typeInfo = typeof(T);

            var props = typeInfo.GetProperties(BindingFlags.Public);

            var instance = new T();

            foreach (var prop in props)
            {
                var columnValue = dataReader.GetFromColumn(prop.PropertyType, prop.Name);

                prop.SetValue(instance, columnValue);
            }

            return instance;
        }
    }
}
