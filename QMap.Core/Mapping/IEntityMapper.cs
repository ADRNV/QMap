using System.Data;

namespace QMap.Core.Mapping
{
    public interface IEntityMapper<T> where T: class, new()
    {
        T Map<T>(IDataReader dataReader) where T : class, new();
    }
}
