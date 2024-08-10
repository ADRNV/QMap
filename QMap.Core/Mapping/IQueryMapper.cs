using System.Data;

namespace QMap.Core.Mapping
{
    public interface IQueryMapper
    {
        IEnumerable<T> Map<T>(IDataReader dataReader) where T : class, new();
    }
}
