using System.Data;

namespace QMap.Core.Mapping
{
    public interface IProjectionMapper<TResult>
    {
        IEnumerable<TResult> Map(IDataReader dataReader);
    }
}
