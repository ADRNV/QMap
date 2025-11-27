using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;

namespace QMap.Core.Mapping
{
    public interface IEntityMapper
    {
        T Map<T>(IDataReader dataReader) where T : class, new();

        void IsMatchToTable(IDataReader dataReader, ReadOnlyCollection<PropertyInfo> properties);
    }
}
