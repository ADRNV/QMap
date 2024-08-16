using QMap.Core.Mapping;
using System.Data;
using System.Reflection;

namespace QMap.Mapping
{
    public class EntityMapperBase : IEntityMapper
    {
        public virtual T Map<T>(IDataReader dataReader) where T : class, new()
        {
            var typeInfo = typeof(T);

            var props = typeInfo.GetProperties(
                BindingFlags.Public
                | BindingFlags.GetProperty
                | BindingFlags.SetProperty
                | BindingFlags.Instance);

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
